using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Lawyer
{
    public class LawyerProfileVm
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CityName { get; set; }
        public string SpeakingLanguages { get; set; }
        public string WhatsApp { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MainImg { get; set; }
        public float AvrageRating { get; set; } = 0;
        public string Experience { get; set; }
        public int TourCount { get; set; }
        public List<string> WorkDays { get; set; }
        public string WorkFrom { get; set; }
        public string WorkTo { get; set; }
    }
}
