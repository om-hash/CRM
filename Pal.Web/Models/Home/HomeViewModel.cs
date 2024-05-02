using Pal.Data.DTOs.Advertisements;
using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using Pal.Data.DTOs.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Models.Home
{
    public class HomeViewModel
    {
        public List<RealEstateCardViewModel> RealEstates { get; set; }
        public List<ProjectCardViewModel> Projects { get; set; }
        public List<RegionCardViewModel> Regions { get; set; }
        public List<AdvertisementsDTO> Advertisements { get; set; }

    }
}
