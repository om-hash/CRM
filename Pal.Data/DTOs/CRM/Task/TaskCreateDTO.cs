using Pal.Core.Domains.Attachments;
using Pal.Core.Domains.Empolyees;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Core.Enums.Task;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Task
{
    public class TaskCreateDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// لمين هي التاسك
        /// </summary>
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        public string Subject { get; set; }

        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        /// <summary>
        /// Task Creator Id (User Id)
        /// </summary>
        public string? CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public TaskPriority? TaskPriority { get; set; }

        /// <summary>
        /// احتمال نحتاجها
        /// </summary>
        public string? ReferenceNumber { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #region Children
        public List<Attachment>? Attachments { get; set; }
        #endregion
    }
}
