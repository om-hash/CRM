using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Lookups
{
    public class SysDay
    {
        public SysDay(int id, string dayName)
        {
            Id = id;
            DayName = dayName;
        }
        public int Id { get; set; }

        [StringLength(10)]
        public string Shortcut { get; set; }

        [StringLength(25)]
        public string DayName { get; set; }
    }
}
