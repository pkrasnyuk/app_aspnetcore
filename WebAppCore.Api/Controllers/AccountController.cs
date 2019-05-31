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
    public class AccountController : BaseController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IRegisterManager _registerManager;

        public AccountController(IAuthenticationManager authenticationManager, IRegisterManager registerManager)
        {
            _authenticationManager = authenticationManager;
            _registerManager = registerManager;
        }

        /// <summary>
        /// User Login method
        /// </summary>
        /// <param name="model">User login model</param>
        /// <returns>User Model</returns>
        [AllowAnonymous]
        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserModel model)
        {
            return Ok(await _authenticationManager.LoginUserAsync(model, HttpContext));
        }

        /// <summary>
        /// Logout User method
        /// </summary>
        /// <returns>Logout User Status</returns>
        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpPost("logoutUser")]
        public async Task<IActionResult> LogoutUserAsync()
        {
            await _authenticationManager.LogoutUserAsync(HttpContext);
            return StatusCode(200);
        }

        /// <summary>
        /// Register User method
        /// </summary>
        /// <param name="model">Register User model</param>
        /// <returns>Registration Status</returns>
        [AllowAnonymous]
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserModel model)
        {
            return Ok(await _registerManager.RegisterUserAsync(model));
        }
    }
}