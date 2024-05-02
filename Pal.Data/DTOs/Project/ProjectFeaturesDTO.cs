using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Project
{
    public class ProjectFeaturesDTO
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public string Name { get; set; }

        public int FeatureId { get; set; }

    }
}
