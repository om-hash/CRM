using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysCompanyCategory : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysCompanyCategoryTranslate> SysCompanyCategoryTranslates { get; set; }
    }

    public class SysCompanyCategoryTranslate : BaseEntityTranslate<int>
    {
        public int CompanyCategoryId { get; set; }

        public string CompanyCategoryName { get; set; }

        [ForeignKey(nameof(CompanyCategoryId))]
        public virtual SysCompanyCategory SysCompanyCategory { get; set; }
    }


}
