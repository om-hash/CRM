using System.ComponentModel.DataAnnotations;

namespace Pal.Web.Models.Customer
{
    public class CustomerProfileViewModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public int NationalityId { get; set; }

        public int CountryId { get; set; }


        [StringLength(50),Required]
        public string FullName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        public string WhatsappNumber { get; set; }

        [StringLength(500)]
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public string MainImage { get; set; }
    }
}
