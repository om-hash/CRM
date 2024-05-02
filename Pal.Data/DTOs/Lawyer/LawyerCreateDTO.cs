using Microsoft.AspNetCore.Http;
using Pal.Data.DTOs.Advisors;
using Pal.Data.DTOs.RatingAndComment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Lawyer
{
    public class LawyerCreateDTO
    {
        public int Id { get; set; }
        public int CountryId { get; set; }

        public int CityId { get; set; }

        [StringLength(250)]
        public string FullAddress { get; set; }

        public string SpeakingLanguages { get; set; }

        [StringLength(25)]
        public string WhatsApp { get; set; }

        [Required, StringLength(25)]
        public string Phone { get; set; }

        [StringLength(100), EmailAddress]
        public string Email { get; set; }

        [StringLength(500)]
        public string MainImg { get; set; }

        public IFormFile MainImgFile { get; set; }

        [StringLength(500)]
        public string CertificateFile { get; set; }

        public IFormFile CertificateFileObject { get; set; }
        public float AvrageRating { get; set; } = 0;

        public int WorkStart { get; set; }
        public int WorkEnd { get; set; }

        public string Password { get; set; }

        public bool IsEmailAutoActivation { get; set; }
        public bool IsPhoneNumberAutoActivation { get; set; }


        [StringLength(50)]
        public string Days { get; set; }
        public string Experience { get; set; }

        public List<LawyerTranslateDTO> Translates { get; set; }
        public List<RateingAndCommentsDTO> Reviews { get; set; }
    }
}
