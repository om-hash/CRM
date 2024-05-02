using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Region
{
    public class RegionSiteVM
    {
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string SiteImage { get; set; }
        public string PlaceTypeName { get; set; }
        public int RegionId { get; set; }

    }
}
