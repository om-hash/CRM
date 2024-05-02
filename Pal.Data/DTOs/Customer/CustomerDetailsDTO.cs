using Pal.Data.DTOs.CRM.Call;
using Pal.Data.DTOs.CRM.Deals;
using Pal.Data.DTOs.CRM.Meeting;
using Pal.Data.DTOs.CRM.Task;
using Pal.Data.DTOs.Tour;
using Pal.Data.VMs.Tour;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Customer
{
    public class CustomerDetailsDTO
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
        public string LeadRatingName { get; set; }

        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string FollowedByEmpName { get; set; }
        public string FullAddress { get; set; }
        public string TwitterId { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public DateTime? CreationDate { get; set; }

        public bool RegisterdByAdmin { get; set; }
        public DateTime LastLogInDate { get; set; }

        public List<CustomerNoteDTO> Notes { get; set; }
        public List<TaskListDTO> Tasks { get; set; }
        public List<DoneCallDTO> Calls { get; set; }
        public List<MeetingListDTO> Meetings { get; set; }
        public List<DealListDTO> Deals { get; set; }
        public List<CustomerFavoriteViewModel> Favorites { get; set; }
        public List<TourListVM> Tours { get; set; }
    }
}
