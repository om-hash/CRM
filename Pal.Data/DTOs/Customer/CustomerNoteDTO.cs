using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Customer
{
    public class CustomerNoteDTO
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? AddedByEmpId { get; set; }
        public string AddedByEmpName { get; set; }
        public string Content { get; set; }
    }
}
