using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysLeadRating : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysLeadRatingTranslate> Translates { get; set; }
    }

    public class SysLeadRatingTranslate : BaseEntityTranslate<int>
    {
        public int LeadRatingId { get; set; }

        [StringLength(50)]
        public string RateName { get; set; }

        [ForeignKey(nameof(LeadRatingId))]
        public virtual SysLeadRating LeadRating { get; set; }
    }
}
