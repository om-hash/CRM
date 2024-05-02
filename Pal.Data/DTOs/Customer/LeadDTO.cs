using Pal.Core.Enums.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Customer
{
    public class LeadDTO
    {
        public CustomerStatus CustomerStatus { get; set; }
        public int NationalityId { get; set; }
        public int CountryId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string WhatsappNumber { get; set; }
        public bool IsLead { get; set; }

        [StringLength(20)]
        public string MobileNumber { get; set; }

        public int? LeadSourceId { get; set; }

        public int? IndustryId { get; set; }

        [StringLength(50)]
        public string WorkCompany { get; set; }

        public int? LeadStatusId { get; set; }

        public int? LeadRatingId { get; set; }

        public int? CityId { get; set; }

        public int? RegionId { get; set; }
        public int CreatedByUserId { get; set; }
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string TwitterId { get; set; }

        [StringLength(100)]
        public string Facebook { get; set; }

        [StringLength(100)]
        public string Instagram { get; set; }
    }
}
