using Pal.Core.Domains.Customers;
using Pal.Core.Domains.Empolyees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Communications

{
    public class Meeting : BaseEntityWithLog<int>
    {
        public string Title { get; set; }

        public string Location { get; set; }

        public bool IsOnline { get; set; } = false;

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public int? EmployeeId { get; set; }

        public long? CustomerId { get; set; }

        public string? Description { get; set; }

        #region Fathers

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        #endregion
    }
}
