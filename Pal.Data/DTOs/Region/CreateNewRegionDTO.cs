using Microsoft.AspNetCore.Http;
using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.RatingAndComment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Region
{
    public class CreateNewRegionDTO
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        

        [StringLength(15)]
        public string Population { get; set; }

        public float MeterAvgPrice { get; set; }

        [StringLength(30)]
        public string Lat { get; set; }

        [StringLength(30)]
        public string Lng { get; set; }

        public string FullAddress { get; set; }

        [StringLength(500)]
        public string MainImg { get; set; }
        public IFormFile MainImageFile { get; set; }

        public string LocationUrl { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsOnlyLookup { get; set; }
        public float MinimumRentPrice { get; set; }
        public float MaxRentPrice { get; set; }

        public List<CreateNewRegionTranslateDTO> SysRegionTranslates { get; set; }
        public List<SysRegionSiteDTO> SysRegionSites { get; set; }
        public List<Attachment> RegionImages { get; set; }
        public List<RateingAndCommentsDTO> Reviews { get; set; }


    }
}
