using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using WebAppCore.BLL.Managers;
using WebAppCore.DAL.Interfaces;
using WebAppCore.DAL.Repositories;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Helpers
{
    public static class DbInitializer
    {
        private static IEntityRepository<User<ObjectId>, ObjectId> _userRepository;
        private static IEntityRepository<Role<ObjectId>, ObjectId> _roleRepository;
        private static IEntityRepository<UserRole<ObjectId>, ObjectId> _userRoleRepository;

        public static void Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _userRepository = (IEntityRepository<User<ObjectId>, ObjectId>) serviceProvider.GetService(
                typeof(IEntityRepository<User<ObjectId>, ObjectId>));
            _roleRepository = (IEntityRepository<Role<ObjectId>, ObjectId>) serviceProvider.GetService(
                typeof(IEntityRepository<Role<ObjectId>, ObjectId>));
            _userRoleRepository = (IEntityRepository<UserRole<ObjectId>, ObjectId>) serviceProvider.GetService(
                typeof(IEntityRepository<UserRole<ObjectId>, ObjectId>));

            Task.WaitAll(InitializeRolesAsync(), InitializeAdminAsync(configuration));
        }

        private static async Task InitializeRolesAsync()
        {
            var roleNames = new List<string>
            {
                Role.Admin.ToString(),
                Role.User.ToString(),
                Role.Guest.ToString()
            };

            foreach (var roleName in roleNames)
            {
                var role = await ((RoleRepository) _roleRepository).GetRoleByNameAsync(roleName);
                if (role == null)
                    await _roleRepository.CreateEntityAsync(new Role<ObjectId>
                    {
                        Name = roleName
                    });
            }
        }

        private static async Task InitializeAdminAsync(IConfiguration configuration)
        {
            var adminEmail = configuration.GetSection("AdminCredential:Email").Value;
            var admin = await ((UserRepository) _userRepository).GetUserByEmailAsync(adminEmail);
            if (admin == null)
            {
                var passModel =
                    SecurityManager.EncryptPassword(configuration.GetSection("AdminCredential:Password").Value);
                var adminUser = await _userRepository.CreateEntityAsync(new User<ObjectId>
                {
                    UserName = configuration.GetSection("AdminCredential:UserName").Value,
                    Email = adminEmail,
                    PassportHash = passModel.PassportHash,
                    PassportSalt = passModel.PassportSalt,
                    IsLocked = false
                });

                if (adminUser?.Id != null && adminUser.Id != ObjectId.Empty)
                {
                    var adminRole = await ((RoleRepository) _roleRepository).GetRoleByNameAsync(Role.Admin.ToString());
                    if (adminRole != null)
                        await _userRoleRepository.CreateEntityAsync(new UserRole<ObjectId>
                        {
                            RoleId = adminRole.Id,
                            UserId = adminUser.Id
                        });
                }
            }
        }
    }
}