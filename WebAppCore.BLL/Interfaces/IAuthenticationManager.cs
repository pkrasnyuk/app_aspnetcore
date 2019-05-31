using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Interfaces
{
    public interface IAuthenticationManager : IManager
    {
        Task<UserModel> LoginUserAsync(LoginUserModel model, HttpContext httpContext);

        Task LogoutUserAsync(HttpContext httpContext);
    }
}