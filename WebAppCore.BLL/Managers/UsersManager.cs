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
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Managers
{
    public class UsersManager : IUsersManager
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository<User<ObjectId>, ObjectId> _repository;

        public UsersManager(IEntityRepository<User<ObjectId>, ObjectId> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<UserViewModel>> GetUsersAsync()
        {
            var users = await _repository.GetEntitiesAsync();
            return _mapper.Map<ICollection<UserViewModel>>(users);
        }

        public async Task<UserViewModel> GetUserAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId userId) && !userId.Equals(ObjectId.Empty))
            {
                var user = await _repository.GetEntityByIdAsync(userId);
                if (user != null)
                {
                    return _mapper.Map<UserViewModel>(user);
                }

                throw new ResourceSearchException($"An user with id={id} not found");
            }

            throw new BadRequestException($"An incorrect id={id} value");
        }

        public async Task<UserViewModel> UpdateUserAsync(string id, UpdateUserModel model)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId userId) && !userId.Equals(ObjectId.Empty))
            {
                var updateUserModels = _mapper.Map<ICollection<UpdateUserModel>>(await _repository.GetEntitiesAsync());
                var validationResult = await new UpdateUserModelValidation(updateUserModels).ValidateAsync(model);
                if (!validationResult.IsValid)
                {
                    throw new ConflictRequestException(validationResult.Errors.ToAggregateResult());
                }

                var user = _mapper.Map<User<ObjectId>>(model);
                user.Id = userId;
                var updateResult = await _repository.UpdateEntityAsync(user);
                if (updateResult)
                {
                    return _mapper.Map<UserViewModel>(user);
                }

                throw new BadResultException("The user has not been updated");
            }

            throw new BadRequestException("Incorrect request for update user");
        }

        public async Task<string> RemoveUserAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId userId) && !userId.Equals(ObjectId.Empty))
            {
                var result = await _repository.RemoveEntityAsync(userId);
                if (result)
                {
                    return "The user has been successfully deleted";
                }

                throw new ResourceSearchException($"An user with id={id} not found");
            }

            throw new BadRequestException($"An incorrect id={id} value");
        }
    }
}