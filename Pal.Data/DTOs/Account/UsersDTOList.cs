using Pal.Core.Enums.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Account
{
    public class UsersDTOList
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string WhatsappNumber { get; set; }
        public UserType UserType { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}
