using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysCallPurpose : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysCallPurposeTranslate> Translates { get; set; }
    }
    public class SysCallPurposeTranslate : BaseEntityTranslate<int>
    {
        public int CallPurposeId { get; set; }

        [StringLength(50)]
        public string CallPurposeName { get; set; }

        [ForeignKey(nameof(CallPurposeId))]
        public virtual SysCallPurpose CallPurpose { get; set; }
    }
}
