using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Blogs
{
    public class BlogsListDTO
    {
        public int Id { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}
