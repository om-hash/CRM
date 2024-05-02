using Pal.Core.Domains.Lookups;
using Pal.Core.Enums.Approvment;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Companies
{
    public class Company : BaseEntityWithLog<int>
    {
        public int? CountryId { get; set; }
        //public int? CountryName { get; set; }
        public int? CompanyCategoryId { get; set; }

        public int? CityId { get; set; }

        //[StringLength(250)]
        public string FullAddress { get; set; }

        //[StringLength(25)]
        public string Phone { get; set; }

        //[StringLength(25)]
        public string WhatsApp { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        //[StringLength(100)]
        public string Website { get; set; }

        //[StringLength(100)]
        public string ManagedBy { get; set; }

        //[StringLength(25)]
        public string ManagerName { get; set; }
        [EmailAddress]
        public string ManagerEmail { get; set; }

        //[StringLength(500)]
        public string ComapnyLogo { get; set; }
        public float? AvrageRating { get; set; } = 0;

        //[StringLength(500)]
        public string MainImgUrl { get; set; }
        public ApproveStatment? ApproveState { get; set; }
        public bool IsActive { get; set; }

        //[StringLength(100)]
        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        //[StringLength(100)]
        public string TaxNumber { get; set; }

        [StringLength(500)]
        public string TaxDocumentImage { get; set; }

        //[StringLength(500)]
        public string ManagerSignatureImage { get; set; }
        public bool IsTearmAndConditionsAccepted { get; set; } = true;

        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string TwitterURL { get; set; }
        public string linkedinURL { get; set; }

        public virtual ICollection<CompanyTranslate> CompanyTranslates { get; set; }

        #region Fathers
        [ForeignKey(nameof(CityId))]
        public virtual SysCity SysCity { get; set; }
        [ForeignKey(nameof(CompanyCategoryId))]
        public virtual SysCompanyCategory SysCompanyCategory { get; set; }
        #endregion


        public static List<MySelectListItem> RequiredFields
        {
            get => new()
            {
                new MySelectListItem{Text = "Country", Value = "CountryId"},
                new MySelectListItem{Text = "Company Category", Value = "CompanyCategoryId"},
                new MySelectListItem{Text = "City", Value = "CityId"},
                new MySelectListItem{Text = "Full Address", Value = "FullAddress"},
                new MySelectListItem{Text = "Phone", Value = "Phone"},
                new MySelectListItem{Text = "WhatsApp", Value = "WhatsApp"},
                new MySelectListItem{Text = "Email", Value = "Email"},
                new MySelectListItem{Text = "Website", Value = "Website"},
                new MySelectListItem{Text = "Manager Name", Value = "ManagerName"},
                new MySelectListItem{Text = "Manager Email", Value = "ManagerEmail"},
                new MySelectListItem{Text = "Tax Number", Value = "TaxNumber"},
                new MySelectListItem{Text = "Facebook URL", Value = "FacebookURL"},
                new MySelectListItem{Text = "Instagram URL", Value = "InstagramURL"},
                new MySelectListItem{Text = "Twitter URL", Value = "TwitterURL"},
                new MySelectListItem{Text = "linkedin URL", Value = "linkedinURL"},
                new MySelectListItem{Text = "linkedin URL", Value = "linkedinURL"},
                new MySelectListItem{Text = "linkedin URL", Value = "linkedinURL"},
                new MySelectListItem{Text = "Company Name", Value = "CompanyName" },
                new MySelectListItem{Text = "Content", Value = "Content" },
            };
        }
    }

    //--------------------------------------------------------------
    public class CompanyTranslate : BaseEntityTranslateWithLog<int>
    {
        public int? CompId { get; set; }

        //[StringLength(50)]
        public string CompanyName { get; set; }
        public string Content { get; set; }


        [ForeignKey(nameof(CompId))]
        public virtual Company Company { get; set; }
    }


   
}
