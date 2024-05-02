using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using Pal.Data.VMs.Pagination;

namespace Pal.Data.VMs.Sales
{
    public class SalesViewModel
    {
        public int Id { get; set; }
        public int? SalesCategoryId { get; set; }
        public string SalesCategoryName { get; set; }
        public string SalesName { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string MainImg { get; set; }
        public string PhoneNumber { get; set; }
        public string WhatsApp { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public float AvrageRating { get; set; } = 0;

        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string TwitterURL { get; set; }
        public string linkedinURL { get; set; }
        public string Content { get; set; }

        public PaginatedList<RealEstateCardViewModel> RealEstates { get; set; }
        public PaginatedList<ProjectCardViewModel> Projects { get; set; }
    }
}
