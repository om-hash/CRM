using Pal.Core.Domains.Lookups;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Core.Enums.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Customers
{
    public class Customer : BaseEntityWithLog<long>
    {
        public string? UserId { get; set; }

        public CustomerStatus CustomerStatus { get; set; }

        public int NationalityId { get; set; }

        public int CountryId { get; set; }


        [StringLength(50)]
        public string? FullName { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(20)]
        public string? WhatsappNumber { get; set; }

        public DateTime RegisterDate { get; set; }

        #region CRM Info  
        public bool IsLead { get; set; }

        [StringLength(20)]
        public string? MobileNumber { get; set; }

        public int? LeadSourceId { get; set; }


        //[StringLength(50)]
        //public string? WorkCompany { get; set; }

        public int? LeadStatusId { get; set; }

        public int? LeadRatingId { get; set; }

        public int? CityId { get; set; }

        public int? RegionId { get; set; }

        //public string? MostInterestingProjects { get; set; }


        [StringLength(100)]
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

        public string? MainImage { get; set; }




        #endregion CRM Info 

        #region Children
        public ICollection<CustomerNotes>? CustomerNotes { get; set; }

        public virtual ICollection<CustomerFavorite>? CustomerFavorites { get; set; }
        #endregion Children


        #region Fathers

        [ForeignKey(nameof(LeadStatusId))]
        public virtual SysLeadStatus? LeadStatus { get; set; }

        [ForeignKey(nameof(LeadRatingId))]
        public virtual SysLeadRating? LeadRating { get; set; }

        [ForeignKey(nameof(NationalityId))]
        public virtual SysNationality? SysNationality { get; set; }


        [ForeignKey(nameof(CountryId))]
        public virtual SysCountry? SysCountry { get; set; }


        [ForeignKey(nameof(LeadSourceId))]
        public virtual SysLeadSource? LeadSource { get; set; }
        //public string WorkCompany { get; set; }

        #endregion Fathers



    }
}
