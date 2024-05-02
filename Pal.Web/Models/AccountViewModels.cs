using Microsoft.AspNetCore.Http;
using Pal.Core.Domains.Attachments;
using Pal.Core.Enums.Account;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pal.Web.Models
{
    public class CustomerRegisterViewModel
    {
        [StringLength(256), Required]
        public string UserName { get; set; }

        [StringLength(256), Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare(nameof(Password), ErrorMessage = "Passwords not match")]
        public string ConfirmPassword { get; set; }
   

        [Required, StringLength(20)]
        public string PhoneNumber { get; set; }

        [ StringLength(20)]
        public string WhatsappNumber { get; set; }


        public int CountryId { get; set; }

        [Required, StringLength(500)]
        public string Address { get; set; }

        public int NationalityId { get; set; }
    }

    //------------------------------------------------------
    public class SalesRegisterViewModel
    {
        [StringLength(256), Required]
        public string UserName { get; set; }

        [StringLength(256), Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public int SalesCategoryId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare(nameof(Password), ErrorMessage = "Passwords not match")]
        public string ConfirmPassword { get; set; }


        [Required, StringLength(20)]
        public string UserPhoneNumber { get; set; }

        [StringLength(20)]
        public string WhatsappNumber { get; set; }

        [Required]
        public string SalesName { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int CityIdd { get; set; }

        public string SalesFullAddress { get; set; }

        public string Website { get; set; }

        [Required]
        public string ContactEmail { get; set; }


        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }

        [Required]
        public string SalesPhoneNumber { get; set; }

        [StringLength(100)]
        public string TaxNumber { get; set; }

        [StringLength(500)]
        public string TaxDocumentImage { get; set; }

        public IFormFile TaxDocumentImageFile { get; set; }
        [StringLength(500)]
        public string ManagerSignatureImage { get; set; }
        public IFormFile ManagerSignatureImageFile { get; set; }
        public List<IFormFile> Attachments { get; set; }

        public IFormFile Logo { get; set; }

        [StringLength(500)]
        public string FacebookURL { get; set; }
        [StringLength(500)]
        public string InstagramURL { get; set; }
        [StringLength(500)]
        public string TwitterURL { get; set; }
        [StringLength(500)]
        public string linkedinURL { get; set; }

    } public class CompanyRegisterViewModel
    {
        [StringLength(256), Required]
        public string UserName { get; set; }

        [StringLength(256), Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public int CompanyCategoryId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare(nameof(Password), ErrorMessage = "Passwords not match")]
        public string ConfirmPassword { get; set; }


        [Required, StringLength(20)]
        public string UserPhoneNumber { get; set; }

        [StringLength(20)]
        public string WhatsappNumber { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int CityIdd { get; set; }

        public string CompanyFullAddress { get; set; }

        public string Website { get; set; }

        [Required]
        public string ContactEmail { get; set; }


        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }

        [Required]
        public string CompanyPhoneNumber { get; set; }

        [StringLength(100)]
        public string TaxNumber { get; set; }

        [StringLength(500)]
        public string TaxDocumentImage { get; set; }

        public IFormFile TaxDocumentImageFile { get; set; }
        [StringLength(500)]
        public string ManagerSignatureImage { get; set; }
        public IFormFile ManagerSignatureImageFile { get; set; }
        public List<IFormFile> Attachments { get; set; }

        public IFormFile Logo { get; set; }

        [StringLength(500)]
        public string FacebookURL { get; set; }
        [StringLength(500)]
        public string InstagramURL { get; set; }
        [StringLength(500)]
        public string TwitterURL { get; set; }
        [StringLength(500)]
        public string linkedinURL { get; set; }

    }

    //------------------------------------------------------
    public class AdvisorRegisterViewModel
    {
        [StringLength(256), Required]
        public string UserName { get; set; }

        [StringLength(256), Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare(nameof(Password), ErrorMessage = "Passwords not match")]
        public string ConfirmPassword { get; set; }


        [Required, StringLength(20)]
        public string UserPhoneNumber { get; set; }

        [StringLength(20)]
        public string WhatsappNumber { get; set; }

        public string SpeakingLanguages { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int CityId { get; set; }

        public string FullAddress { get; set; }

        public int WorkStart { get; set; }

        public int WorkEnd { get; set; }

        public string Days { get; set; }

        public IFormFile MainImg { get; set; }
        public IFormFile CertificateFile { get; set; }
    }

    //------------------------------------------------------
    public class LawyerRegisterViewModel
    {
        [StringLength(256), Required]
        public string UserName { get; set; }

        [StringLength(256), Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare(nameof(Password), ErrorMessage = "Passwords not match")]
        public string ConfirmPassword { get; set; }


        [Required, StringLength(20)]
        public string UserPhoneNumber { get; set; }

        [StringLength(20)]
        public string WhatsappNumber { get; set; }

        public string SpeakingLanguages { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int CityId { get; set; }

        public string FullAddress { get; set; }

        public int WorkStart { get; set; }

        public int WorkEnd { get; set; }

        public string Days { get; set; }

        public IFormFile MainImg { get; set; }
        public IFormFile CertificateFile { get; set; }
    }
    //--------------------------------------------------------------
    public class LoginViewModel
    {
        [StringLength(256), Required]
        public string UserNameOrEmail { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }


    //--------------------------------------------------------------
    public class ResetPasswordViewModel
    {

        public string UserId { get; set; }

        [Required]
        public string VerificationCode { get; set; }



        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


    }

    public class ResetPhoneNumberVM
    {
        public string Id { get; set; }
        public string phoneNumber { get; set; }
        public string VerificationCode { get; set; }
    }

    public class ChangePasswordVM
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }


}
