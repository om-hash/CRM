using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Account;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Lookups;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Attachment;
using Pal.Core.Enums.Notifications;
using Pal.Core.Enums.Roles;
using Pal.Core.Enums.Task;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Task;
using Pal.Data.DTOs.Employees;
using Pal.Data.DTOs.Notifications;
using Pal.Services.DataServices.Employees;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.WebWorkContext;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReferenceType = Pal.Core.Enums.Attachment.ReferenceType;

namespace Pal.Services.CRM.Tasks
{
    public class TaskService : ITasksService
    {
        #region Fields and CTOR
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IWebWorkContext _workContext;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeesService _employeesService;
        private readonly ILanguageService _languageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskService(ApplicationDbContext context,
              UserManager<ApplicationUser> userManager, IMapper mapper, ILanguageService languageService,
            IHttpContextAccessor httpContextAccessor, INotificationService notificationService, ILoggerService logger, IEmployeesService employeesService, IWebWorkContext webWork)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _workContext = webWork;
            _userManager = userManager;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _employeesService = employeesService;
            _languageService = languageService;
        }


        #endregion
        #region DTOs Methods


        public async Task<IQueryable<TaskListDTO>> GetPaginatedTaskListForAdmin(int? priority,
            int? orderBy,
            int? employeeId,
            int? statusId,
            DateTime? startDate,
            DateTime? endDate)
        {
            var langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                


                var query = _context.Tasks.Where(a => !a.IsDeleted);
                switch (orderBy)
                {
                    case 0:
                        query = query.OrderBy(t => t.StartDate);
                        break;
                    case 1:
                        query = query.OrderBy(t => t.EndDate);
                        break;
                    case 2:
                        query = query.OrderByDescending(t => t.TaskPriority);
                        break;
                }
                if (priority != null)
                    query = query.Where(a => a.TaskPriority == (TaskPriority)priority);

                if (employeeId != null)
                    query = query.Where(p => p.EmployeeId == employeeId);

                if (statusId != null)
                    query = query.Where(a => a.StatusId == statusId);
                
                if (startDate != null)
                    query = query.Where(a => a.StartDate >= startDate);
                

                if (endDate != null)
                    query = query.Where(a => a.EndDate <= endDate);

                

                //var data = await _context.Tours.Where(a => !a.IsTemporary).Include(a => a.Customer).ToListAsync();
                var result = query.Select(t => new TaskListDTO
                {
                    Id = t.Id,
                    CreatedBy = t.CreatedBy,
                    EmployeeId = t.EmployeeId,
                    EmployeeName = t.Employee.FullName,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    CustomerId = t.CustomerId,
                    Status = t.TaskStatus.Translates.FirstOrDefault(t => t.LanguageId == langId).StatusName,
                    Subject = t.Subject,
                    TaskPriority = t.TaskPriority

                }).AsQueryable().AsNoTracking();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetPaginatedTaskListForAdmin), ex);
                return null;
            }
        }
        public async Task<SyncPaginatedListModel<TaskListDTO>> GetAllAsListAsync(DataManagerRequest dm, int? employeeId = null)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                var query = _context.Tasks.Where(p => !p.IsDeleted).AsQueryable().AsNoTracking()
                    .Select(t => new TaskListDTO
                    {
                        Id = t.Id,
                        CreatedBy = t.CreatedBy,
                        EmployeeId = t.EmployeeId,
                        EmployeeName = t.Employee.FullName,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        CustomerId = t.CustomerId,
                        Status = t.TaskStatus.Translates.FirstOrDefault(t => t.LanguageId == langId).StatusName,
                        StatusId = t.StatusId,
                        Subject = t.Subject,
                        TaskPriority = t.TaskPriority
                    });

                if (employeeId != null)
                    query = query.Where(p => p.EmployeeId == employeeId);

                var data = await SyncGridOperations<TaskListDTO>.PagingAndFilterAsync(query, dm);

                data.Data.ForEach(a =>
                {
                    a.CreatedByName = _context.Users.FirstOrDefault(u => u.Id == a.CreatedBy)?.FullName ?? "";
                });


                return data;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAllAsListAsync), ex);
                return null;
            }
        }
        public async Task<TaskDetailsDTO> GetTaskDetailsByIdAsync(int id, bool isSuperAdmin, int? employeeId = null)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                var entity = await _context.Tasks.Include(t => t.Employee).Include(t => t.Customer).FirstOrDefaultAsync(a => a.Id == id);

                if (entity == null)
                    return null;

                if (!(isSuperAdmin || entity.EmployeeId == employeeId))
                    return null;

                TaskDetailsDTO detailsModel = new TaskDetailsDTO
                {
                    Id = entity.Id,
                    Subject = entity.Subject,
                    StatusId = entity.StatusId,
                    EmployeeName = entity.Employee.FullName,
                    CustomerName = entity.Customer?.FullName,
                    CreatedByName = _context.Users.FirstOrDefault(u => u.Id == entity.CreatedBy)?.FullName ?? "",
                    TaskPriority = char.ToLower(entity.TaskPriority.ToString()[0]) + entity.TaskPriority.ToString().Substring(1),
                    EndDate = entity.EndDate,
                    StartDate = entity.StartDate,
                    Description = entity.Description,
                };


                detailsModel.Attachments = await _context.Attachments.Where(a => a.ReferenceType == ReferenceType.Tasks && a.ReferenceId == detailsModel.Id.ToString()).ToListAsync();
                return detailsModel;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetTaskDetailsByIdAsync), ex);
                return null;
            }
        }
        public async Task<int> AddTaskAsync(TaskCreateDTO model)
        {


            try
            {
                var entity = _mapper.Map<Pal.Core.Domains.Tasks.Task>(model);

                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                #region Sending notification
                if (entity.Id > 0)
                {
                    //var paramse = new List<NotificationParamDTO>()
                    //{
                    //    new NotificationParamDTO(entity.Subject),
                    //    new NotificationParamDTO(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value),
                    //};
                    await _notificationService.SendNotificationAsync(new NotificationDTO
                    {
                        //NotificationParams = paramse,
                        GroupId = UserType.Admins.ToString() + entity.EmployeeId,
                        NotificationTypeId = (int)NotificationTypes.ToAdmin_YouHaveANewTask,
                        NotificationFor = UserType.Admins,
                        Url = "/Admin/Tasks/TaskDetails/" + entity.Id.ToString()
                    });
                }
                #endregion

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(AddTaskAsync), ActionType = ActionType.Add, TransReferenceId = entity.Id.ToString(), TransType = LogTransType.Tasks });

                return entity.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(AddTaskAsync), ex);
                return 0;
            }


        }
        public async Task<int> UpdateTaskAsync(int id, TaskCreateDTO model)
        {
            try
            {
                Pal.Core.Domains.Tasks.Task entity = _mapper.Map<Pal.Core.Domains.Tasks.Task>(model);

                _context.Update(entity);
                await _context.SaveChangesAsync();

                #region Sending notification
                if (entity.Id > 0)
                {
                    await _notificationService.SendNotificationAsync(new NotificationDTO
                    {
                        GroupId = UserType.Admins.ToString() + entity.EmployeeId,
                        NotificationTypeId = (int)NotificationTypes.ToAdmin_YouHaveAnUpdatedTask,
                        NotificationFor = UserType.Admins,
                        Url = "/Admin/Tasks/TaskDetails/" + entity.Id.ToString()
                    });
                }
                #endregion

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateTaskAsync), ActionType = ActionType.Update, TransReferenceId = entity.Id.ToString(), TransType = LogTransType.Tasks });

                return entity.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateTaskAsync), ex);
                return 0;
            }
        }

        public async Task<TaskCreateDTO> GetTaskByIdAsync(int id)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var entity = await _context.Tasks.FirstOrDefaultAsync(a => a.Id == id);

                if (entity == null)
                    return null;

                var model = _mapper.Map<TaskCreateDTO>(entity);
                if (model.CustomerId == null || model.CustomerId <= 0)
                {
                    model.CustomerName = "-";
                }
                else
                {
                    model.CustomerName = _context.Customers.FirstOrDefault(x => x.Id == model.CustomerId).FullName;
                }
                model.EmployeeName = (await _context.Employees.FirstOrDefaultAsync(x => x.Id == model.EmployeeId)).FullName;
                model.StatusName = (await _context.SysTaskStatusTranslates.FirstOrDefaultAsync(x => x.TaskStatusId == model.StatusId)).StatusName;
              
                var languages = _languageService.GetAllLanguages();


                model.Attachments = await _context.Attachments.Where(a => a.ReferenceType == ReferenceType.Tasks && a.ReferenceId == model.Id.ToString()).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetTaskByIdAsync), ex);

                return null;
            }
        }
        public async Task<ResponseType> DeleteTaskAsync(int id)
        {
            try
            {
                var entity = await _context.Tasks.FirstOrDefaultAsync(a => a.Id == id);


                entity.IsDeleted = true;

                _context.Tasks.Update(entity);
                await _context.SaveChangesAsync();

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteTaskAsync), ActionType = ActionType.Delete, TransReferenceId = entity.Id.ToString(), TransType = LogTransType.Tasks });
                return ResponseType.Success;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteTaskAsync), ex);
                return ResponseType.Error;
            }
        }

        public async Task<ResponseType> ChangeTaskStatus(int id, int statusId, bool isSuperAdmin, int? employeeId = null)
        {
            try
            {
                var entity = await _context.Tasks.FirstOrDefaultAsync(a => a.Id == id);
                if (!(isSuperAdmin || entity.EmployeeId == employeeId))
                    return ResponseType.Error;
                entity.StatusId = statusId;

                _context.Update(entity);
                await _context.SaveChangesAsync();

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(ChangeTaskStatus), ActionType = ActionType.Update, TransReferenceId = entity.Id.ToString(), TransType = LogTransType.Tasks });
                return ResponseType.Success;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(ChangeTaskStatus), ex);
                return ResponseType.Error;
            }
        }
        //-------------------------------------------------------------------------
        public async Task<TaskSchedulerDTO> TasksSchedulerForEmployee()
        {
            try
            {
                var userId = _workContext.GetMyUserId();

                var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
                var isInRole = await _userManager.IsInRoleAsync(user, PalRoles.Task_List.ToString());
                isInRole = await _userManager.IsInRoleAsync(user, PalRoles.SuperAdmin.ToString());

                var employeeId = await _workContext.GetEmployeeIdByUserId(userId);

                var result = ((await GetAllAsListAsync(null))).Data.Where(x => x.EmployeeId == employeeId).Select(a => new TaskSchedulerItemsDTO
                {
                    CreatedBy = a.CreatedBy,
                    CreatedByName = a.CreatedByName,
                    CustomerId = a.CustomerId,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.EmployeeName,
                    EndTime = a.EndDate,
                    Id = a.Id,
                    IsAllDay = a.IsAllDay,
                    StartTime = a.StartDate,
                    Status = a.Status,
                    Subject = a.Subject,
                    TaskPriority = a.TaskPriority,

                }).ToList();
                var response = new TaskSchedulerDTO
                {
                    IsInRole = isInRole,
                    TaskScheduler = result,
                };


                return response;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(ChangeTaskStatus), ex);
                return null;
            }
        }


        public async Task<Pal.Core.Domains.Tasks.Task> GetTaskByReferenceNumber(int referenceNumber)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                var entity = await _context.Tasks.FirstOrDefaultAsync(a => a.ReferenceNumber == referenceNumber);

                if (entity == null)
                    return null;

                return entity;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetTaskByIdAsync), ex);

                return null;
            }
        }

        public async Task<int> UpdateTaskAsync(int id, Pal.Core.Domains.Tasks.Task entity)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();

                //#region Sending notification
                //if (entity.Id > 0)
                //{
                //    await _notificationService.SendNotificationAsync(new NotificationDTO
                //    {
                //        GroupId = UserType.Admins.ToString() + entity.EmployeeId,
                //        NotificationTypeId = (int)NotificationTypes.ToAdmin_YouHaveAnUpdatedTask,
                //        NotificationFor = UserType.Admins,
                //        Url = "/Admin/Tasks/TaskDetails/" + entity.Id.ToString()
                //    });
                //}
                //#endregion

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateTaskAsync), ActionType = ActionType.Update, TransReferenceId = entity.Id.ToString(), TransType = LogTransType.Tasks });

                return entity.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateTaskAsync), ex);
                return 0;
            }
        }

        public async Task<int> GetTasksCount(int? empId)
        {
            try
            {
                var query = _context.Tasks.AsQueryable();
                if (empId != null)
                    query = query.Where(t => t.EmployeeId == empId);

                int count = await _context.Tasks.CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateTaskAsync), ex);
                return -1;
            }
        }

        #endregion
    }
}
