using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Blogs
{
    public class BlogsTranslateCreateDTO
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int LanguageId { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        [StringLength(250)]
        public string SubTitle { get; set; }

        public string Content { get; set; }
    }
}
