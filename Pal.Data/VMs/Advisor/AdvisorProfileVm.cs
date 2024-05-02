using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Advisor
{
    public class AdvisorProfileVm
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CityName { get; set; }
        public string SpeakingLanguages { get; set; }
        public string WhatsApp { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public float AvrageRating { get; set; } = 0;
        public string Experience { get; set; }
        public int TourCount { get; set; }
        public string Days { get; set; }
        public List<int> WordDays { get; set; }
        public string StartWork { get; set; }
        public string EndWork { get; set; }
    }
}
