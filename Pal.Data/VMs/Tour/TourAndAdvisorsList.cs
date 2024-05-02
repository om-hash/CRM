using Pal.Data.DTOs.Advisors;
using Pal.Data.DTOs.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Tour
{
    public class TourAndAdvisorsList
    {
        public TourCreateDTO Tour { get; set; }
        public IEnumerable<AdvisorListDTO> AdvisorList { get; set; }
    }
}
