using Pal.Core.Enums.Account;
using Pal.Core.Enums.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Employees
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public UserType UserType { get; set; }

        [Required]
        public string UserId { get; set; }

        public int JobTitleId { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [NotMapped]
        public string GCols { get; set; }
    }
}
