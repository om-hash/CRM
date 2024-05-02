using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysLeadSource: BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysLeadSourceTranslate> Translates { get; set; }
    }
    public class SysLeadSourceTranslate : BaseEntityTranslate<int>
    {
        public int LeadSourceId { get; set; }

        [StringLength(50)]
        public string SourceName { get; set; }

        [ForeignKey(nameof(LeadSourceId))]
        public virtual SysLeadSource LeadSource { get; set; }
    }
}
