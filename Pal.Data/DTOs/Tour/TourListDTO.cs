using Pal.Core.Enums.Approvment;
using System;

namespace Pal.Data.DTOs.Tour
{
    public class TourListDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }
        public long CustomerId { get; set; }

        public string Time { get; set; }

        public int TimeId { get; set; }

        public float TourDuration { get; set; }

        public string CustomerNotes { get; set; }
        public string AdvisorName { get; set; }
        public string RegionName { get; set; }

        public int CityId { get; set; }
        //public bool ApproveState { get; set; }
        public ApproveStatment ApproveState { get; set; }

        public string RejectionContent { get; set; }

        public int AdvisorId { get; set; }

        public string Barcode { get; set; }



    }
}
