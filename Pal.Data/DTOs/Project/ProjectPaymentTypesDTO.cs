using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Project
{
    public class ProjectPaymentTypesDTO
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int PaymentTypeId { get; set; }
        public string PaymentName { get; set; }
    }
}
