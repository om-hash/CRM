using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Customers
{
   public class CustomerSavedSearch: BaseEntity<int>
    {
        public long CustomerId { get; set; }
        public DateTime Date { get; set; }
        public bool? IsForSale { get; set; }
        public bool? IsForRent { get; set; }
        public int? RealestateTypeId { get; set; }
        public string CountryIds { get; set; }
        public string CityIds { get; set; }
        public string RegionIds { get; set; }
        public string neighborhoodIds { get; set; }
        public int? StatusId { get; set; }
        public string RoomsCountIds { get; set; }
        public string BathRoomsCountIds { get; set; }
        public string FeaturesIds { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public float? MinArea { get; set; }
        public float? MaxArea { get; set; }
        public bool IsDeleted { get; set; }
        public string AddressNamesAsString { get; set; }
        public string Address { get; set; }
    }
}
