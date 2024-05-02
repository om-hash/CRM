using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Tour
{
    public class TourDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CityId { get; set; }
        public int TimeId { get; set; }
        public decimal Duration { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public int AdvisorId { get; set; }


        public List<int> Projects { get; set; }
        public List<int> RealEstates { get; set; }
    }
}
