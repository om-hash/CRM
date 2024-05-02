using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Filtering
{
    public class ProjectFilteringVM
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int NeighborhoodId { get; set; }

        public int ProjectStatusId { get; set; }
        public int CompanyId { get; set; }
        public int ProjectTypeId { get; set; }
    }
}
