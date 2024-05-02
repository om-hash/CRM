using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pal.Core.Domains.Email;
using Pal.Core.Enums.Email;
using Pal.Services.FileManager;
using Pal.Services.Logger;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Pal.Services.Email
{

    //-----------------------------------------------------------------------------------------------
    public class EmailService : IEmailService
    {
        private readonly ILoggerService _logger;
        private readonly IConfiguration _configuration;
        private readonly SendGridAccountDetails _sendGridConfig;
        private readonly IFileManagerService _fileManagerService;


        public EmailService(ILoggerService logger, IConfiguration configuration, IFileManagerService fileManagerService)
        {
            _logger = logger;
            _configuration = configuration;
            _sendGridConfig = _configuration.GetSection("AppSettings:SendGrid").Get<SendGridAccountDetails>();
            _fileManagerService = fileManagerService;
        }

        public async Task<bool> SendEmail(string fromEmail, string fromName, string ToEmail, string ToName, string url, EmailType emailType, params object[] arg)
        {

            try
            {
                if (ToEmail == null || fromEmail == null)
                    return false;

                var client = new SendGridClient(_sendGridConfig.ApiKey);
                EmailAddress from = new(fromEmail, fromName);

                List<EmailAddress> tos = new()
                {
                    new EmailAddress(ToEmail, ToName)
                };

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, "", "", "", false);
                EmailAddress replayTo = new(fromEmail, fromName);
                msg.SetReplyTo(replayTo);

                switch (emailType)
                {
                    case EmailType.ConfirmAccount:
                        msg.SetTemplateId(_sendGridConfig.ConfirmAccountTemplate);
                        msg.SetTemplateData(new ConfirmEmail
                        {
                            Url = url,
                            Name = ToName,
                        });
                        msg.SetSubject("Confirm Email");
                        var result = await client.SendEmailAsync(msg);
                        break;

                    case EmailType.ResetPassword:
                        msg.SetTemplateId(_sendGridConfig.ResetPasswordTemplate);
                        msg.SetTemplateData(new ConfirmEmail
                        {
                            Url = url
                        });

                        await client.SendEmailAsync(msg);
                        break;
                    case EmailType.CompanyApprovement:
                        msg.SetTemplateId(_sendGridConfig.CompanyApprovedTemplate);
                        msg.SetTemplateData(new
                        {
                            Url = url,
                            Name = ToName,
                            CompanyName = arg[0],
                        });
                        var result1 = await client.SendEmailAsync(msg);
                        break;
                    case EmailType.CompanyDisApprovement:
                        msg.SetTemplateId(_sendGridConfig.CompanyDisApprovedTemplate);
                        msg.SetTemplateData(new
                        {
                            Url = url,
                            Name = ToName,
                            CompanyName = arg[0],
                        });
                        var result2 = await client.SendEmailAsync(msg);
                        break;
                    case EmailType.TourApprove:
                        msg.SetTemplateId(_sendGridConfig.TourApprovedTemplate);
                        msg.SetTemplateData(new ConfirmEmail
                        {
                            Url = url,
                            Name = ToName,
                      

                        });
                        var result3 = await client.SendEmailAsync(msg);
                        break;
                    case EmailType.TourDisApprove:
                        msg.SetTemplateId(_sendGridConfig.TourDisApprovedTemplate);
                        msg.SetTemplateData(new
                        {
                            Url = url,
                            Name = ToName,
                            RejectReason = arg[0],
                        });
                        var result4 = await client.SendEmailAsync(msg);
                        break;
                    case EmailType.TourCreatedCustomer:
                        msg.SetTemplateId(_sendGridConfig.TourCreatedCustmer);
                     
                        var result5 = await client.SendEmailAsync(msg);
                        break;
                    case EmailType.TourCreatedAgentAndAdmin:
                        msg.SetTemplateId(_sendGridConfig.TourCreatedAgentAndAdmin);

                        var result6 = await client.SendEmailAsync(msg);
                        break;
                }

                return true;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendEmail), ex);
                return false;
            }
        }


        public async Task<bool> SendCustomEmail(string fromEmail, string fromName, List<EmailAddress> to, string subject, string body, List<IFormFile> files)
        {
            try
            {
                if (to == null || fromEmail == null)
                    return false;

                var client = new SendGridClient(_sendGridConfig.ApiKey);
                EmailAddress from = new(fromEmail, fromName);

              

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, "", body, false);
                var attachments = new List<SendGrid.Helpers.Mail.Attachment>();


                if (files != null)
                    foreach (var item in files)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);


                    FileInfo fileInfo = new(item.FileName);
                    string fileName = item.FileName + fileInfo.Extension;
                    string fileNameWithPath = Path.Combine(path, fileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }


                        var bytes = File.ReadAllBytes(fileNameWithPath);
                        var file = Convert.ToBase64String(bytes);
                        attachments.Add(
                                new SendGrid.Helpers.Mail.Attachment
                                {
                                    Content = file,
                                    Filename = item.FileName,
                                    Type = item.ContentType,
                                    Disposition = "attachment"
                                }

                        );
                        File.Delete(fileNameWithPath);
                    }
                msg.Attachments = attachments;
                var result4 = await client.SendEmailAsync(msg);

                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendEmail), ex);
                return false;
            }
        }

    }
}