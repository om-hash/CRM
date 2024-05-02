using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Blogs
{
    public class BlogsCardVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public string MainImage { get; set; }
        public DateTime PostData { get; set; }
    }
}
