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

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IConfiguration _configuration { get; }

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

            // Add MySQL�����]�w
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseMySql(this._configuration["DbContext:MySQLConnectionString"]);
            });

            // TODO:�ݮa�@�o��ӫ��²��
            // DI���U
            // Service��Scoped:�C��Request��s
            // Repo��Transient:�C�Ӥl���Ȩ�s
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBaseRepository<Experience>, BaseRepository<Experience>>();
            services.AddTransient<IBaseRepository<Tag>, BaseRepository<Tag>>();
            services.AddTransient<IBaseRepository<Tag_Experience>, BaseRepository<Tag_Experience>>();
            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();

            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IExperienceService, ExperienceService>();
            services.AddScoped<ITagService, TagService>();

            // Add Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ExperienceProfile());
                mc.AddProfile(new TagProfile());
                mc.AddProfile(new AuthenticationProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Add Swagger�����]�w
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
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, new string[] { } } });
            });

            // JWT�n�J�A�Ȫ`�J
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Authentication:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = _configuration["Authentication:Audience"],

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });

            // �ϥ�Asp.Net core Identity�����{�Үج[
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO:Docker�]�w�����ܼ�
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

            // ���|�޾�
            app.UseRouting();
            // �T�{����
            app.UseAuthentication();
            // �v���պA
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}