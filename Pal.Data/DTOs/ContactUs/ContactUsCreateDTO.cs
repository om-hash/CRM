using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.ContactUs
{
    public class ContactUsCreateDTO
    {
        public int Id { get; set; }
        [StringLength(50), EmailAddress]
        public string Email { get; set; }

        [StringLength(50), EmailAddress]
        public string SupportEmail { get; set; }

        [StringLength(500)]
        public string Address { get; set; }


        [StringLength(500)]
        public string Img { get; set; }
        public IFormFile ImgFile { get; set; }

        [StringLength(30)]
        public string LocationLatLng { get; set; }

        [StringLength(150)]
        public string LocationLink { get; set; }

        public List<ContactUsTranslateCreateDTO> ContactUsTranslates { get; set; }
        public List<ContactUsSocialMediaDTO> ContactUsSocialMedia { get; set; }
        public List<ContactUsPhoneNumbersCreaetDTO> CreaetNewContactUsPhoneNumbers { get; set; }


    }
}
