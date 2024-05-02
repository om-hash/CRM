using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.AboutUs
{
    public class AboutUsCreateTranslateDTO
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }

        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(300)]
        public string SubTitle { get; set; }
        public string Content { get; set; }
        public int AboutUsId { get; set; }
    }
}
