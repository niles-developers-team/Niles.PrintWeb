﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Niles.PrintWeb.Data;
using Niles.PrintWeb.Data.Enumerations;
using Niles.PrintWeb.Data.Interfaces;
using Niles.PrintWeb.Api.Services;
using Niles.PrintWeb.Shared;

namespace Niles.PrintWeb.Api
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("*");
                });
            });
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
            var appSettings = appSettingsSection.Get<ApplicationSettings>();

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services
            .AddAuthentication()
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

            string connectionString = SolutionSettings.MSSqlServerConnectionString;

            services.AddScoped(provider =>
            {
                var logger = provider.GetService<ILogger<IDaoFactory>>();
                return DaoFactories.GetFactory(DataProvider.MSSql, connectionString, logger);
            });

            services.AddScoped(provider =>
            {
                var logger = provider.GetService<ILogger<EmailService>>();
                return new EmailService(logger, appSettings);
            });

            services.AddScoped(provider =>
            {
                var logger = provider.GetService<ILogger<UserService>>();
                var daoFactory = provider.GetService<IDaoFactory>();
                var emailService = provider.GetService<EmailService>();
                var httpAccessor = provider.GetService<IHttpContextAccessor>();

                return new UserService(daoFactory.UserDao, emailService, appSettings, httpAccessor, logger);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            app.UseMvc();

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
