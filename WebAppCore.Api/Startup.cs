using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebAppCore.BLL.Extensions;
using WebAppCore.BLL.Middleware;

namespace WebAppCore.Api
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurationServices(Configuration);
            services.AddOptions();

            services.AddSwaggerGenServices();
            services.AddAuthenticationServices(Configuration);
            services.AddAuthorizationServices();

            services.AddInfrastructureServices();

            services.AddSignalR();
            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            app.ConfigureApplication(loggerFactory, serviceProvider, Configuration);

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}