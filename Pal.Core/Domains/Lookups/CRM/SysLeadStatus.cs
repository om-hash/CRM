using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysLeadStatus :BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysLeadStatusTranslate> Translates { get; set; }
    }

    public class SysLeadStatusTranslate : BaseEntityTranslate<int>
    {
        public int LeadStatusId { get; set; }

        [StringLength(50)]
        public string StatusName { get; set; }

        [ForeignKey(nameof(LeadStatusId))]
        public virtual SysLeadStatus LeadStatus { get; set; }
    }
}
