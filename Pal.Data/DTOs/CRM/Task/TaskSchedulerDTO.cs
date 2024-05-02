using Pal.Core.Enums.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Task
{
   public class TaskSchedulerDTO
    {
        public bool IsInRole { get; set; }
        public List<TaskSchedulerItemsDTO> TaskScheduler{ get; set; }
    }
}
