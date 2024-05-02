using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Tour
{
   public class SaveTourDTO
    {
        public int TourId { get; set; }

        public int? AdvisorId { get; set; }

        public DateTime Date { get; set; }

        public int Time { get; set; }
        public string  CustomerNote { get; set; }

    }
}
