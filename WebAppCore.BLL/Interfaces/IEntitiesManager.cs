using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.BLL.Helpers;
using WebAppCore.DAL.PagingHelpers;

namespace WebAppCore.BLL.Interfaces
{
    public interface IEntitiesManager<in TModel, TViewModel> : IManager
    {
        Task<ICollection<TViewModel>> GetEntitiesAsync();

        Task<PagedResults<TViewModel>> GetPagedEntitiesAsync(PageParameters parameters);

        Task<TViewModel> GetEntityAsync(string id);

        Task<TViewModel> CreateEntityAsync(TModel model);

        Task<TViewModel> UpdateEntityAsync(string id, TModel model);

        Task<string> RemoveEntityAsync(string id);
    }
}