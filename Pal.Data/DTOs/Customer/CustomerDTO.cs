using Microsoft.AspNetCore.Http;
using Pal.Core.Domains.Attachments;
using Pal.Core.Enums.Customer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Customer
{

    public class CustomerDTO
    {
        public long Id { get; set; }
        public string? UserId { get; set; }


        public string? Username { get; set; }

        public string? Password { get; set; }

        public CustomerStatus CustomerStatus { get; set; } = CustomerStatus.New;

        public int NationalityId { get; set; }

        public int CountryId { get; set; }

        public bool IsEmailVerified { get; set; }


        [StringLength(100)]
        public string? Email { get; set; }


        [StringLength(50)]
        public string? FullName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(20)]
        public string? WhatsappNumber { get; set; }


        #region CRM Info  


        public bool IsLead { get; set; }

        [StringLength(20)]
        public string? MobileNumber { get; set; }

        public int LeadSourceId { get; set; }


        [StringLength(50)]
        public string WorkCompany { get; set; }

        //public string? MostInterestingProjects { get; set; }

        public int LeadStatusId { get; set; }

        public int LeadRatingId { get; set; }

        public int CityId { get; set; }

        public int RegionId { get; set; }

        public int FollowedByEmpId { get; set; }

        [StringLength(250)]
        public string? FullAddress { get; set; }

        [StringLength(100)]
        public string? TwitterId { get; set; }

        [StringLength(100)]
        public string? Facebook { get; set; }

        [StringLength(100)]
        public string? Instagram { get; set; }

        public bool IsRegisterdByAdmin { get; set; }

        public string? CreatedBy { get; set; }

        public string? MainImage { get; set; }

        public IFormFile? MainImageFile { get; set; }

        #endregion CRM Info 

        #region Children
        public List<CustomerFavoriteDTO>? CustomerFavorites { get; set; }
        public List<CustomerNoteDTO>? CustomerNotes { get; set; }
        //public List<int> MostInterestingProjectsId { get; set; }

        public List<Attachment>? Attachments { get; set; }

        #endregion

    }
}
