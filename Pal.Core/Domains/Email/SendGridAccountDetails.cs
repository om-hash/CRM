namespace Pal.Core.Domains.Email
{
    public class SendGridAccountDetails
    {
        public string ApiKey { get; set; } = "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";
        public string ConfirmAccountTemplate { get; set; } 
        public string ResetPasswordTemplate { get; set; } = " d-2abf83b9490a44e7a777c0225a79ffe8";
        public string CompanyApprovedTemplate { get; set; }
        public string CompanyDisApprovedTemplate { get; set; }
        public string TourApprovedTemplate { get; set; }
        public string TourDisApprovedTemplate { get; set; }
        public string TourCreatedCustmer { get; set; }
        public string TourCreatedAgentAndAdmin { get; set; }
    }
}
