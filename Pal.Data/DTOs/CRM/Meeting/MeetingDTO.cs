using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Meeting
{
    public class MeetingDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Location { get; set; }

        public bool IsOnline { get; set; } = false;

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public int EmployeeId { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string Description { get; set; }
        public bool IsAssignedToTask { get; set; }


    }
}
