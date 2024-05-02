using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Call
{
    public class DoneCallDTO
    {
        public int Id { get; set; }
        public long CustomerId { get; set; }
        public string EmployeeName { get; set; }
        public string CallToTypeName { get; set; }
        public string CustomerName { get; set; }
        public string DealName { get; set; }
        public string CallTypeName { get; set; }
        public string CallStatusName { get; set; }
        public string CallPurposeName { get; set; }
        public string CallResultName { get; set; }
        public string CallStart { get; set; }
        public string Duration { get; set; }
        public string Subject { get; set; }
        public string CallDetails { get; set; }
    }
}
