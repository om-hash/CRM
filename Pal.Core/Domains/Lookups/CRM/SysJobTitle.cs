using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups.CRM
{
    public class SysJobTitle : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysJobTitleTranslate> Translates { get; set; }
    }
    public class SysJobTitleTranslate : BaseEntityTranslate<int>
    {
        public int JobTitleId { get; set; }

        [StringLength(50)]
        public string JobTitleName { get; set; }

        [ForeignKey(nameof(JobTitleId))]
        public virtual SysJobTitle JobTitle { get; set; }
    }
}
