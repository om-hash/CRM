using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.AboutUs
{
    public class AboutUsCreateDTO
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string MainImage { get; set; }

        public IFormFile MainImageFile { get; set; }

        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }
        public List<AboutUsCreateTranslateDTO> AboutUsTranslates { get; set; }

    }
}
