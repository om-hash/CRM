using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.Advertisements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Project
{
    public class ProjectCardViewModel
    {
        //---
        public int Id { get; set; }
        //---
        public float AverageRating { get; set; }
        //---
        public string ProjectName { get; set; }
        //---
        public float MeterAvgPrice { get; set; }
        //---
        public string MainImg { get; set; }
        //---
        public bool IsFeatured { get; set; }
        //---
        public string FullAddress { get; set; }
        //---
        public string ProjectStatus { get; set; }
        //----
        public float Area { get; set; }

        // Address Lookups
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string NeighborhoodName { get; set; }

        public bool IsAddedToFavorite { get; set; }

        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }
        public double RealEstateAvgPrice { get; set; }

        public int CommentsCount { get; set; }

        public List<Attachment> Images { get; set; }

        //public List<AdvertisementsDTO> Advertisements { get; set; }



    }
}
