using System.ComponentModel.DataAnnotations;

namespace Pal.Core.Domains.Sms
{
    public class SmsMsg : BaseEntity<int>
    {
        [Required, StringLength(50)]
        public string PhoneNumber { get; set; }


        [Required, StringLength(500)]
        public string Msg { get; set; }

    }
}
