using Pal.Core.Enums.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Task
{
    public class TaskListDTO
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public string CreatedBy { get;set; }
        public string CreatedByName { get;set; }

        public string Status { get; set; }

        public int? StatusId { get; set; }
        public TaskPriority TaskPriority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsAllDay { get; set; } = false;

        public long? CustomerId { get; set; }


    }
}
