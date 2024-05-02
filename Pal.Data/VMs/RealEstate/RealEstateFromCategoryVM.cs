using Pal.Data.DTOs.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.RealEstate
{
    public class RealEstateFromCategoryVM
    {
        public int Count { get; set; }

        public int CategoryId { get; set; }
       

        public IQueryable<RealEstateCardViewModel> RealEstateList { get; set; }
        public List<RealEstateCardViewModel> RealEstateListView { get; set; }
    }
}
