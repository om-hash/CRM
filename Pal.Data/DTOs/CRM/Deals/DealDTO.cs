using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Data.DTOs.CRM.Deals
{

    public class DealDTO
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public int LeadSourceId { get; set; }

        public long? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public int StageId { get; set; }

        public int EmployeeId { get; set; }

        public string DealName { get; set; }

        public string NextStep { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime? ClosingDate { get; set; }

        /// <summary>
        /// 0.5 %  
        /// </summary>
        public float SuccessProbability { get; set; }

    }

    public class DealListDTO
    {
        public int Id { get; set; }

      
        public string TypeString { get; set; }

        public string LeadSourceString { get; set; }

        public string Customer { get; set; }

        public long? CustomerId { get; set; }

        public string StageName { get; set; }

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

    }


}
