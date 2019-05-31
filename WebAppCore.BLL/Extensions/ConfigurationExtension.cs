using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAppCore.BLL.Helpers;
using WebAppCore.DAL.CacheHelpers;
using WebAppCore.DAL.DataAccessHelpers;

namespace WebAppCore.BLL.Extensions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.Configure<DataAccessConfiguration>(configuration.GetSection("DataAccess"));
            services.Configure<TokenAuthentication>(configuration.GetSection("TokenAuthentication"));
            services.Configure<List<CacheSettings>>(configuration.GetSection("CacheSettings"));

            return services;
        }
    }
}