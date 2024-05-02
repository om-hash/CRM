using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Sales
{
   public class CreateNewSalesTranslateDTO
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string SalesName { get; set; }
        public string Content { get; set; }
        public int SalesId { get; set; }
        public int LanguageId { get; set; }
    }
}
