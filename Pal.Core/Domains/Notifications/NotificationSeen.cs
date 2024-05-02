namespace Pal.Core.Domains.Notifications
{
    public class NotificationSeen : BaseEntity<long>
    {
        public long NotificationId { get; set; }
        public string UserId { get; set; }
    }
}
