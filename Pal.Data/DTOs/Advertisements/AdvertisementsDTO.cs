using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Advertisements
{
    public class AdvertisementsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SildeImage { get; set; }
        public IFormFile SildeImageFile { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}
