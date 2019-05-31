using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Interfaces
{
    public interface IUsersManager : IManager
    {
        Task<ICollection<UserViewModel>> GetUsersAsync();

        Task<UserViewModel> GetUserAsync(string id);

        Task<UserViewModel> UpdateUserAsync(string id, UpdateUserModel model);

        Task<string> RemoveUserAsync(string id);
    }
}