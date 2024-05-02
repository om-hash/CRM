using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysCallType : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysCallTypeTranslate> Translates { get; set; }
    }
    public class SysCallTypeTranslate : BaseEntityTranslate<int>
    {
        public int CallTypeId { get; set; }

        [StringLength(50)]
        public string CallTypeName { get; set; }

        [ForeignKey(nameof(CallTypeId))]
        public virtual SysCallType CallType { get; set; }
    }
}
