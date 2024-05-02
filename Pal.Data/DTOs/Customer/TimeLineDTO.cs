using Pal.Core.Enums.ActivityLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Customer
{
    public class TimeLineDTO
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Date { get; set; }
        public string Link { get; set; }
        public LogTransType Type { get; set; }

        public string TypeName { get; set; }
        public int TypeCount { get; set; }
    }
}
