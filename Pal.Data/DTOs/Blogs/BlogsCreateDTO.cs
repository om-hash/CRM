using Microsoft.AspNetCore.Http;
using Pal.Core.Domains.Attachments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Blogs
{
    public class BlogsCreateDTO
    {
        public int Id { get; set; }
        public DateTime PostDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string Tags { get; set; }
        [StringLength(500)]
        public string MainImg { get; set; }
        public IFormFile MainImgFile { get; set; }

        public List<BlogsTranslateCreateDTO> Translates { get; set; }

        public List<Attachment> BlogImages { get; set; }
    }
}
