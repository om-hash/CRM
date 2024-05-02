using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RealEstate
{
    public class RealEstatePaymentTypesDTO
    {
        public int Id { get; set; }

        public int RealEstateId { get; set; }

        public int PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }


    }
}
