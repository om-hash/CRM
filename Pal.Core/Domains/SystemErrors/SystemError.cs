using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.SystemErrors
{
    public class SystemError : BaseEntity<Guid>
    {
        public DateTime LogDate { get; set; }

        [StringLength(50)]
        public string ActionName { get; set; }

        public string ExceptionMessage { get; set; }

        public Importance Importance { get; set; }

        public UserType? UserType { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }
    }
}
