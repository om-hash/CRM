using Pal.Core.Domains.Customers;
using Pal.Core.Domains.Empolyees;
using Pal.Core.Domains.Lookups.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.Communications
{
    public class Call : BaseEntityWithLog<int>
    {
        public int EmployeeId { get; set; }

        /// <summary>
        /// Lead Or Customer
        /// </summary>
        public int CallToType { get; set; }


        public long CustomerId { get; set; }


        public int? DealId { get; set; }

        /// <summary>
        /// Social Media , Chat , Call , Messenger ...etc
        /// </summary>
        public int CallTypeId { get; set; }


        public int? CallStatusId { get; set; }


        public int? CallPurposeId { get; set; }

        public int? CallResultId { get; set; }

        public DateTime CallStart { get; set; }

        /// <summary>
        /// In Minutes
        /// </summary>
        public string Duration { get; set; }

        public string Subject { get; set; }

        public string CallDetails { get; set; }

        public bool IsScheduled { get; set; }

        #region Fathers
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }  

        [ForeignKey(nameof(CallTypeId))]
        public virtual SysCallType CallType { get; set; } 

        [ForeignKey(nameof(CallStatusId))]
        public virtual SysCallStatus CallStatus { get; set; }
        
        [ForeignKey(nameof(CallPurposeId))]
        public virtual SysCallPurpose CallPurpose { get; set; } 

        [ForeignKey(nameof(CallResultId))]
        public virtual SysCallResult CallResult { get; set; }
        #endregion
    }
}
