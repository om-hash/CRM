using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysCallStatus : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysCallStatusTranslate> Translates { get; set; }
    }
    public class SysCallStatusTranslate : BaseEntityTranslate<int>
    {
        public int CallStatusId { get; set; }

        [StringLength(50)]
        public string CallStatusName { get; set; }

        [ForeignKey(nameof(CallStatusId))]
        public virtual SysCallStatus CallStatus { get; set; }
    }
}
