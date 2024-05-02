using Pal.Core.Domains.Empolyees;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Customers
{
    public class CustomerNotes : BaseEntity<long>
    {
        public long CustomerId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int? AddedByEmpId { get; set; }

        // [StringLength(5000)]
        public string Content { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(AddedByEmpId))]
        public virtual Employee Employee { get; set; }
    }
}
