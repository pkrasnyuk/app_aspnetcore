using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using WebAppCore.DAL.CacheHelpers;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class UserRepository : EntityCacheRepository<User<ObjectId>, ObjectId>
    {
        public UserRepository(IOptions<DataAccessConfiguration> dbOptions, IOptions<List<CacheSettings>> cacheOptions,
            IMemoryCache memoryCache) : base(dbOptions, "Users", cacheOptions, "UsersCache", memoryCache)
        {
        }

        public async Task<User<ObjectId>> GetUserByEmailAsync(string email)
        {
            return await GetEntityAsync(x => x.Email.Equals(email));
        }

        public override async Task<bool> UpdateEntityAsync(User<ObjectId> entity)
        {
            var dbEntity = await GetEntityByIdAsync(entity.Id);
            if (dbEntity != null)
            {
                dbEntity.UserName = entity.UserName;
                dbEntity.Email = entity.Email;

                return await base.UpdateEntityAsync(dbEntity);
            }

            return false;
        }
    }
}