using Pal.Core.Enums.Attachment;
using Pal.Core.Enums.Customer;
using Pal.Data.VMs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Customer
{
    public class CustomerFavoriteViewModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string Link { get; set; }
        public string AdminDashboardLink { get; set; }
        public float Price { get; set; }
        public bool IsAddedToTour { get; set; }
        public CustomerFavoriteType ItemType { get; set; }
        public ReferenceType ReferenceType { get; set; }

        public ProfileSidebarViewModel ProfileSidebarViewModel { get; set; }
    }
}
