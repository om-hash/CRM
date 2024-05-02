using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Employees
{
    public class EmployeesList
    {
        public int Id { get; set; }

        public string JobTitle { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Color { get; set; }
    }
}
