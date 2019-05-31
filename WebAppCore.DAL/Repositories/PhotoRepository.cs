using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using WebAppCore.DAL.CacheHelpers;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class PhotoRepository : EntityCacheRepository<Photo<ObjectId>, ObjectId>
    {
        private readonly IGridFSBucket _gridFs;

        public PhotoRepository(IOptions<DataAccessConfiguration> dbOptions, IOptions<List<CacheSettings>> cacheOptions,
            IMemoryCache memoryCache) : base(dbOptions, "Photos", cacheOptions, "PhotosCache", memoryCache)
        {
            _gridFs = new GridFSBucket(Context);
        }

        public override async Task<bool> RemoveEntityAsync(ObjectId id)
        {
            var entity = await GetEntityByIdAsync(id);
            if (entity != null && entity.FileId != ObjectId.Empty)
            {
                await _gridFs.DeleteAsync(entity.FileId);
            }
            return await base.RemoveEntityAsync(id);
        }

        public async Task<bool> UploadPhotoFile(ObjectId photoId, string fileName, byte[] source)
        {
            var entity = await GetEntityByIdAsync(photoId);
            if (entity != null && !string.IsNullOrWhiteSpace(fileName) && source != null)
            {
                if (entity.FileId != ObjectId.Empty)
                {
                    await _gridFs.DeleteAsync(entity.FileId);
                }
                entity.FileName = fileName;
                entity.FileId = await _gridFs.UploadFromBytesAsync(fileName, source);

                return await UpdateEntityAsync(entity);
            }

            return false;
        }

        public async Task<byte[]> GetPhotoBytesAsync(ObjectId fileId)
        {
            return fileId != ObjectId.Empty ? await _gridFs.DownloadAsBytesAsync(fileId) : null;
        }
    }
}