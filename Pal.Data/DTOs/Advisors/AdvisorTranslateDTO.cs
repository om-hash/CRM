using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Advisors
{
    public class AdvisorTranslateDTO
    {
        public int Id { get; set; }
        public int AdvisorId { get; set; }
        public int LanguageId { get; set; }

        [Required,StringLength(150)]
        public string FullName { get; set; }
    }
}
