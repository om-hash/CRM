using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.RealEstate
{
    public class ProjectFromCategoryVM
    {
        public int Count { get; set; }

        public int CategoryId { get; set; }
       

        public IQueryable<ProjectCardViewModel> ProjectList { get; set; }
        public List<ProjectCardViewModel> projectListView { get; set; }
    }
}
