using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Company
{
   public class CreateNewCompanyTranslateDTO
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string CompanyName { get; set; }
        public string Content { get; set; }
        public int CompanyId { get; set; }
        public int LanguageId { get; set; }
    }
}
