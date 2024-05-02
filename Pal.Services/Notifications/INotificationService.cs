using Pal.Data.DTOs.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.Notifications
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(NotificationDTO model);

        Task<bool> SetNotificationSeen(long id);

        Task<IEnumerable<NotificationMsg>> GetNotificationsAsync(int count);
        Task<IEnumerable<NotificationMsg>> GetPAginatedNotificationsAsync(int pageSize, int pageNumber);
        Task<bool> SendNotificationIfNotExistAsync(NotificationDTO model);
        Task SendMobileNotification(string Title, string Body, string Image);
    }
}