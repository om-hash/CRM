using Pal.Core.Enums.Account;
using Pal.Core.Enums.Notifications;
using System;
using System.Collections.Generic;

namespace Pal.Data.DTOs.Notifications
{
    public class NotificationDTO
    {
        public long Id { get; set; }

        public int NotificationTypeId { get; set; }
        public UserType NotificationFor { get; set; }
        public string Url { get; set; }
        public string GroupId { get; set; }

        public List<NotificationParamDTO> NotificationParams { get; set; }
    }

  
}
