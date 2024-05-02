using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.OurServices
{
    public class OurSerivesDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public List<OurSerivesTranslateDTO> OurServicesTranslates { get; set; }
    }
}
