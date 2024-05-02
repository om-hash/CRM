using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.OurServices
{
    public class OurServicesListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

    }
}
