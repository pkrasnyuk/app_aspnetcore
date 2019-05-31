using System.Threading.Tasks;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Interfaces
{
    public interface IRegisterManager : IManager
    {
        Task<UserViewModel> RegisterUserAsync(RegisterUserModel model);
    }
}