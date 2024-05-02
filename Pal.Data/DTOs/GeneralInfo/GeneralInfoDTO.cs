using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.GeneralInfo
{
    public class GeneralInfoDTO
    {
        public int Id { get; set; }
        public string MainLogo { get; set; }
        public IFormFile MainLogoFile { get; set; }
        public List<GeneralInfoTranslateDTO> GeneralInformationTranslates { get; set; }
    }
    public class GeneralInfoTranslateDTO
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string PrivacyPolicyContent { get; set; }
        public string TermsConditionsContent { get; set; }
        public string TermsOfUseContent { get; set; }
        public string CustomerSignupContent { get; set; }
        public string AdvisorSignupContent { get; set; }
        public string CompanySignupContent { get; set; }
        public string RegistrationFeatures { get; set; }
        public string TourWithoutAdvisorWarning { get; set; }

    }

    public class GeneralInfoForViewDTO
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string MainLogo { get; set; }
        public string PrivacyPolicyContent { get; set; }
        public string TermsConditionsContent { get; set; }
        public string TermsOfUseContent { get; set; }
        public string CustomerSignupContent { get; set; }
        public string AdvisorSignupContent { get; set; }
        public string CompanySignupContent { get; set; }
        public string RegistrationFeatures { get; set; }
        public string TourWithoutAdvisorWarning { get; set; }
    }
}
