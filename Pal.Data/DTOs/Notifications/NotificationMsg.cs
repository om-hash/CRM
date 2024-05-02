using System;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Notifications
{
    public class NotificationMsg
    {
        public long Id { get; set; }

        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public int LanguageId { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public string Url { get; set; }
        public DateTime Date { get; set; }

        public bool IsSeen { get; set; }

    }
}
