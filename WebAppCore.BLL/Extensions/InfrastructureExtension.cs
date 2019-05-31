using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Managers;
using WebAppCore.DAL.Interfaces;
using WebAppCore.DAL.Repositories;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(),
                sp.GetService));

            services.AddScoped<IEntityRepository<Album<ObjectId>, ObjectId>, AlbumRepository>();
            services.AddScoped<IEntityRepository<Photo<ObjectId>, ObjectId>, PhotoRepository>();
            services.AddScoped<IEntityRepository<Role<ObjectId>, ObjectId>, RoleRepository>();
            services.AddScoped<IEntityRepository<User<ObjectId>, ObjectId>, UserRepository>();
            services.AddScoped<IEntityRepository<UserRole<ObjectId>, ObjectId>, UserRoleRepository>();

            services.AddSingleton<IEntityRepository<Error<ObjectId>, ObjectId>, ErrorRepository>();

            services.AddScoped<IAlbumsManager, AlbumsManager>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IPhotosManager, PhotosManager>();
            services.AddScoped<IRegisterManager, RegisterManager>();
            services.AddScoped<IUsersManager, UsersManager>();

            return services;
        }
    }
}