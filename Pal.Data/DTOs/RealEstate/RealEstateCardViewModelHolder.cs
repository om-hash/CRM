using Pal.Data.DTOs.Advertisements;
using Pal.Data.VMs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RealEstate
{
   public class RealEstateCardViewModelHolder
    {
        public List<AdvertisementsDTO> Advertisements { get; set; }
        public IQueryable<RealEstateCardViewModel> RealEstatesItems { get; set; }
        public PaginatedList<RealEstateCardViewModel> RealEstatesItemsForList { get; set; }
    }
}
