using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using WebAppCore.BLL.Exceptions;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Models;
using WebAppCore.BLL.Validations;
using WebAppCore.DAL.Interfaces;
using WebAppCore.DAL.Repositories;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Managers
{
    public class RegisterManager : IRegisterManager
    {
        private static IEntityRepository<User<ObjectId>, ObjectId> _userRepository;
        private static IEntityRepository<Role<ObjectId>, ObjectId> _roleRepository;
        private static IEntityRepository<UserRole<ObjectId>, ObjectId> _userRoleRepository;
        private readonly IMapper _mapper;

        public RegisterManager(IEntityRepository<User<ObjectId>, ObjectId> userRepository,
            IEntityRepository<Role<ObjectId>, ObjectId> roleRepository,
            IEntityRepository<UserRole<ObjectId>, ObjectId> userRoleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<UserViewModel> RegisterUserAsync(RegisterUserModel model)
        {
            var registerUserModels = _mapper.Map<ICollection<RegisterUserModel>>(await _userRepository.GetEntitiesAsync());
            var validationResult = await new RegisterUserModelValidation(registerUserModels).ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ConflictRequestException(validationResult.Errors.ToAggregateResult());
            }

            var user = _mapper.Map<User<ObjectId>>(model);

            var passwordModel = SecurityManager.EncryptPassword(model.Password);
            user.PassportHash = passwordModel.PassportHash;
            user.PassportSalt = passwordModel.PassportSalt;

            user = await _userRepository.CreateEntityAsync(user);
            if (user?.Id != null && user.Id != ObjectId.Empty)
            {
                var guestRole = await ((RoleRepository) _roleRepository).GetRoleByNameAsync(Role.Guest.ToString());
                if (guestRole != null)
                {
                    await _userRoleRepository.CreateEntityAsync(new UserRole<ObjectId>
                    {
                        RoleId = guestRole.Id,
                        UserId = user.Id
                    });
                }
            }

            return _mapper.Map<UserViewModel>(user);
        }
    }
}