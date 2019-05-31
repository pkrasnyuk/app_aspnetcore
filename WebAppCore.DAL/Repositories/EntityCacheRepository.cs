using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebAppCore.DAL.CacheHelpers;
using WebAppCore.DAL.Common;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.DAL.PagingHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class EntityCacheRepository<TEntity, TKey> : EntityRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        private readonly object _cacheLock = new object();
        private readonly CacheParameters _entityCacheParameters;
        private readonly IMemoryCache _memoryCache;

        protected EntityCacheRepository(IOptions<DataAccessConfiguration> dbOptions, string collectionName,
            IOptions<List<CacheSettings>> cacheOptions, string cacheSettingsName, IMemoryCache memoryCache) : base(dbOptions, collectionName)
        {
            _memoryCache = memoryCache;

            if (cacheOptions?.Value != null && cacheOptions.Value.Any())
            {
                var entityCacheSettings = cacheOptions.Value.FirstOrDefault(x => x.Name.Equals(cacheSettingsName));
                if (entityCacheSettings != null)
                {
                    _entityCacheParameters = entityCacheSettings.Parameters;
                }
            }
        }

        public override async Task<TEntity> GetEntityByIdAsync(TKey id)
        {
            if (_entityCacheParameters != null && _memoryCache != null)
            {
                var entities = await GetEntitiesAsync();
                return entities?.SingleOrDefault(x => x.Id.Equals(id));
            }

            return await base.GetEntityByIdAsync(id);
        }

        public override async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            if (filterExpression != null)
            {
                if (_entityCacheParameters != null && _memoryCache != null)
                {
                    var entities = await GetEntitiesAsync(filterExpression);
                    return entities?.SingleOrDefault();
                }

                return await base.GetEntityAsync(filterExpression);
            }

            return null;
        }

        public override async Task<ICollection<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            if (_entityCacheParameters != null && _memoryCache != null)
            {
                ICollection<TEntity> entities;
                lock (_cacheLock)
                {
                    if (_memoryCache.TryGetValue(_entityCacheParameters.Key, out entities))
                    {
                        return filterExpression == null
                            ? entities
                            : entities?.AsQueryable().Where(filterExpression).ToList();
                    }
                }

                entities = await base.GetEntitiesAsync(null);

                lock (_cacheLock)
                {
                    _memoryCache.Set(_entityCacheParameters.Key, entities,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_entityCacheParameters.Expiration)));
                    return filterExpression == null
                        ? entities
                        : entities?.AsQueryable().Where(filterExpression).ToList();
                }
            }

            return await base.GetEntitiesAsync(filterExpression);
        }

        public override async Task<PagedResults<TEntity>> GetPagedEntitiesAsync(int? pageNumber = 1, int? pageSize = 10,
            string orderBy = "", bool ascending = false, Expression<Func<TEntity, bool>> filterExpression = null)
        {
            if (_entityCacheParameters != null && _memoryCache != null)
            {
                var pageNumberValue = pageNumber.HasValue && pageNumber.Value > 0 ? pageNumber.Value : 1;

                var entities = await GetEntitiesAsync(filterExpression);
                if (entities != null)
                {
                    var query = entities.AsQueryable();
                    var totalNumberOfEntities = query.Count();

                    var orderedQueryable = !string.IsNullOrWhiteSpace(orderBy)
                        ? (Func<TEntity, object>) (x => x.GetType().GetProperty(orderBy.FirstCharToUpper()).GetValue(x, null))
                        : (x => x.UpdatedAt);

                    query = ascending
                        ? query.OrderBy(orderedQueryable).AsQueryable()
                        : query.OrderByDescending(orderedQueryable).AsQueryable();

                    long totalPageCount = 0;

                    if (pageNumber.HasValue && pageNumber.Value > 0 && pageSize.HasValue && pageSize.Value > 0)
                    {
                        var skipAmount = pageSize.Value * (pageNumberValue - 1);
                        var mod = totalNumberOfEntities % pageSize.Value;
                        totalPageCount = totalNumberOfEntities / pageSize.Value + (mod == 0 ? 0 : 1);

                        query = query.Skip(skipAmount).Take(pageSize.Value);
                    }

                    return new PagedResults<TEntity>
                    {
                        Entities = query.ToList(),
                        PageNumber = pageNumberValue,
                        PageSize = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 1,
                        TotalNumberOfPages = totalPageCount > 0 ? totalPageCount : 1,
                        TotalNumberOfEntities = totalNumberOfEntities
                    };
                }

                return new PagedResults<TEntity>
                {
                    Entities = null,
                    PageNumber = pageNumberValue,
                    PageSize = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 1,
                    TotalNumberOfPages = 1,
                    TotalNumberOfEntities = 0
                };
            }

            return await base.GetPagedEntitiesAsync(pageNumber, pageSize, orderBy, ascending, filterExpression);
        }

        public override async Task<bool> IsEntityExistAsync(TEntity enity)
        {
            if (_entityCacheParameters != null && _memoryCache != null)
            {
                var entities = await GetEntitiesAsync();
                return entities?.SingleOrDefault(x => x.Id.Equals(enity.Id)) != null;
            }

            return await base.IsEntityExistAsync(enity);
        }

        public override async Task<TEntity> CreateEntityAsync(TEntity entity)
        {
            if (_entityCacheParameters != null && _memoryCache != null)
            {
                lock (_cacheLock)
                {
                    _memoryCache.Remove(_entityCacheParameters.Key);
                }
            }

            return await base.CreateEntityAsync(entity);
        }

        public override async Task<bool> UpdateEntityAsync(TEntity entity)
        {
            if (_entityCacheParameters != null && _memoryCache != null)
            {
                lock (_cacheLock)
                {
                    _memoryCache.Remove(_entityCacheParameters.Key);
                }
            }

            return await base.UpdateEntityAsync(entity);
        }

        public override async Task<bool> RemoveEntityAsync(TKey id)
        {
            if (_entityCacheParameters != null && _memoryCache != null)
            {
                lock (_cacheLock)
                {
                    _memoryCache.Remove(_entityCacheParameters.Key);
                }
            }

            return await base.RemoveEntityAsync(id);
        }
    }
}