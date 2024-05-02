using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysRegion : BaseEntityNoIdentity<int>
    {
        public int CountryId { get; set; }

        public int CityId { get; set; }

        [StringLength(500)]
        public string? MainImg { get; set; }

        [StringLength(15)]
        public string? Population { get; set; }

        public string? FullAddress { get; set; }

        [StringLength(30)]
        public string? Lat { get; set; }

        [StringLength(30)]
        public string? Lng { get; set; }

        public string? LocationUrl { get; set; }

        //public bool IsFeatured { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsOnlyLookup { get; set; }

        //public int ViewsCount { get; set; } = 0;

        public virtual ICollection<SysNeighborhood> SysNeighborhoods { get; set; }
        public virtual ICollection<SysRegionTranslate> SysRegionTranslates { get; set; }
        //public virtual ICollection<SysRegionSite> SysRegionSites { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual SysCity SysCity { get; set; }
    }


    //------------------------------------------------------------------- 
    public class SysRegionTranslate : BaseEntityTranslate<int>
    {
        public int RegionId { get; set; }

        [StringLength(50), Required]
        public string RegionName { get; set; }


        [StringLength(255)]
        public string? SubTitle { get; set; }


        public string? Content { get; set; }


        [ForeignKey(nameof(RegionId))]
        public virtual SysRegion SysRegion { get; set; }
    }
}
