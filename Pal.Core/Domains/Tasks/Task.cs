using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pal.Core.Domains.Customers;
using Pal.Core.Domains.Empolyees;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Core.Enums.Task;


namespace Pal.Core.Domains.Tasks
{
    public class Task : BaseEntityWithLog<int>
    {

        /// <summary>
        /// لمين هي التاسك
        /// </summary>
        public int EmployeeId { get; set; }

        public string Subject { get; set; }

        public long? CustomerId { get; set; }

        public int? StatusId { get; set; }

        public TaskPriority TaskPriority { get; set; }

        /// <summary>
        /// ReferenceNumber
        /// رقم الريفرانس من حيث تم استيراد التاسك
        /// </summary>
        public int? ReferenceNumber { get; set; }
        /// <summary>
        /// ReferenceType
        /// نوع الريفرانس هل اجتماع ام اتفاق ام اتصال ام ... الخ - فهمت شلون
        /// </summary>
        public ReferenceType? ReferenceType { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #region Fathers


        [ForeignKey(nameof(StatusId))]
        public virtual SysTaskStatus TaskStatus { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
        #endregion
    }
}
