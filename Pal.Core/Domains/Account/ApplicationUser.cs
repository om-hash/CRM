using Microsoft.AspNetCore.Identity;
using Pal.Core.Enums.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Account
{
    public class ApplicationUser : IdentityUser
    {
        

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [ StringLength(20)]
        public string WhatsappNumber { get; set; }


        [ StringLength(20)]
        public string PhoneNumber2 { get; set; } = string.Empty;

        /// <summary>
        /// CompanyId or CustomerId or AdvisorId  etc...
        /// </summary>
        public string ReferenceId { get; set; } = string.Empty;


        public UserType UserType { get; set; }

        public bool IsDeleted { get; set; }

        public bool RegisterdByAdmin { get; set; } = true;
        public DateTime LastLogInDate { get; set; }

        public string FCMToken { get; set; } = "";

        //[StringLength(500)]
        //public string MainImg { get; set; }
    }
}
