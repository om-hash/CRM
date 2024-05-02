using Microsoft.AspNetCore.SignalR;
using Pal.Core.Enums.Account;
using Pal.Data.DTOs.Notifications;
using Pal.Services.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;
        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                Groups.AddToGroupAsync(Context.ConnectionId, Context.User.FindFirst(ClaimTypes.Name).Value);
                Groups.AddToGroupAsync(Context.ConnectionId, Context.User.FindFirst(PalClaimType.UserType.ToString()).Value);

                if (Context.User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Companies.ToString())
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, UserType.Companies.ToString());
                    Groups.AddToGroupAsync(Context.ConnectionId, UserType.Companies.ToString() + Context.User.FindFirst(PalClaimType.CompanyId.ToString()).Value);
                }
                return base.OnConnectedAsync();
            }
            catch (Exception )
            {


            }
            // Join Group

            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.FindFirst(ClaimTypes.Name).Value);

            // هل هادا اليوزر كومباني؟

            return base.OnConnectedAsync();

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task<IEnumerable<NotificationMsg>> GetAllNotification(int count)
        {
            var model = _notificationService.GetNotificationsAsync(count);
            return model;
        }
    }
}
