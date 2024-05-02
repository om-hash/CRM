using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.ContactUs
{
    public class ContactUsSocialMediaDTO
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Icon { get; set; }

        public string URL { get; set; }
    }
}
