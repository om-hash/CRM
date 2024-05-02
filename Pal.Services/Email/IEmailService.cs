using Microsoft.AspNetCore.Http;
using Pal.Core.Enums.Email;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendCustomEmail(string fromEmail, string fromName, List<EmailAddress> to, string subject, string body, List<IFormFile> files);
        Task<bool> SendEmail(string fromEmail, string fromName, string ToEmail, string ToName, string url, EmailType emailType, params object[] arg);
    }
}