using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RealEstate
{
    public class RealEstateExtraDetailsDTO
    {
        public int RealEstateId { get; set; }

        [Required, StringLength(50)]
        public string Key { get; set; }

        [Required, StringLength(255)]
        public string Value { get; set; }

        public bool IsBoolean { get; set; }
    }
}
