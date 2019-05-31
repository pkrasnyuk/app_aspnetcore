using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebAppCore.BLL.Managers;

namespace WebAppCore.BLL.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetSection("TokenAuthentication:Issuer").Value,

                        ValidateAudience = true,
                        ValidAudience = configuration.GetSection("TokenAuthentication:Audience").Value,

                        ValidateLifetime = true,

                        IssuerSigningKey =
                            SecurityManager.GetSymmetricSecurityKey(configuration.GetSection("TokenAuthentication:Key")
                                .Value),
                        ValidateIssuerSigningKey = true
                    };
                })
                .AddCookie(options => { options.ExpireTimeSpan = TimeSpan.FromHours(1); });

            return services;
        }
    }
}