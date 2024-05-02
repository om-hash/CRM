using Pal.Core.Domains.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Lookups
{
    public class SysRealEstateTypeDTO
    {
        public int Id { get; set; }
        public int RealEstateClassId { get; set; }
        public string RealEstateClassName { get; set; }

        public string RealEstateTypeName { get; set; }

        public List<ComboboxModelTranslate> Translates { get; set; }
    }
}
