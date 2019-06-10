namespace SmartSchedule.Api
{
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using SmartSchedule.Api.Filters;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Application.Infrastructure.AutoMapper;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Application.User.Queries.GetUserDetails;
    using SmartSchedule.Infrastructure.UoW;
    using SmartSchedule.Infrastucture.Authentication;
    using SmartSchedule.Infrastucture.Email;
    using SmartSchedule.Persistence;
    using Swashbuckle.AspNetCore.Swagger;

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
            //Automapper
            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            //ReverseProxy configuration
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("13.80.110.174"));
            });

            //Mediator
            {
                services.AddMediatR(typeof(GetUserDetailsQuery.Handler).GetTypeInfo().Assembly);

                services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            }

            //email configruation
            {
                var emailSettings = Configuration.GetSection("EmailSettings");
                services.Configure<EmailSettings>(emailSettings);
            }

            //jwt authentication configuration
            {
                var jwtSettingsSection = Configuration.GetSection("JwtSettings");
                services.Configure<JwtSettings>(jwtSettingsSection);

                var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
                var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            }

            //Database connection
            {
                services.AddDbContext<SmartScheduleDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SmartScheduleDatabase")));

                services.AddTransient<IJwtService, JwtService>();
                services.AddScoped<DbContext, SmartScheduleDbContext>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddTransient<IEmailService, EmailService>();

                services.AddHttpContextAccessor();
            }

            //Cors
            services.AddCors(options => //TODO: Change cors only to our server
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "SmartSchedule Api",
                    Description = "Backend Api for SmartSchedule site",
                    TermsOfService = "None"
                });

                c.AddSecurityDefinition("jwt", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Use /api/login endpoint below to retrive token, then paste it to the textbox below in the following schema \"Bearer {token}\". Example: \"Bearer abcefghi12345\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"jwt", new string[] { }},
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAll");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSchedule V1");
            });
        }
    }
}
