using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Call
{
    public class CallDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int CallToType { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int? DealId { get; set; }

        public int CallTypeId { get; set; }

        public int? CallStatusId { get; set; }

        public int? CallPurposeId { get; set; }

        public int? CallResultId { get; set; }

        public DateTime CallStart { get; set; }
        public string Duration { get; set; }

        public string Subject { get; set; }

        public string CallDetails { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsAssignedToTask { get; set; }

    }
}
