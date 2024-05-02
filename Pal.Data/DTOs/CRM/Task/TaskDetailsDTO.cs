using Pal.Core.Domains.Attachments;
using Pal.Core.Enums.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Task
{
    public class TaskDetailsDTO
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string EmployeeName { get; set; }

        public string CustomerName { get; set; }

        public string CreatedByName { get; set; }

        public int? StatusId { get; set; }

        public string TaskPriority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public bool IsAllDay { get; set; } = false;

        public List<Attachment> Attachments { get; set; }
    }
}
