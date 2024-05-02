using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Meeting
{
    public class MeetingListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Location { get; set; }

        public bool IsOnline { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string FromDateTime { get; set; }
        public string  ToDateTime { get; set; }

        public string EmployeeName { get; set; }

        public string CustomerName { get; set; }

        public long? CustomerId { get; set; }

        public string Description { get; set; }
    }
}
