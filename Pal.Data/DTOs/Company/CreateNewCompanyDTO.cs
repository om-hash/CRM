using Microsoft.AspNetCore.Http;
using Pal.Core.Domains.Attachments;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Company
{
    public class CreateNewCompanyDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } //
        public string UserName { get; set; } //
        public string OwnerName { get; set; } //
        public int? CountryId { get; set; }
        public int? CityId { get; set; }

        [Required]
        public int? CompanyCategoryId { get; set; }
        
        [StringLength(250)]
        public string FullAddress { get; set; }

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

        [Required]
        public string ManagerName { get; set; }
        [StringLength(55), EmailAddress]
        public string ManagerEmail { get; set; }

        [StringLength(500)]
        public string MainImgUrl { get; set; }
        public IFormFile MainImageFile { get; set; } //

        [StringLength(500)]
        public string ComapnyLogo { get; set; }
        public IFormFile CompanyLogoFile { get; set; } //

        public int? ApproveState { get; set; }
        public bool IsActive { get; set; }

        [StringLength(100)]
        public string TaxNumber { get; set; }
        public string Password { get; set; } //

        public bool IsEmailAutoActivation { get; set; } //
        public bool IsPhoneNumberAutoActivation { get; set; } //


        [StringLength(500)]
        public string TaxDocumentImage { get; set; }
        public IFormFile TaxDocumentImageFile { get; set; } //
        [StringLength(500)]
        public string ManagerSignatureImage { get; set; }
        public IFormFile ManagerSignatureImageFile { get; set; } //
        [StringLength(500)]
        public string FacebookURL { get; set; }
        [StringLength(500)]
        public string InstagramURL { get; set; }
        [StringLength(500)]
        public string TwitterURL { get; set; }
        [StringLength(500)]
        public string linkedinURL { get; set; }
        public List<CreateNewCompanyTranslateDTO> CompanyTranslates { get; set; }
        public List<IFormFile> AttachmentsFiles { get; set; } //
        public List<Attachment> Attachments { get; set; } //


    }
}
