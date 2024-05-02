using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.FooterAndHeader
{
    public class BlogsVM
    {
        public int BlogId { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(250)]
        public string SubTitle { get; set; }

        public string Content { get; set; }
    }
}
