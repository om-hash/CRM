using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Notifications
{
    public class NotificationParam : BaseEntity<long>
    {
        public long NotificationId { get; set; }

        public string Param { get; set; }


        [ForeignKey(nameof(NotificationId))]
        public virtual Notification Notification { get; set; }
    }
}
