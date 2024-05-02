using Pal.Core.Domains.Attachments;
using Pal.Core.Enums.Attachment;
using Pal.Data.VMs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Tour
{
    public class TourDetailsListVM
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public string ReferenceTypeString { get; set; }
        public int ReferenceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public float Duration { get; set; }
        public float Price { get; set; }
        public float AvrageRating { get; set; }
        public int BathRoomsCount { get; set; }
        public string roomCount { get; set; }
        public float AreaTotal { get; set; }
        public List<Attachment> Images { get; set; }
        public ProfileSidebarViewModel ProfileSidebarViewModel { get; set; }
    }
}
