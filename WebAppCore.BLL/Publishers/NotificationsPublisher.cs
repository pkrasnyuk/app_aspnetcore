using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Publishers
{
    [AllowAnonymous]
    public class NotificationsPublisher : Hub
    {
        public async Task PublishNotification(NotificationModel model)
        {
            await Clients.All.InvokeAsync("OnSendingNotification", model);
        }
    }
}