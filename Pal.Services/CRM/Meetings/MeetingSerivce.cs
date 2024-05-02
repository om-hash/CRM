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
using Pal.Data.DTOs.CRM.Meeting;
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

namespace Pal.Services.CRM.Meetings
{
    public class MeetingSerivce : RepositoryBase<Meeting>, IMeetingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly IFileManagerService _fileManager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private readonly INotificationService _notificationService;
        private readonly ILanguageService _languageService;

        public MeetingSerivce(ApplicationDbContext context, IWebWorkContext workContext, ILanguageService languageService,
          IFileManagerService fileManager, IMapper mapper,
          ILoggerService logger, IHttpContextAccessor httpContextAccessor, INotificationService notificationService) : base(context)
        {
            _context = context;
            _workContext = workContext;
            _fileManager = fileManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            
            _notificationService = notificationService;
            _languageService = languageService;
        }
        public async Task<SyncPaginatedListModel<MeetingListDTO>> GetAllAsync(DataManagerRequest dm)
        {

            try
            {
                var query = FindAll()
                    .Where(x => !x.IsDeleted)
                    .Select(x => new MeetingListDTO
                    {
                        Id = x.Id,
                        CustomerName = x.Customer.FullName,
                        Description = x.Description,
                        EmployeeName = x.Employee.FullName,
                        FromDateTime = x.From == null ? "" : x.From.ToString("MM/dd/yyyy h:mm tt"),
                        ToDateTime = Convert.ToDateTime(x.To).ToString("MM/dd/yyyy h:mm tt"),
                        IsOnline = x.IsOnline,
                        Location = x.Location,
                        Title = x.Title,
                        CustomerId = x.CustomerId,
                    });

                return await SyncGridOperations<MeetingListDTO>.PagingAndFilterAsync(query, dm);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAllAsync), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<int> AddAsync(MeetingDTO model)
        {
            try
            {
                var Meeting = _mapper.Map<Meeting>(model);
                _context.Add(Meeting);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(model: new ActivityLog { ActionName = nameof(AddAsync), ActionType = ActionType.Add, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Meeting, RelatedTo = Convert.ToInt32(Meeting.CustomerId) });

                //AssignedToTask
                if (model.IsAssignedToTask)
                {
                    var task = new Pal.Core.Domains.Tasks.Task()
                    {
                        EmployeeId = model.EmployeeId,
                        CustomerId = model.CustomerId,
                        Subject = model.Title,
                        TaskPriority = Core.Enums.Task.TaskPriority.Normal,
                        StartDate = model.From,
                        StatusId = _context.SysTaskStatus.FirstOrDefault().Id,
                        EndDate = model.From.AddHours(1),
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

                return Meeting.Id;
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
                var meeting = await _context.Meetings.FirstOrDefaultAsync(a => a.Id == id);
                meeting.IsDeleted = true;
                _context.Meetings.Update(meeting);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteAsync), ActionType = ActionType.Delete, TransReferenceId = meeting.Id.ToString(), TransType = LogTransType.Meeting });
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
        public async Task<MeetingDTO> GetMeetingByIdAsync(int id)
        {
            try
            {
                var meeting = await _context.Meetings.FirstOrDefaultAsync(a => a.Id == id);

                if (meeting == null)
                    return null;

                var model = _mapper.Map<MeetingDTO>(meeting);
                model.CustomerName = _context.Customers.FirstOrDefault(x => x.Id == model.CustomerId).FullName;
                model.IsAssignedToTask = _context.Tasks.FirstOrDefault(x => x.ReferenceNumber == id && x.ReferenceType == ReferenceType.Meeting) == null ? false : true;

                var languages = _languageService.GetAllLanguages();
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetMeetingByIdAsync), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<int> UpdateAsync(MeetingDTO model)
        {
            try
            {
                var meeting = _mapper.Map<Meeting>(model);
                _context.Update(meeting);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateAsync), ActionType = ActionType.Update, TransReferenceId = meeting.Id.ToString(), TransType = LogTransType.Meeting });

                //AssignedToTask
                if (model.IsAssignedToTask)
                {
                    var task = new Pal.Core.Domains.Tasks.Task()
                    {
                        EmployeeId = model.EmployeeId,
                        CustomerId = model.CustomerId,
                        Subject = model.Title,
                        TaskPriority = Core.Enums.Task.TaskPriority.Normal,
                        StartDate = model.From,
                        StatusId = null,
                        ReferenceType = ReferenceType.Meeting,
                        ReferenceNumber = meeting.Id
                    };
                    _context.Tasks.Add(task);
                    await _context.SaveChangesAsync();
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

                return meeting.Id;
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
