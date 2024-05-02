using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Communications;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Notifications;
using Pal.Core.Enums.Task;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Call;
using Pal.Data.DTOs.Notifications;
using Pal.Services.FileManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.WebWorkContext;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pal.Services.CRM.Calls
{
    public class CallSerivce : RepositoryBase<Call>, ICallService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly IFileManagerService _fileManager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;
        private readonly ILanguageService _languageService;

        public CallSerivce(ApplicationDbContext context, IWebWorkContext workContext, IFileManagerService fileManager, IMapper mapper,
          ILoggerService logger, ILanguageService languageService, IHttpContextAccessor httpContextAccessor, INotificationService notificationService) : base(context)
        {
            _context = context;
            _workContext = workContext;
            _fileManager = fileManager;
            _mapper = mapper;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
         
            _languageService = languageService;
        }
        public async Task<SyncPaginatedListModel<CallListDTO>> GetAllAsync(DataManagerRequest dm)
        {
            try
            {
                var model = new CallListDTO();
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var query = FindAll()
                    .Where(a => !a.IsDeleted)
                    .Where(x => x.CallStart <= DateTime.Now.AddHours(3))
                    .Select(x => new DoneCallDTO
                     {
                        CallPurposeName = x.CallPurpose.Translates.FirstOrDefault(a => a.LanguageId == langId).CallPurposeName,
                        CallResultName = x.CallResult.Translates.FirstOrDefault(a => a.LanguageId == langId).CallResultName,
                        CallStatusName = x.CallStatus.Translates.FirstOrDefault(a => a.LanguageId == langId).CallStatusName,
                        CallTypeName = x.CallType.Translates.FirstOrDefault(a => a.LanguageId == langId).CallTypeName,
                        DealName = _context.Deals.FirstOrDefault(a => a.Id == x.DealId).DealName,
                        EmployeeName = x.Employee.FullName,
                        CustomerName = x.Customer.FullName,
                        CallDetails = x.CallDetails,
                        CallStart = x.CallStart.ToString("MM/dd/yyyy h:mm tt"),
                        Id = x.Id,
                        Duration = x.Duration,
                        Subject = x.Subject,
                        CustomerId = x.CustomerId,
                    });

                model.DoneCalls.AddRange((await SyncGridOperations<DoneCallDTO>.PagingAndFilterAsync(query, dm)).Data);



                var query2 = FindAll()
                   .Where(a => !a.IsDeleted)
                   .Where(x => x.CallStart >= DateTime.Now.AddHours(3))
                   .Select(x => new ScheduledCallsDTO
                   {

                       CallPurposeName = x.CallPurpose.Translates.FirstOrDefault(a => a.LanguageId == langId).CallPurposeName,
                       CallResultName = x.CallResult.Translates.FirstOrDefault(a => a.LanguageId == langId).CallResultName,
                       CallStatusName = x.CallStatus.Translates.FirstOrDefault(a => a.LanguageId == langId).CallStatusName,
                       CallTypeName = x.CallType.Translates.FirstOrDefault(a => a.LanguageId == langId).CallTypeName,
                       DealName = _context.Deals.FirstOrDefault(a => a.Id == x.DealId).DealName,
                       EmployeeName = x.Employee.FullName,
                       CustomerName = x.Customer.FullName,
                       CallDetails = x.CallDetails,
                       CallStart = x.CallStart.ToString("MM/dd/yyyy h:mm tt"),
                       Id = x.Id,
                       Duration = x.Duration,
                       Subject = x.Subject,
                   });

                model.ScheduledCalls.AddRange((await SyncGridOperations<ScheduledCallsDTO>.PagingAndFilterAsync(query2, dm)).Data);

                return new SyncPaginatedListModel<CallListDTO>(model, dm.Take);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAllAsync), ex);
                return null;
            }
        }


        //-----------------------------------------------------------------------------------
        public async Task<int> AddAsync(CallDTO model)
        {
            try
            {
                var call = _mapper.Map<Call>(model);
                _context.Add(call);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(model: new ActivityLog { ActionName = nameof(AddAsync), ActionType = ActionType.Add, TransReferenceId = call.Id.ToString(), TransType = LogTransType.Call, RelatedTo = Convert.ToInt32(call.CustomerId) });

                //AssignedToTask
                if (model.IsAssignedToTask)
                {
                    var task = new Pal.Core.Domains.Tasks.Task()
                    {
                        EmployeeId = model.EmployeeId,
                        CustomerId = model.CustomerId,
                        Subject = model.Subject,
                        TaskPriority = Core.Enums.Task.TaskPriority.Normal,
                        StartDate = model.CallStart,
                        StatusId = _context.SysTaskStatus.FirstOrDefault().Id,
                        EndDate = model.CallStart.AddHours(1),
                    };
                    _context.Tasks.Add(task);
                    await _context.SaveChangesAsync();
                    await _logger.LogInfoAsync(model: new ActivityLog { ActionName = nameof(AddAsync), ActionType = ActionType.Add, TransReferenceId = task.Id.ToString(), TransType = LogTransType.Tasks });
                    #region Sending notification
                    if (task.Id > 0)
                    {
                        await _notificationService.SendNotificationAsync(new NotificationDTO
                        {
                            GroupId = UserType.Admins.ToString() + model.EmployeeId,
                            NotificationTypeId = (int)NotificationTypes.ToAdmin_YouHaveAnUpdatedTask,
                            NotificationFor = UserType.Admins,
                            Url = "/Admin/Tasks/GetTaskById/" + task.Id.ToString()
                        });
                    }
                    #endregion
                }
                //AssignedToTask
                return call.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(AddAsync), ex);
                return 0;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<ResponseType> DeleteAsync(int id)
        {
            using var transaction = _context.BeginTransaction();

            try
            {
                var call = await _context.Calls.FirstOrDefaultAsync(a => a.Id == id);
                call.IsDeleted = true;
                _context.Calls.Update(call);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteAsync), ActionType = ActionType.Delete, TransReferenceId = call.Id.ToString(), TransType = LogTransType.Call });
                return ResponseType.Success;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteAsync), ex);
                await transaction.RollbackAsync();
                return ResponseType.Error;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<CallDTO> GetCallByIdAsync(int id)
        {
            try
            {

                var call = await _context.Calls.FirstOrDefaultAsync(a => a.Id == id);

                if (call == null)
                    return null;

                var model = _mapper.Map<CallDTO>(call);
                model.CustomerName = _context.Customers.FirstOrDefault(x => x.Id == model.CustomerId).FullName;
                model.IsAssignedToTask = _context.Tasks.FirstOrDefault(x => x.ReferenceNumber == id && x.ReferenceType == ReferenceType.Call) == null ? false : true;

                var languages = _languageService.GetAllLanguages();
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCallByIdAsync), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<int> UpdateAsync(CallDTO model)
        {
            try
            {
                var call = _mapper.Map<Call>(model);
                _context.Update(call);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateAsync), ActionType = ActionType.Update, TransReferenceId = call.Id.ToString(), TransType = LogTransType.Call });

                //AssignedToTask
                if (model.IsAssignedToTask)
                {
                    var task = new Pal.Core.Domains.Tasks.Task()
                    {
                        EmployeeId = model.EmployeeId,
                        CustomerId = model.CustomerId,
                        Subject = model.Subject,
                        TaskPriority = Core.Enums.Task.TaskPriority.Normal,
                        StartDate = model.CallStart,
                        StatusId = _context.SysTaskStatus.FirstOrDefault().Id,
                        EndDate = model.CallStart.AddHours(1),
                        ReferenceType = ReferenceType.Call,
                        ReferenceNumber = call.Id
                    };
                    _context.Tasks.Add(task);
                    await _context.SaveChangesAsync();

                    #region Sending notification
                    if (task.Id > 0)
                    {
                        await _notificationService.SendNotificationAsync(new NotificationDTO
                        {
                            GroupId = UserType.Admins.ToString() + model.EmployeeId,
                            NotificationTypeId = (int)NotificationTypes.ToAdmin_YouHaveANewTask,
                            NotificationFor = UserType.Admins,
                            Url = "/Admin/Tasks/GetTaskById/" + task.Id.ToString()
                        });
                    }
                    #endregion
                }
                //AssignedToTask

                return call.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateAsync), ex);
                return 0;
            }
        }
        //----------------------------------------------------------------------------------------
    }
}
