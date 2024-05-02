using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.ContactUs
{
    public class ContactUsPhoneNumbersCreaetDTO
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        public string PhoneIcon { get; set; }

        [StringLength(25)]
        public string Title { get; set; }

    }
}
