using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Region
{
    public class SysRegionSiteDTO
    {
        public int Id { get; set; }
        public int PlacesTypeId { get; set; }
        public string PlaceTypeName { get; set; }
        public int RegionId { get; set; }
        public IFormFile MainImageFile { get; set; }
        public string MainImage { get; set; }
        public string SiteName { get; set; }
    }
}
