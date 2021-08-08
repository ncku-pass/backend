using Api.Profiles;
using Application.Services;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Database;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Infrastructure.Service;
using Infrastructure.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = true;
                })
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            // Reverse_Proxy forwardheader設定
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            // Add MySQL相關設定
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseMySql(this._configuration.GetConnectionString("MySQL"));
            });

            // Add HttpClient調用外部Api
            services.AddHttpClient();

            // TODO:問家駿這邊該怎麼簡化
            // DI註冊
            // Service用Scoped:每個Request刷新
            // Repo用Transient:每個子任務刷新
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBaseRepository<Experience>, BaseRepository<Experience>>();
            services.AddTransient<IBaseRepository<Experience_Tag>, BaseRepository<Experience_Tag>>();
            services.AddTransient<IBaseRepository<Tag>, BaseRepository<Tag>>();
            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
            services.AddTransient<IBaseRepository<Resume>, BaseRepository<Resume>>();
            services.AddTransient<IBaseRepository<Card>, BaseRepository<Card>>();
            services.AddTransient<IBaseRepository<Card_Experience>, BaseRepository<Card_Experience>>();

            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IExperienceService, ExperienceService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IResumeService, ResumeService>();
            services.AddScoped<INCKUPortalService, NCKUPortalService>();

            services.AddScoped<INCKUPortalAPI, NCKUPortalAPI>();

            // Add Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ExperienceProfile());
                mc.AddProfile(new TagProfile());
                mc.AddProfile(new AuthenticationProfile());
                mc.AddProfile(new ResumeProfile());
                mc.AddProfile(new NCKUPortalProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Add Swagger相關設定
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, new string[] { } } });
            });

            // JWT登入服務注入
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        // TODO:待網域確認改回True
                        ValidateIssuer = false,
                        ValidIssuer = _configuration["Authentication:Issuer"],
                        ValidateAudience = false,
                        ValidAudience = _configuration["Authentication:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["JWTToken"];
                            return Task.CompletedTask;
                        },
                    };
                });

            // 使用Asp.Net core Identity身分認證框架
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });

            // 跨域請求Cors設定
            string[] corsOrigins = this._configuration["Cors:AllowOrigin"].Split(',', StringSplitOptions.RemoveEmptyEntries);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(corsOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Update-database migration
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            // Reverse_Proxy forwardheader設定
            app.UseForwardedHeaders();
            app.UseCertificateForwarding();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            // app.UseHttpsRedirection();

            // 路徑引導
            app.UseRouting();
            // 跨域請求規則
            app.UseCors("CorsPolicy");
            // 確認身分
            app.UseAuthentication();
            // 權限組態
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
