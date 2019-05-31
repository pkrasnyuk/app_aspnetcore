using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using WebAppCore.BLL.Exceptions;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Models;
using WebAppCore.BLL.Validations;
using WebAppCore.DAL.Interfaces;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Managers
{
    public class AlbumsManager : EntitiesManager<Album<ObjectId>, AlbumModel, AlbumViewModel>, IAlbumsManager
    {
        private readonly IEntityRepository<Photo<ObjectId>, ObjectId> _photoRepository;

        public AlbumsManager(IEntityRepository<Album<ObjectId>, ObjectId> albumRepository,
            IEntityRepository<Photo<ObjectId>, ObjectId> photoRepository, IMapper mapper)
            : base(albumRepository, mapper, new AlbumModelValidation())
        {
            _photoRepository = photoRepository;
        }

        public override async Task<string> RemoveEntityAsync(string id)
        {
            var result = true;

            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out ObjectId albumId) && !albumId.Equals(ObjectId.Empty))
            {
                var photos = await _photoRepository.GetEntitiesAsync(x => x.AlbumId == albumId);
                if (photos != null && photos.Any())
                {
                    foreach (var photo in photos)
                    {
                        result &= await _photoRepository.RemoveEntityAsync(photo.Id);
                    }
                }
            }

            if (result)
            {
                return await base.RemoveEntityAsync(id);
            }

            throw new BadResultException("The entity has not been successfully deleted");
        }
    }
}