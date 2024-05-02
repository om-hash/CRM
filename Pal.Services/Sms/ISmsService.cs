using Pal.Core.Domains.Sms;
using Pal.Core.Enums;
using System.Threading.Tasks;

namespace Pal.Services.Sms
{
    public interface ISmsService
    {
 
        Task<ResponseResult> SendSms(string phoneNumber, string msg);
        Task<ResponseResult> SendSms(int templateId, string phoneNumber, int langId = 1, params object[] parameters);
    }
}