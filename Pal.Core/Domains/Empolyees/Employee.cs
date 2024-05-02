using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Empolyees
{
    public class Employee : BaseEntityWithLog<int>
    {
        [Required]
        public string UserId { get; set; }

        public UserType UserType { get; set; }

        public int JobTitleId { get; set; }

        //[StringLength(500)]
        //public string MainImg { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        #region Fathers
        [ForeignKey(nameof(JobTitleId))]
        public virtual SysJobTitle JobTitle { get; set; }
        #endregion
    }
}
