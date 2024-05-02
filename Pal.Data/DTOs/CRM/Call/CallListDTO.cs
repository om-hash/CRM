using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Call
{
    public class CallListDTO
    {
        public CallListDTO( )
        {
            DoneCalls = new List<DoneCallDTO>();
            ScheduledCalls = new List<ScheduledCallsDTO>();
        }

        public List<DoneCallDTO> DoneCalls { get; set; }
        public List<ScheduledCallsDTO> ScheduledCalls { get; set; }

    }
}
