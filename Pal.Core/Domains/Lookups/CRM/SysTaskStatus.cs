using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysTaskStatus : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysTaskStatusTranslate> Translates { get; set; }
    }
    public class SysTaskStatusTranslate : BaseEntityTranslate<int>
    {
        public int TaskStatusId { get; set; }

        [StringLength(50)]
        public string StatusName { get; set; }

        [ForeignKey(nameof(TaskStatusId))]
        public virtual SysTaskStatus TaskStatus { get; set; }
    }
}
