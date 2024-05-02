using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysDealType : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysDealTypeTranslate> Translates { get; set; }
    }
    public class SysDealTypeTranslate : BaseEntityTranslate<int>
    {
        public int DealTypeId { get; set; }

        [StringLength(50)]
        public string TypeName { get; set; }

        [ForeignKey(nameof(DealTypeId))]
        public virtual SysDealType DealType { get; set; }
    }
}
