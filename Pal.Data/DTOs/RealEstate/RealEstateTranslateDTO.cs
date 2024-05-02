using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RealEstate
{
    public class RealEstateTranslateDTO
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int RealEstateId { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public string Content { get; set; }

        public string Terms { get; set; }
    }
}
