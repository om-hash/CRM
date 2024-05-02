using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysDealStage : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysDealStageTranslate> Translates { get; set; }
    }
    public class SysDealStageTranslate : BaseEntityTranslate<int>
    {
        public int StageId { get; set; }

        [StringLength(50)]
        public string StageName { get; set; }

        [ForeignKey(nameof(StageId))]
        public virtual SysDealStage DealStage { get; set; }
    }
}

