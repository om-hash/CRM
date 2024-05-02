using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pal.Core.Domains.Sms;
using Pal.Core.Enums;
using Pal.Core.Enums.ActivityLog;
using Pal.Data.Contexts;
using Pal.Services.Logger;
using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Pal.Services.Sms
{
    public class SmsService : ISmsService
    {
        private readonly ILoggerService _logger;
        private readonly IConfiguration _configuration;
        private readonly TwilioAccountDetails _twilioConfig;
        private readonly ApplicationDbContext _ApplicationDbContext;
        public SmsService(ILoggerService logger, IConfiguration configuration, ApplicationDbContext ApplicationDbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _twilioConfig = _configuration.GetSection("AppSettings:Twilio").Get<TwilioAccountDetails>();
            _ApplicationDbContext = ApplicationDbContext;
        }

        //-----------------------------------------------------------------------------
        public async Task<ResponseResult> SendSms(int templateId, string phoneNumber, int langId, params object[] parameters)
        {
            try
            {
                TwilioClient.Init(_twilioConfig.AccountSid, _twilioConfig.AuthToken);
                var template = await _ApplicationDbContext.SmsTemplates.Include(a=>a.Translates).FirstOrDefaultAsync(a => a.Id == templateId);
                string msg = string.Format(template.Translates.FirstOrDefault(a => a.LanguageId == langId).MessageTemplate, parameters);

                // check the tempalte
                if (string.IsNullOrEmpty(msg))
                    return new ResponseResult(ResponseType.Success, "Msg Template Empty!");

                // sending msg
                var message = await MessageResource.CreateAsync(
                    body: msg,
                    from: new PhoneNumber(_twilioConfig.SenderPhoneNumber),
                    to: new PhoneNumber(phoneNumber)
                );

                // check if sent or not!
                if (string.IsNullOrEmpty(message.Sid))
                    return new ResponseResult(ResponseType.Success, "Unknown error!");

                // Saving in the DB
                _ApplicationDbContext.SmsMsgs.Add(new SmsMsg
                {
                    Msg = msg,
                    PhoneNumber = phoneNumber,
                });
                await _ApplicationDbContext.SaveChangesAsync();

                return new ResponseResult(ResponseType.Success, message.Sid);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendSms), ex, Importance.High);
                return new ResponseResult(ResponseType.Error, ex);
            }
        }

        //-----------------------------------------------------------------------------
        public async Task<ResponseResult> SendSms(string phoneNumber, string msg)
        {
            try
            {
                TwilioClient.Init(_twilioConfig.AccountSid, _twilioConfig.AuthToken);

                var message = await MessageResource.CreateAsync(
                    body: msg,
                    from: new PhoneNumber(_twilioConfig.SenderPhoneNumber),
                    to: new PhoneNumber(phoneNumber)
                );

                // check if sent or not!
                if (string.IsNullOrEmpty(message.Sid))
                    return new ResponseResult(ResponseType.Success, "Unknown error!");

                // Saving in the DB
                _ApplicationDbContext.SmsMsgs.Add(new SmsMsg
                {
                    Msg = msg,
                    PhoneNumber = phoneNumber,
                });
                await _ApplicationDbContext.SaveChangesAsync();

                return new ResponseResult(ResponseType.Success, message.Sid);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendSms), ex);
                return new ResponseResult(ResponseType.Error, ex);
            }
        }



    }
}
