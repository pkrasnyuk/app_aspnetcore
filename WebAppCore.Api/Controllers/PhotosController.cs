using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.BLL.Attributes;
using WebAppCore.BLL.Extensions;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Models;

namespace WebAppCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PhotosController : EntitiesController<PhotoModel, PhotoViewModel>
    {
        private readonly IPhotosManager _photosManager;

        public PhotosController(IPhotosManager photosManager) : base(photosManager)
        {
            _photosManager = photosManager;
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpGet("byAlbumId/{albumId}")]
        public async Task<IActionResult> GetPhotosByAlbumIdAsync([FromRoute] string albumId)
        {
            return Ok(await _photosManager.GetPhotosByAlbumIdAsync(albumId));
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpPost("byAlbumId/{albumId}/withPagination")]
        public async Task<IActionResult> GetPhotosByAlbumIdWithPaginationAsync([FromRoute] string albumId, [FromBody] PageParameters parameters)
        {
            return Ok(await _photosManager.GetPhotosByAlbumIdAsync(albumId, parameters));
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpPost("{id}/fileUpload")]
        public async Task<IActionResult> UploadPhotoFileAsync([FromRoute] string id, [FromForm] IFormFile filePayload)
        {
            var fileModel = new FileModel().UploadFile(filePayload);
            return Ok(await _photosManager.UploadPhotoFileAsync(id, fileModel));
        }
    }
}