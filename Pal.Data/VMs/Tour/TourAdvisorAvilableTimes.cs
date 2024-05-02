using System.ComponentModel.DataAnnotations;

namespace Pal.Data.VMs.Tour
{
    public class TourAdvisorAvilableTime
    {
        [StringLength(50)]
        public string AvilableTimeDescription { get; set; }

        public int TimeId { get; set; }

    }
}
