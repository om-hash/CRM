using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Sms
{
    public class TwilioAccountDetails
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string SenderPhoneNumber { get; set; }
    }
}
