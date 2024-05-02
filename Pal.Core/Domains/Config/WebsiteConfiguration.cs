using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Config
{
    public class WebsiteConfiguration :BaseEntity<int>
    {
        public bool IsUseIFrameTemplateInControlPanel { get; set; }

        [StringLength(100)]
        public string GoogleMapsAPIKey { get; set; }
    }
}
