using Pal.Core.Domains.Lookups;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Lookups
{
    public class SysCityDTO
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        [StringLength(50)]
        public string CityName { get; set; }
        public List<ComboboxModelTranslate> Translates { get; set; }

    }
}
