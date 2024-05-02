using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Customer
{
   public class CustomerSavedSearchDTO
    {

        public int Id { get; set; }
        public long CustomerId { get; set; }
        public DateTime Date { get; set; }
        public bool? IsForSale { get; set; }
        public bool? IsForRent { get; set; }
        public int? RealEstateTypeId { get; set; }
        public List<int>? CountryId { get; set; }
        public List<int>? CityId { get; set; }
        public List<int>? RegionId { get; set; }
        public List<int>? neighborhoodId { get; set; }
        public int? StatusId { get; set; }
        public List<int>? RoomCount { get; set; } 
        public List<int>? BathRoomCount { get; set; }
        public List<int>? FeaturesList { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public float? MinArea { get; set; }
        public float? MaxArea { get; set; }
        public string AddressNamesAsString { get; set; }
        public string Address { get; set; }
 
    }
}
