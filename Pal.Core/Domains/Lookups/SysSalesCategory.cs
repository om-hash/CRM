using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysSalesCategory : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysSalesCategoryTranslate> SysSalesCategoryTranslates { get; set; }
    }

    public class SysSalesCategoryTranslate : BaseEntityTranslate<int>
    {
        public int SalesCategoryId { get; set; }

        public string SalesCategoryName { get; set; }

        [ForeignKey(nameof(SalesCategoryId))]
        public virtual SysSalesCategory SysSalesCategory { get; set; }
    }


}
