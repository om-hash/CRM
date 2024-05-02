using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.ContactUs
{
    public class ContactUsTranslateCreateDTO
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Subtitle { get; set; }
        public string OpenHours { get; set; }
        public int ContactUsId { get; set; }

    }
}
