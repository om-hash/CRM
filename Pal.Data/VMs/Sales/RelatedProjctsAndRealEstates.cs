using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;

namespace Pal.Data.VMs.Sales
{
    public class RelatedProjctsAndRealEstates
    {
        public List<RealEstateCardViewModel> RealEstates { get; set; }
        public List<ProjectCardViewModel> Projects { get; set; }
    }
}
