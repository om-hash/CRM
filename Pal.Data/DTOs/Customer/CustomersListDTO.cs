using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Customer
{
    public class CustomersListDTO
    {
        public long Id { get; set; }

        public string FullName { get; set; }
        public string NationalityName { get; set; }
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public string CountryName { get; set; }
        public string CustomerStatusName { get; set; }
        public string WhatsappNumber { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsLead { get; set; }
        public string MobileNumber { get; set; }
        public string LeadSourceName { get; set; }
        public string WorkCompany { get; set; }
        public string LeadStatusName { get; set; }
        public string  LeadRatingName { get; set; }

        public string CityName { get; set; }
        public string RegionName { get; set; }
        public int FollowedByEmpId { get; set; }
        public string FollowedByEmpName { get; set; }
        public string FullAddress { get; set; }
        public string TwitterId { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }

        public string CreatedBy { get; set; }
        public string RegisterdByAdmin { get; set; }
        public string LastLogInDate { get; set; }
    }
}
