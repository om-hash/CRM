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
    public class SysCountryDTO
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string CountryName { get; set; }

        public List<ComboboxModelTranslate> Translates { get; set; }
    }
}
