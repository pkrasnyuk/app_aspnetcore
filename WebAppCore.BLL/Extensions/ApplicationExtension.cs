using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Publishers;

namespace WebAppCore.BLL.Extensions
{
    public static class ApplicationExtension
    {
        public static IApplicationBuilder ConfigureApplication(this IApplicationBuilder app,
            ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IConfigurationRoot configuration)
        {
            ConfigureLogger(loggerFactory, configuration);

            app.ConfigureCors();
            app.ConfigureSwaggerUi();
            app.ConfigureSignalR();

            DbInitializer.Initialize(serviceProvider, configuration);

            return app;
        }

        private static void ConfigureLogger(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Warning);
            loggerFactory.AddFile(configuration.GetSection("Logging"));

            Utilities.ConfigureLogger(loggerFactory);
        }

        private static IApplicationBuilder ConfigureCors(this IApplicationBuilder app)
        {
            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            return app;
        }

        private static IApplicationBuilder ConfigureSwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAppCore Project API v1.1");
                s.ShowRequestHeaders();
            });

            return app;
        }

        private static IApplicationBuilder ConfigureSignalR(this IApplicationBuilder app)
        {
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationsPublisher>("notificationsPublisher");
            });

            return app;
        }
    }
}