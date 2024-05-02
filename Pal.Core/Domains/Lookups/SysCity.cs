using Pal.Core.Domains;
using Pal.Core.Domains.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysCity : BaseEntityNoIdentity<int>
    {
        public int CountryId { get; set; }

        public virtual ICollection<SysRegion> SysRegions { get; set; }
        public virtual ICollection<SysCityTranslate> SysCityTranslates { get; set; }


        [ForeignKey(nameof(CountryId))]
        public virtual SysCountry SysCountry { get; set; }
    }

    public class SysCityTranslate : BaseEntityTranslate<int>
    {
        public int CityId { get; set; }

        [StringLength(50), Required]
        public string CityName { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual SysCity City { get; set; }
    }
}
