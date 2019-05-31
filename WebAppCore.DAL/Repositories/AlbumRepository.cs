using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using WebAppCore.DAL.CacheHelpers;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class AlbumRepository : EntityCacheRepository<Album<ObjectId>, ObjectId>
    {
        public AlbumRepository(IOptions<DataAccessConfiguration> dbOptions, IOptions<List<CacheSettings>> cacheOptions,
            IMemoryCache memoryCache) : base(dbOptions, "Albums", cacheOptions, "AlbumsCache", memoryCache)
        {
        }
    }
}