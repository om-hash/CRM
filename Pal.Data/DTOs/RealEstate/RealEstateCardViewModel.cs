using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.Advertisements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RealEstate
{
    public class RealEstateCardViewModel
    {
        //---
        public int Id { get; set; }
        //---
        public string Title { get; set; }
        //---
        public string MainImgUrl { get; set; }
        //---
        public string FullAddress { get; set; }
        //---
        public string RoomCount { get; set; }
        //---
        public int BathRoomsCount { get; set; }
        //---
        public float AreaTotal { get; set; }
        //---
        public float Price { get; set; }
        //---
        public bool IsFeatured { get; set; }
        //---
        public bool IsNationaltySupport { get; set; }
        public DateTime CompletionDate { get; set; }

        // Address Lookups
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string NeighborhoodName { get; set; }

        public bool IsAddedToFavorite { get; set; }

        public float AverageRating { get; set; }
        public int CommentsCount { get; set; }

        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }

        public int ViewsCount { get; set; }
        public List<Attachment> Images { get; set; }
    }
}
