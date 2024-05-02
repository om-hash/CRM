using Pal.Core.Domains.Lookups;
using Pal.Data.DTOs.Advertisements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Region
{
    public class RegionListDTO
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string Blog { get; set; }

        [StringLength(255)]
        public string MainImgUrl { get; set; }

        [StringLength(15)]
        public string Population { get; set; }

        public float MeterAvgPrice { get; set; }

        [StringLength(30)]
        public string Lat { get; set; }

        [StringLength(30)]
        public string Lng { get; set; }
        public string LocationUrl { get; set; }
        public string Content { get; set; }

        [StringLength(255)]
        public string SubTitle { get; set; }


        #region ForViewModel
        public int RealEstatesCount { get; set; }
        public int ProjectsCount { get; set; }
        public float MinimumRentPrice { get; set; }
        public float MaxRentPrice { get; set; }
        public bool IsAddedToFavorite { get; set; }
        public bool IsDeleted { get; set; }

        #endregion


    }
}
