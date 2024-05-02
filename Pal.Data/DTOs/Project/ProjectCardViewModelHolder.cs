using Pal.Data.DTOs.Advertisements;
using Pal.Data.VMs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Project
{
    public  class ProjectCardViewModelHolder
    {
        public List<AdvertisementsDTO> Advertisements { get; set; }
        public IQueryable<ProjectCardViewModel> ProejctItems { get; set; }
        public PaginatedList<ProjectCardViewModel> ProjectItemsForList { get; set; }

    }
}
