using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Notifications
{
    public class NotificationType : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<NotificationTypeTranslate> NotificationTypeTranslates { get; set; }
    }


    public class NotificationTypeTranslate : BaseEntityTranslate<int>
    {
        public int NotificationTypeId { get; set; }
        /// <summary>
        /// As Format => Like  Company {0} has added a new Project {1}.
        /// </summary>
        public string TypeName { get; set; }

        [ForeignKey(nameof(NotificationTypeId))]
        public virtual NotificationType NotificationType { get; set; }
    }
}
