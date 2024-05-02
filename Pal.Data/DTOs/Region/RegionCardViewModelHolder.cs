using Pal.Data.DTOs.Advertisements;
using Pal.Data.VMs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Region
{
   public class RegionCardViewModelHolder
    {
        public List<AdvertisementsDTO> Advertisements { get; set; }
        public IQueryable<RegionListDTO> RegionItems { get; set; }
        public PaginatedList<RegionListDTO> RegionItemsForList { get; set; }
    }
}
