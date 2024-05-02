using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysCountry : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysCity> SysCities { get; set; }
        public virtual ICollection<SysCountryTranslate> SysCountryTranslates { get; set; }

    }

    public class SysCountryTranslate : BaseEntityTranslate<int>
    {
        public int CountryId { get; set; }

        [Required,StringLength(50)]
        public string CountryName { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual SysCountry SysCountry { get; set; }
    }
}
