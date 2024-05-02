using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Company
{
   public class CompanyListDTO
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string CompanyName { get; set; }

        [Required, StringLength(25)]
        public string Phone { get; set; }

        [StringLength(25)]
        public string WhatsApp { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }


        [StringLength(100)]
        public string Website { get; set; }

        [StringLength(100)]
        public string ManagedBy { get; set; }

        [StringLength(255)]
        public string MainImgUrl { get; set; }

        public string CompanyLogo { get; set; }
        public string ManagerName { get; set; }
        [StringLength(55), EmailAddress]
        public string ManagerEmail { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        [StringLength(250)]
        public string FullAddress { get; set; }
        public bool IsRegisterd { get; set; }
        public DateTime LastLogInDate { get; set; }
        public int Categoryid { get; set; }
        public string CategoryName { get; set; }
        public string Password { get; set; }
        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string TwitterURL { get; set; }
        public string linkedinURL { get; set; }
        public bool IsDeleted { get; set; }

        public string UserName { get; set; }
        public List<CreateNewCompanyTranslateDTO> CompanyTranslates { get; set; }


    }
}
