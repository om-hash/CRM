using Pal.Core.Enums.Customer;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Customers
{

    public class CustomerFavorite : BaseEntity<Guid>
    {
        public long CustomerId { get; set; }

        public DateTime Date { get; set; }

        public CustomerFavoriteType FavoriteType { get; set; }

        public int FavoriteReference { get; set; }

        [StringLength(500)]
        public string Link { get; set; }


        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
    }
}