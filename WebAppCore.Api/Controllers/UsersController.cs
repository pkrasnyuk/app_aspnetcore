using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.BLL.Attributes;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Interfaces;
using WebAppCore.BLL.Models;

namespace WebAppCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUsersManager _usersManager;

        public UsersController(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _usersManager.GetUsersAsync());
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] string id)
        {
            return Ok(await _usersManager.GetUserAsync(id));
        }

        [AuthorizePolicy(Policy.AdminOnly)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] string id, [FromBody] UpdateUserModel model)
        {
            return Ok(await _usersManager.UpdateUserAsync(id, model));
        }

        [AuthorizePolicy(Policy.AdminOnly)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUserAsync([FromRoute] string id)
        {
            return Ok(await _usersManager.RemoveUserAsync(id));
        }
    }
}