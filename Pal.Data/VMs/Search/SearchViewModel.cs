using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Search
{
    public class SearchViewModel
    {
        public List<int> realEstates { get; set; }

        public List<int> projects { get; set; }
    }
}
