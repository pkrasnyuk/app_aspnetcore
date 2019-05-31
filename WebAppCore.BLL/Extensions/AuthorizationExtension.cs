using System;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using WebAppCore.BLL.Helpers;

namespace WebAppCore.BLL.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Enum.GetName(Policy.AdminOnly.GetType(), Policy.AdminOnly),
                    policy =>
                    {
                        policy.RequireClaim(ClaimTypes.Role,
                            Role.Admin.ToString());
                    });

                options.AddPolicy(Enum.GetName(Policy.RegisteredUser.GetType(), Policy.RegisteredUser),
                    policy =>
                    {
                        policy.RequireClaim(ClaimTypes.Role,
                            Role.Admin.ToString(),
                            Role.User.ToString(),
                            Role.Guest.ToString());
                    });
            });

            return services;
        }
    }
}