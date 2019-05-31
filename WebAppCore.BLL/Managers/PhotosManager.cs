using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using WebAppCore.BLL.Exceptions;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Models;
using WebAppCore.BLL.Validations;
using WebAppCore.DAL.Interfaces;
using WebAppCore.DAL.PagingHelpers;
using WebAppCore.DAL.Repositories;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Managers
{
    public class PhotosManager : EntitiesManager<Photo<ObjectId>, PhotoModel, PhotoViewModel>, IPhotosManager
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository<Photo<ObjectId>, ObjectId> _repository;

        public PhotosManager(IEntityRepository<Photo<ObjectId>, ObjectId> repository, IMapper mapper) : base(repository, mapper, new PhotoModelValidation())
        {
            _mapper = mapper;
            _repository = repository;
        }

        public override async Task<PhotoViewModel> GetEntityAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId photoId) && !photoId.Equals(ObjectId.Empty))
            {
                var photo = await _repository.GetEntityByIdAsync(photoId);
                if (photo != null)
                {
                    var result = _mapper.Map<PhotoViewModel>(photo);
                    if (photo.FileId != ObjectId.Empty)
                    {
                        result.FileBytes = await ((PhotoRepository) _repository).GetPhotoBytesAsync(photo.FileId);
                    }
                    return result;
                }

                throw new ResourceSearchException($"An entity with id={id} not found");
            }

            throw new BadRequestException($"An incorrect id={id} value");
        }

        public override async Task<ICollection<PhotoViewModel>> GetEntitiesAsync()
        {
            var photos = await _repository.GetEntitiesAsync();
            if (photos != null && photos.Any())
            {
                var result = new List<PhotoViewModel>();

                foreach (var photo in photos)
                {
                    var photoViewModel = _mapper.Map<PhotoViewModel>(photo);
                    if (photo.FileId != ObjectId.Empty)
                    {
                        photoViewModel.FileBytes = await ((PhotoRepository) _repository).GetPhotoBytesAsync(photo.FileId);
                    }
                    result.Add(photoViewModel);
                }

                return result;
            }

            return _mapper.Map<ICollection<PhotoViewModel>>(photos);
        }

        public override async Task<PagedResults<PhotoViewModel>> GetPagedEntitiesAsync(PageParameters parameters)
        {
            var photos = await _repository.GetPagedEntitiesAsync(parameters.PageNumber, parameters.PageSize, parameters.OrderBy, parameters.Ascending);

            var collection = new List<PhotoViewModel>();
            if (photos.Entities != null && photos.Entities.Any())
            {
                foreach (var photo in photos.Entities)
                {
                    var photoViewModel = _mapper.Map<PhotoViewModel>(photo);
                    if (photo.FileId != ObjectId.Empty)
                    {
                        photoViewModel.FileBytes = await ((PhotoRepository) _repository).GetPhotoBytesAsync(photo.FileId);
                    }
                    collection.Add(photoViewModel);
                }
            }

            return new PagedResults<PhotoViewModel>
            {
                PageNumber = photos.PageNumber,
                PageSize = photos.PageSize,
                TotalNumberOfPages = photos.TotalNumberOfPages,
                TotalNumberOfEntities = photos.TotalNumberOfEntities,
                Entities = collection
            };
        }

        public async Task<ICollection<PhotoViewModel>> GetPhotosByAlbumIdAsync(string albumId)
        {
            if (!string.IsNullOrWhiteSpace(albumId) && ObjectId.TryParse(albumId, out ObjectId id) && !id.Equals(ObjectId.Empty))
            {
                var photos = await _repository.GetEntitiesAsync(x => x.AlbumId.Equals(id));
                if (photos != null && photos.Any())
                {
                    var result = new List<PhotoViewModel>();

                    foreach (var photo in photos)
                    {
                        var photoViewModel = _mapper.Map<PhotoViewModel>(photo);
                        if (photo.FileId != ObjectId.Empty)
                        {
                            photoViewModel.FileBytes = await ((PhotoRepository) _repository).GetPhotoBytesAsync(photo.FileId);
                        }
                        result.Add(photoViewModel);
                    }

                    return result;
                }

                return _mapper.Map<ICollection<PhotoViewModel>>(photos);
            }

            throw new BadRequestException($"An incorrect album id={albumId} value");
        }

        public async Task<PagedResults<PhotoViewModel>> GetPhotosByAlbumIdAsync(string albumId, PageParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(albumId) && ObjectId.TryParse(albumId, out ObjectId id) && !id.Equals(ObjectId.Empty))
            {
                var photos = await _repository.GetPagedEntitiesAsync(parameters.PageNumber, parameters.PageSize, parameters.OrderBy, parameters.Ascending, x => x.AlbumId == id);

                var collection = new List<PhotoViewModel>();
                if (photos.Entities != null && photos.Entities.Any())
                {
                    foreach (var photo in photos.Entities)
                    {
                        var photoViewModel = _mapper.Map<PhotoViewModel>(photo);
                        if (photo.FileId != ObjectId.Empty)
                        {
                            photoViewModel.FileBytes = await ((PhotoRepository)_repository).GetPhotoBytesAsync(photo.FileId);
                        }
                        collection.Add(photoViewModel);
                    }
                }

                return new PagedResults<PhotoViewModel>
                {
                    PageNumber = photos.PageNumber,
                    PageSize = photos.PageSize,
                    TotalNumberOfPages = photos.TotalNumberOfPages,
                    TotalNumberOfEntities = photos.TotalNumberOfEntities,
                    Entities = collection
                };
            }

            throw new BadRequestException($"An incorrect album id={albumId} value");
        }

        public async Task<PhotoViewModel> UploadPhotoFileAsync(string id, FileModel fileModel)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId photoId) && !photoId.Equals(ObjectId.Empty))
            {
                var fileValidationResult = await new FileModelValidation().ValidateAsync(fileModel);
                if (!fileValidationResult.IsValid)
                {
                    throw new ConflictRequestException(fileValidationResult.Errors.ToAggregateResult());
                }

                var uploadResult = await ((PhotoRepository) _repository).UploadPhotoFile(photoId, fileModel.FileName, fileModel.Source);
                if (uploadResult)
                {
                    return await GetEntityAsync(id);
                }

                throw new BadResultException("The photo has been not uploaded");
            }

            throw new BadRequestException("Incorrect request for upload photo file");
        }
    }
}