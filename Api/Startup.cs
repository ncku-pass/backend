using Api.Attributes.HeaderFilter;
using Api.Profiles;
using Application.Services;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Database;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            // Add MySQL相關設定
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseMySql(Configuration["DbContext:MySQLConnectionString"]);
            });

            // TODO:問家駿這邊該怎麼簡化
            // DI註冊
            // Service用Scoped:每個Request刷新
            // Repo用Transient:每個子任務刷新
            services.AddScoped<IExperienceService, ExperienceService>();
            services.AddScoped<ITagService, TagService>();
            services.AddTransient<IBaseRepository<Experience>, BaseRepository<Experience>>();
            services.AddTransient<IBaseRepository<Tag>, BaseRepository<Tag>>();
            services.AddTransient<IBaseRepository<Tag_Experience>, BaseRepository<Tag_Experience>>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Add Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ExperienceProfile());
                mc.AddProfile(new TagProfile());
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
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
            });

            // JWT登入服務注入
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:Audience"],

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO:Docker設定環境變數
            if (env.IsDevelopment())
            {
            }
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            // 路徑引導
            app.UseRouting();
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