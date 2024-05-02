using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Region
{
    public class CreateNewRegionTranslateDTO
    {
        public int Id { get; set; }
        public string RegionName { get; set; }
        public int RegionId { get; set; }
        public string Content { get; set; }
        public string Blog { get; set; }
        public int LanguageId { get; set; }
        [StringLength(255)]
        public string SubTitle { get; set; }
    }
}
