using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Statistics
{
    public class LogDTO
    {
        public string Id { get; set; }
        public DateTimeType Date { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionName { get; set; }
        public LogTransType LogTransType { get; set; }

        
    }
}
