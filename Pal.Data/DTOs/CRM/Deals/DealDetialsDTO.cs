using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Deals
{
    public class DealDetialsDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public string TypeString { get; set; }
        public string LeadSourceString { get; set; }
        public string StageName { get; set; }
        public int StageId { get; set; }
        public string Employee { get; set; }

        public string DealName { get; set; }

        public string NextStep { get; set; }

        public string Description { get; set; }

        public float Amount { get; set; }

        public DateTime? ClosingDate { get; set; }

        /// <summary>
        /// 0.5 %  
        /// </summary>
        public float SuccessProbability { get; set; }

        //-----------------------------------------Customer Info
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        //----------------------------------------
    }
}
