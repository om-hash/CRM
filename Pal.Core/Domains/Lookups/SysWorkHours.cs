using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pal.Core.Domains.Lookups
{
    public class SysWorkHours
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }


        /// <summary>
        /// like 09:00 AM 
        /// </summary>
        [Required, StringLength(10)]
        public string TimeName { get; set; }

        public static List<SysWorkHours> DefaultWorkHours
        {
            get
            {
                return new List<SysWorkHours>
                {
                    new SysWorkHours{ Id = 1 , TimeName = "01:00 AM"},
                    new SysWorkHours{ Id = 2 , TimeName = "02:00 AM"},
                    new SysWorkHours{ Id = 3 , TimeName = "03:00 AM"},
                    new SysWorkHours{ Id = 4 , TimeName = "04:00 AM"},
                    new SysWorkHours{ Id = 5 , TimeName = "05:00 AM"},
                    new SysWorkHours{ Id = 6 , TimeName = "06:00 AM"},
                    new SysWorkHours{ Id = 7 , TimeName = "07:00 AM"},
                    new SysWorkHours{ Id = 8 , TimeName = "08:00 AM"},
                    new SysWorkHours{ Id = 9 , TimeName = "09:00 AM"},
                    new SysWorkHours{ Id = 10, TimeName = "10:00 AM"},
                    new SysWorkHours{ Id = 11, TimeName = "11:00 AM"},
                    new SysWorkHours{ Id = 12, TimeName = "12:00 PM"},
                    new SysWorkHours{ Id = 13, TimeName = "01:00 PM"},
                    new SysWorkHours{ Id = 14, TimeName = "02:00 PM"},
                    new SysWorkHours{ Id = 15, TimeName = "03:00 PM"},
                    new SysWorkHours{ Id = 16, TimeName = "04:00 PM"},
                    new SysWorkHours{ Id = 17, TimeName = "05:00 PM"},
                    new SysWorkHours{ Id = 18, TimeName = "06:00 PM"},
                    new SysWorkHours{ Id = 19, TimeName = "07:00 PM"},
                    new SysWorkHours{ Id = 20, TimeName = "08:00 PM"},
                    new SysWorkHours{ Id = 21, TimeName = "09:00 PM"},
                    new SysWorkHours{ Id = 22, TimeName = "10:00 PM"},
                    new SysWorkHours{ Id = 23, TimeName = "11:00 PM"},
                    new SysWorkHours{ Id = 0 , TimeName = "00:00 AM"},
                };
            }
        }

  

    }


}
