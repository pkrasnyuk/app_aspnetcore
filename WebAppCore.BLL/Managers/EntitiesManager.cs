using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MongoDB.Bson;
using WebAppCore.BLL.Exceptions;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Interfaces;
using WebAppCore.DAL.Interfaces;
using WebAppCore.DAL.PagingHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Managers
{
    public class EntitiesManager<TDomainModel, TModel, TViewModel> : IEntitiesManager<TModel, TViewModel> where TDomainModel : Entity<ObjectId>
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository<TDomainModel, ObjectId> _repository;
        private readonly AbstractValidator<TModel> _validator;

        protected EntitiesManager(IEntityRepository<TDomainModel, ObjectId> repository, IMapper mapper, AbstractValidator<TModel> validator)
        {
            _mapper = mapper;
            _validator = validator;
            _repository = repository;
        }

        public virtual async Task<ICollection<TViewModel>> GetEntitiesAsync()
        {
            var entities = await _repository.GetEntitiesAsync();
            return _mapper.Map<ICollection<TViewModel>>(entities);
        }

        public virtual async Task<PagedResults<TViewModel>> GetPagedEntitiesAsync(PageParameters parameters)
        {
            var entities = await _repository.GetPagedEntitiesAsync(parameters.PageNumber, parameters.PageSize, parameters.OrderBy, parameters.Ascending);
            return new PagedResults<TViewModel>
            {
                PageNumber = entities.PageNumber,
                PageSize = entities.PageSize,
                TotalNumberOfPages = entities.TotalNumberOfPages,
                TotalNumberOfEntities = entities.TotalNumberOfEntities,
                Entities = _mapper.Map<ICollection<TViewModel>>(entities.Entities)
            };
        }

        public virtual async Task<TViewModel> GetEntityAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId entityId) && !entityId.Equals(ObjectId.Empty))
            {
                var entity = await _repository.GetEntityByIdAsync(entityId);
                if (entity != null)
                {
                    return _mapper.Map<TViewModel>(entity);
                }

                throw new ResourceSearchException($"An entity with id={id} not found");
            }

            throw new BadRequestException($"An incorrect id={id} value");
        }
        
        public virtual async Task<TViewModel> CreateEntityAsync(TModel model)
        {
            var validationResult = await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ConflictRequestException(validationResult.Errors.ToAggregateResult());
            }

            var album = _mapper.Map<TDomainModel>(model);
            var result = await _repository.CreateEntityAsync(album);
            if (result != null)
            {
                return _mapper.Map<TViewModel>(result);
            }

            throw new BadResultException("An entity has not been created");
        }

        public virtual async Task<TViewModel> UpdateEntityAsync(string id, TModel model)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId entityId) && !entityId.Equals(ObjectId.Empty))
            {
                var validationResult = await _validator.ValidateAsync(model);
                if (!validationResult.IsValid)
                {
                    throw new ConflictRequestException(validationResult.Errors.ToAggregateResult());
                }

                var album = _mapper.Map<TDomainModel>(model);
                album.Id = entityId;
                var updateResult = await _repository.UpdateEntityAsync(album);
                if (updateResult)
                {
                    return _mapper.Map<TViewModel>(album);
                }

                throw new BadResultException("The entity has not been updated");
            }

            throw new BadRequestException("Incorrect request for update entity");
        }

        public virtual async Task<string> RemoveEntityAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId entityId) && !entityId.Equals(ObjectId.Empty))
            {
                var result = await _repository.RemoveEntityAsync(entityId);
                if (result)
                {
                    return "The entity has been successfully deleted";
                }

                throw new ResourceSearchException($"An entity with id={id} not found");
            }

            throw new BadRequestException($"An incorrect id={id} value");
        }
    }
}