using Pal.Data.DTOs.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.VMs
{
    public class FullAddressViewModel
    {
        public List<SysCountryDTO> Countries { get; set; }
        public List<SysCityDTO> Cities { get; set; }
        public List<SysRegionDTO> Regions { get; set; }
        public List<SysNeighborhoodDTO> Neighborhoods { get; set; }
    }
}
