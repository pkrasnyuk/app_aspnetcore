using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAppCore.DAL.PagingHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Interfaces
{
    public interface IEntityRepository<TEntity, in TKey> where TEntity : Entity<TKey>
    {
        Task<TEntity> GetEntityByIdAsync(TKey id);

        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> filterExpression = null);

        Task<ICollection<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> filterExpression = null);

        Task<PagedResults<TEntity>> GetPagedEntitiesAsync(int? pageNumber, int? pageSize, string orderBy, bool ascending, Expression<Func<TEntity, bool>> filterExpression = null);

        Task<bool> IsEntityExistAsync(TEntity enity);

        Task<TEntity> CreateEntityAsync(TEntity entity);

        Task<bool> UpdateEntityAsync(TEntity entity);

        Task<bool> RemoveEntityAsync(TKey id);
    }
}