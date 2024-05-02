using Pal.Core.Enums.Account;
using Pal.Core.Enums.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Account
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string WhatsappNumber { get; set; }
        public UserType UserType { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }

        public bool IsEmailAutoActivation { get; set; }
        public bool IsPhoneNumberAutoActivation { get; set; }

        public List<RoleDTO> Roles { get; set; }

        public bool IsAddEmployee { get; set; }

        public int JobTitleId { get; set; }
        public string JobTitle { get; set; }
    }
}
