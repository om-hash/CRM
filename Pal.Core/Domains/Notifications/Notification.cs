using Pal.Core.Enums.Account;
using Pal.Core.Enums.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Notifications
{
    public class Notification : BaseEntity<long>
    {
        public int NotificationTypeId { get; set; }
        public DateTime NotifyDate { get; set; } = DateTime.Now;
        public UserType NotificationFor { get; set; }
        public string Url { get; set; }
        public string GroupId { get; set; }

        public string SeenUserIdsList { get; set; }

        public virtual ICollection<NotificationParam> NotificationParams { get; set; }


        [ForeignKey(nameof(NotificationTypeId))]
        public virtual NotificationType NotificationType { get; set; }
    }
}
