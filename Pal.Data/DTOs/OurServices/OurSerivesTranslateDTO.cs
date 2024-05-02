using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.OurServices
{
    public class OurSerivesTranslateDTO
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Content { get; set; }
        public int LanguageId { get; set; }
    }
}
