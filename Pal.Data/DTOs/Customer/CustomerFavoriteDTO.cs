using Pal.Core.Enums.Customer;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Customer
{
    public class CustomerFavoriteDTO
    {
        public Guid Id { get; set; }

        public long CustomerId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public CustomerFavoriteType FavoriteType { get; set; }

        public int FavoriteReference { get; set; }

        [StringLength(500)]
        public string Link { get; set; }
    }
}