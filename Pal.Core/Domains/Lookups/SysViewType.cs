using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysViewType : BaseEntityNoIdentity<int>
    {
        public virtual ICollection<SysViewTypeTranslate> SysViewTypeTranslates { get; set; }
    }
    public class SysViewTypeTranslate : BaseEntityTranslate<int>
    {
        public int ViewTypeId { get; set; }
        public string ViewTypeName { get; set; }

        [ForeignKey(nameof(ViewTypeId))]
        public virtual SysViewType SysViewType { get; set; }
    }
}
