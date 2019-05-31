using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebAppCore.BLL.Helpers;

namespace WebAppCore.BLL.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerGenServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "WebAppCore Project",
                    Description = "WebAppCore API Swagger surface",
                    Contact = new Contact {Name = "Petro Krasnyuk", Email = "pkrasnyuk@hotmail.com"},
                    License = new License {Name = "MIT"}
                });

                s.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                s.OperationFilter<FileOperationFilter>();
            });

            return services;
        }
    }
}