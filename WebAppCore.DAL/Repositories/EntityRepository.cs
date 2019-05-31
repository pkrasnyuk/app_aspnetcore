using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAppCore.DAL.Common;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.DAL.Interfaces;
using WebAppCore.DAL.PagingHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class EntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        protected EntityRepository(IOptions<DataAccessConfiguration> dbOptions, string collectionName)
        {
            Context = new DataAccess(dbOptions).DbContext;
            Collection = Context.GetCollection<TEntity>(collectionName);
        }

        protected IMongoDatabase Context { get; }

        protected IMongoCollection<TEntity> Collection { get; }

        public virtual async Task<TEntity> GetEntityByIdAsync(TKey id)
        {
            return await Collection.Find(x => x.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            return filterExpression != null ? await Collection.Find(filterExpression).SingleOrDefaultAsync() : null;
        }

        public virtual async Task<ICollection<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            return filterExpression != null
                ? await Collection.Find(filterExpression).ToListAsync()
                : await Collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<PagedResults<TEntity>> GetPagedEntitiesAsync(int? pageNumber = 1, int? pageSize = 10,
            string orderBy = "", bool ascending = false, Expression<Func<TEntity, bool>> filterExpression = null)
        {
            var query = filterExpression != null ? Collection.Find(filterExpression) : Collection.Find(_ => true);

            var totalNumberOfEntities = await query.CountAsync();

            var sortOrder = !string.IsNullOrWhiteSpace(orderBy) ? orderBy.FirstCharToUpper() : "UpdatedAt";
            query = query.Sort(ascending
                ? Builders<TEntity>.Sort.Ascending(sortOrder)
                : Builders<TEntity>.Sort.Descending(sortOrder));

            var pageNumberValue = pageNumber.HasValue && pageNumber.Value > 0 ? pageNumber.Value : 1;
            long totalPageCount = 0;

            if (pageNumber.HasValue && pageNumber.Value > 0 && pageSize.HasValue && pageSize.Value > 0)
            {
                var skipAmount = pageSize.Value * (pageNumberValue - 1);
                var mod = totalNumberOfEntities % pageSize.Value;
                totalPageCount = totalNumberOfEntities / pageSize.Value + (mod == 0 ? 0 : 1);

                query = query.Skip(skipAmount).Limit(pageSize.Value);
            }

            return new PagedResults<TEntity>
            {
                Entities = await query.ToListAsync(),
                PageNumber = pageNumberValue,
                PageSize = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 1,
                TotalNumberOfPages = totalPageCount > 0 ? totalPageCount : 1,
                TotalNumberOfEntities = totalNumberOfEntities
            };
        }

        public virtual async Task<bool> IsEntityExistAsync(TEntity enity)
        {
            return await Collection.Find(x => x.Id.Equals(enity.Id)).SingleOrDefaultAsync() != null;
        }

        public virtual async Task<TEntity> CreateEntityAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = entity.CreatedAt;
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual async Task<bool> UpdateEntityAsync(TEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            var result = await Collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity);
            return result.IsAcknowledged;
        }

        public virtual async Task<bool> RemoveEntityAsync(TKey id)
        {
            var result = await Collection.DeleteOneAsync(x => x.Id.Equals(id));
            return result.IsAcknowledged;
        }
    }
}