using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Statistics
{
    public class LatestPropertiesDTO
    {
        public string GridId { get; set; }
        public string Title { get; set; }
        public List<LatestPropertiesListDTO> dataSource { get; set; }
    }
}
