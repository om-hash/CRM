using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysDecision : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysDecisionTranslate> Translates { get; set; }
    }
    public class SysDecisionTranslate : BaseEntityTranslate<int>
    {
        public int DecisionId { get; set; }

        [StringLength(50)]
        public string DecisionName { get; set; }

        [ForeignKey(nameof(DecisionId))]
        public virtual SysDecision Decision { get; set; }
    }
}
