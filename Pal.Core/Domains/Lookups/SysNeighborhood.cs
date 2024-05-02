//using Pal.Core.Domains.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysNeighborhood: BaseEntityNoIdentity<int>
    {
        public int RegionId { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual SysRegion SysRegion { get; set; }

        public virtual ICollection<SysNeighborhoodTranslate> SysNeighborhoodTranslates { get; set; }
        //public virtual ICollection<Project> Projects { get; set; }
    }

    public class SysNeighborhoodTranslate : BaseEntityTranslate<int>
    {
        public int NeighborhoodId { get; set; }

        [Required, StringLength(500)]
        public string NeighborhoodName { get; set; }



        [ForeignKey(nameof(NeighborhoodId))] 
        public virtual SysNeighborhood SysNeighborhood { get; set; }
    }

}
