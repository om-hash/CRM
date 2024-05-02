using Pal.Core.Enums.Approvment;
using Pal.Data.VMs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Tour
{
    public class TourListVM
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public int TimeId { get; set; }
        public string Time { get; set; }

        public float TourDuration { get; set; }

        public string CustomerNotes { get; set; }

        public string AdvisorName { get; set; }

        public int? AdvisorId { get; set; }
        public string AdvisorPhone { get; set; }

        public string AdvisorLink { get; set; }
        public string CityName { get; set; }
        public string RejectionContent { get; set; }

             
        public string Barcode { get; set; }
        public ApproveStatment ApproveState { get; set; }
        public ProfileSidebarViewModel ProfileSidebarViewModel { get; set; }
        public List<TourDetailsListVM> TourDetails { get; set; }
}
}
