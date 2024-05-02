using Newtonsoft.Json;
using Pal.Core.Domains.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Lookups
{
    public class SysRegionDTO
    {
        public int Id { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public string FullAddress { get; set; }

        [StringLength(50)]
        public string RegionName { get; set; }
        public List<ComboboxModelTranslate> Translates { get; set; }
    }
}
