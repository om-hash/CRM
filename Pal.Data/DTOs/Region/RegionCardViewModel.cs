using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Region
{
    public class RegionCardViewModel
    {
        public int Id { get; set; }
        public string RegionName { get; set; }
        public string MainImg { get; set; }
        public string SubTitle { get; set; }
        public string LocationUrl { get; set; }

    }
}
