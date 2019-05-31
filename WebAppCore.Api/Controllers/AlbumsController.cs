using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Models;

namespace WebAppCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AlbumsController : EntitiesController<AlbumModel, AlbumViewModel>
    {
        public AlbumsController(IAlbumsManager albumsManager) : base(albumsManager)
        {
        }
    }
}