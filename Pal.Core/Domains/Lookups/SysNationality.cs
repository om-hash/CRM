using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysNationality : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysNationalityTranslate> SysNationalityTranslates { get; set; }
    }

    public class SysNationalityTranslate : BaseEntityTranslate<int>
    {
        public int NationalityId { get; set; }

        [Required, StringLength(30)]
        public string NationalityName { get; set; }

        [ForeignKey(nameof(NationalityId))]
        public virtual SysNationality SysNationality { get; set; }
    }
}
