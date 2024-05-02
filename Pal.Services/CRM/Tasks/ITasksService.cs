using Pal.Core.Enums;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Task;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.CRM.Tasks
{
    public interface ITasksService
    {
        #region DTOs Methods

        public Task<SyncPaginatedListModel<TaskListDTO>> GetAllAsListAsync(DataManagerRequest dm, int? employeeId = null);

        public Task<int> AddTaskAsync(TaskCreateDTO model);

        public Task<int> UpdateTaskAsync(int id, TaskCreateDTO model);

        public Task<int> UpdateTaskAsync(int id, Pal.Core.Domains.Tasks.Task entity);

        public Task<TaskCreateDTO> GetTaskByIdAsync(int Id);

        public Task<TaskDetailsDTO> GetTaskDetailsByIdAsync(int Id, bool isSuperAdmin, int? employeeId = null);

        public Task<ResponseType> DeleteTaskAsync(int id);

        public Task<ResponseType> ChangeTaskStatus(int id, int statusId, bool isSuperAdmin, int? employeeId = null);
        Task<TaskSchedulerDTO> TasksSchedulerForEmployee();
        Task<IQueryable<TaskListDTO>> GetPaginatedTaskListForAdmin(int? priority,
            int? orderBy,
            int? employeeId,
            int? statusId,
            DateTime? startDate,
            DateTime? endDate);

        public Task<Pal.Core.Domains.Tasks.Task> GetTaskByReferenceNumber(int referenceNumber);

        public Task<int> GetTasksCount(int? empId);
        #endregion
    }
}
