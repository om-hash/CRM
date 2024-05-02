using Pal.Data.DTOs.Lookups;
using System.Collections.Generic;

namespace Pal.Web.Models
{
    public class FullAddressViewModel
    {
        public List<SysCountryDTO> Countries { get; set; }
        public List<SysCityDTO> Cities { get; set; }
        public List<SysRegionDTO> Regions { get; set; }
        public List<SysNeighborhoodDTO> Neighborhoods { get; set; }
    }
}
