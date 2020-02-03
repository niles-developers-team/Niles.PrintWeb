using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Niles.PrintWeb.DataAccessObjects;
using Niles.PrintWeb.DataAccessObjects.Interfaces;
using Niles.PrintWeb.Api.Services;
using Niles.PrintWeb.Models.Settings;
using Niles.PrintWeb.Services;

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
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
            var appSettings = appSettingsSection.Get<Appsettings>();

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
                    ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true, ValidIssuer = appSettings.Issuer,
                    ValidateAudience = false
                };
            });

            var connectionSettings = appSettings.DatabaseConnectionSettings;
            var emailSettings = appSettings.EmailConnectionSettings;

            services.AddScoped(provider =>
            {
                var logger = provider.GetService<ILogger<IDaoFactory>>();
                return DaoFactories.GetFactory(connectionSettings, logger);
            });

            services.AddScoped<IEmailService>(provider =>
            {
                var logger = provider.GetService<ILogger<EmailService>>();
                return new EmailService(logger, emailSettings);
            });

            services.AddScoped<IUserService>(provider =>
            {
                var logger = provider.GetService<ILogger<UserService>>();
                var daoFactory = provider.GetService<IDaoFactory>();
                var emailService = provider.GetService<IEmailService>();
                var httpAccessor = provider.GetService<IHttpContextAccessor>();

                return new UserService(daoFactory.UserDao, emailService, appSettings, httpAccessor, logger);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
