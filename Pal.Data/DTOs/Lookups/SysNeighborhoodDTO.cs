using Newtonsoft.Json;
using Pal.Core.Domains.Lookups;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Lookups
{
    public class SysNeighborhoodDTO
    {

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public int RegionId { get; set; }
        public string RegionName { get; set; }

        [StringLength(50)]
        public string NeighborhoodName { get; set; }

        public List<ComboboxModelTranslate> Translates { get; set; }

        // Total Count of Neighborhoods in database 
        public int TotalCount { get; set; }
    }
}
