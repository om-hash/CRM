using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Lawyer
{
    public class LawyerListDTO
    {
        public int Id { get; set; }

        public int CityId { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string SpeakingLanguages { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string WhatsApp { get; set; }
        public string Experience { get; set; }

        [StringLength(20)]
        public string Days { get; set; }
        public int WorkStart { get; set; }
        public int WorkEnd { get; set; }


        [StringLength(500)]
        public string Image { get; set; }
    }
}
