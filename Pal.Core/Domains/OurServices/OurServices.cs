using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.OurServices
{
    public class OurServices: BaseEntity<int>
    {
        public string Image { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public List<OurServicesTranslate> OurServicesTranslates { get; set; }
    }

    public class OurServicesTranslate : BaseEntityTranslate<int>
    {
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Content { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public virtual OurServices OurServices { get; set; }

    }
}
