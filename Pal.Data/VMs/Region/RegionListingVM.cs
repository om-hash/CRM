using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Region
{
    public class RegionListingVM
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }

        public string RegionName { get; set; }
        public float MeterAvgPrice { get; set; }

        public int RealEstatesCount { get; set; }
        public int ProjectsCount { get; set; }


    }
}
