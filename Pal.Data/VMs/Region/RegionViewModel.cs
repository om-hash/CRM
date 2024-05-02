using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.Advertisements;
using Pal.Data.DTOs.RealEstate;
using Pal.Data.DTOs.Region;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Region
{
    public class RegionViewModel
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string MainImg { get; set; }
        public string RegionName { get; set; }
        [StringLength(15)]
        public string Population { get; set; }
        public float MeterAvgPrice { get; set; }
        [StringLength(30)]
        public string Lat { get; set; }
        [StringLength(30)]
        public string Lng { get; set; }
        public float AvrageRating { get; set; } = 0;

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }

        public bool IsFeatured { get; set; }

        public string Content { get; set; }
        public string SubTitle { get; set; }
        public float MinimumRentPrice { get; set; }
        public float MaxRentPrice { get; set; }
        public List<RealEstateCardViewModel> RealEstateCardsViewModel { get; set; }
        public List<Attachment> RegionImages { get; set; }

        public List<SysRegionSiteDTO> RegionSites { get; set; }



    }
}
