using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Region
{
    public class MapPropertiesVM
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public double Price { get; set; }
        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }
        public string Link { get; set; }

    }
}
