using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Advisors
{
    public class AdviserBusyTimesDTO
    {
        public int? AdviserId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }


    }
}
