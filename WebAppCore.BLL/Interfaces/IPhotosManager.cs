using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Models;
using WebAppCore.DAL.PagingHelpers;

namespace WebAppCore.BLL.Interfaces
{
    public interface IPhotosManager : IEntitiesManager<PhotoModel, PhotoViewModel>
    {
        Task<ICollection<PhotoViewModel>> GetPhotosByAlbumIdAsync(string albumId);

        Task<PagedResults<PhotoViewModel>> GetPhotosByAlbumIdAsync(string albumId, PageParameters parameters);

        Task<PhotoViewModel> UploadPhotoFileAsync(string id, FileModel fileModel);
    }
}