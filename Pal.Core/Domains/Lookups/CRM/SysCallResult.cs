using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysCallResult : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysCallResultTranslate> Translates { get; set; }
    }
    public class SysCallResultTranslate : BaseEntityTranslate<int>
    {
        public int CallResultId { get; set; }

        [StringLength(50)]
        public string CallResultName { get; set; }

        [ForeignKey(nameof(CallResultId))]
        public virtual SysCallResult CallResult { get; set; }
    }
}
