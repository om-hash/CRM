using Newtonsoft.Json;
using Pal.Core.Domains.Lookups;
using System.Collections.Generic;

namespace Pal.Data.DTOs.Lookups
{
    public class SysAirConditionTypeDTO
    {
        public int Id { get; set; }

        public string AirConditionTypeName { get; set; }

        public List<ComboboxModelTranslate> Translates { get; set; }
    }
}
