using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Activity_logs
{
    public class ActivityLog : BaseEntity<Guid>
    {
        public DateTime TransDate { get; set; }

        public ActionType ActionType { get; set; }
        [StringLength(50)]
        public string? ActionName { get; set; }


        public LogTransType TransType { get; set; }

        [StringLength(25)]
        public string TransReferenceId { get; set; }

        public UserType UserType { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(25)]
        public string UserTypeReferenceId { get; set; }

        public bool IsCustomer { get; set; }
        [StringLength(50)]
        public string? Country { get; set; }
        [StringLength(50)]
        public string? City { get; set; }
        [StringLength(50)]
        public string? UserName { get; set; }
        [StringLength(25)]
        public string? UserPhone { get; set; }
        [StringLength(100)]
        public string? UserEmail { get; set; }
        public string? OldData { get; set; }
        public string? NewData { get; set; }
        public int RelatedTo { get; set; }


    }
}
