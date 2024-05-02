using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.AboutUs
{
    public class AboutUsListDTO
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string MainImage { get; set; }
        public IFormFile MainImageFile { get; set; }
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(300)]
        public string SubTitle { get; set; }
        public string Content { get; set; }
        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }
    }
}
