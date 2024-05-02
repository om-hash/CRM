using AutoMapper;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Notifications;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.Notifications;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Notifications;
using Pal.Services.Hubs;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pal.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly ILoggerService _logger;

        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IWebHostEnvironment _env;
        private readonly ILanguageService _languageService;

        public NotificationService(ILoggerService logger, ApplicationDbContext context, IWebWorkContext workContext,
            IMapper mapper, IHubContext<NotificationHub> hub, IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment env, ILanguageService languageService)
        {
            _logger = logger;
            _context = context;
            _workContext = workContext;
            _mapper = mapper;
            _hub = hub;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _languageService = languageService;
        }

        //-----------------------------------------------------------------
        public async Task<bool> SendNotificationAsync(NotificationDTO model)
        {
            try
            {
                var notification = _mapper.Map<Core.Domains.Notifications.Notification>(model);
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                await SendMobileNotification("Title", "Body", "");
                var data = GetNotificationMsg(notification.Id);
                _ = _hub.Clients.Group(model.GroupId).SendAsync("notify", data);
                return false;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendNotificationAsync), ex);
                return false;
            }
        }

        //-----------------------------------------------------------------
        public async Task<bool> SendNotificationIfNotExistAsync(NotificationDTO model)
        {
            try
            {
                if (!await _context.Notifications.AnyAsync(a => a.NotificationTypeId == model.NotificationTypeId &&
                                                                a.NotificationFor == model.NotificationFor &&
                                                                a.GroupId == model.GroupId &&
                                                                string.IsNullOrEmpty(a.SeenUserIdsList)))
                {
                    await SendNotificationAsync(model);
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendNotificationIfNotExistAsync), ex);
                return false;
            }
        }


        //-----------------------------------------------------------------
        private List<NotificationMsg> GetNotificationMsg(long id)
        {
            try
            {
                var notification = _context.Notifications
                    .Include(a => a.NotificationType)
                    .ThenInclude(a => a.NotificationTypeTranslates)
                    .FirstOrDefault(a => a.Id == id);
                var model = notification.NotificationType.NotificationTypeTranslates.Select(a => new NotificationMsg
                {
                    Id = a.Id,
                    Date = notification.NotifyDate,
                    LanguageId = a.LanguageId,
                    Url = notification.Url,
                    Title = string.Format(a.TypeName, notification.NotificationParams.OrderBy(x => x.Id).Select(x => x.Param).ToArray()),

                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetNotificationMsg), ex);
                return null;
            }
        }


        //-----------------------------------------------------------------
        public async Task<bool> SetNotificationSeen(long id)
        {
            try
            {

                string groupId = GetMyGroupId();
                string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //var noti = await _context.Notifications.Where(a => a.GroupId == groupId
                //&& (a.SeenUserIdsList == null || !a.SeenUserIdsList.Contains(userId))).ToListAsync();

                var noti = await _context.Notifications.Where(x => x.Id == id).FirstOrDefaultAsync();
                _ = noti.SeenUserIdsList += ", " + userId;
                _context.Update(noti);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SetNotificationSeen), ex);
                return false;
            }
        }

        //---------------------------------------------------
        private string GetMyGroupId()
        {
            string groupName = "";


            if (_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Companies.ToString())
            {
                //geting the user type
                var companyid = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.CompanyId.ToString()).Value);
                groupName = UserType.Companies.ToString() + companyid;
                //end of geting the user type

            }
            if (_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Adviser.ToString())
            {
                //geting the user type
                var AdvisorId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.AdvisorId.ToString()).Value);
                groupName = UserType.Adviser.ToString() + AdvisorId;
                //end of geting the user type

            }


            if (_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Admins.ToString())
            {
                //geting the user type
                var EmployeeId = "";

                EmployeeId = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.EmployeeId.ToString())?.Value ?? "";

                groupName = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value + EmployeeId;

                //end of geting the user type
            }


            return groupName;
        }
        //------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<NotificationMsg>> GetNotificationsAsync(int count)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var groupName = GetMyGroupId();
                var notification = new List<Core.Domains.Notifications.Notification>();
                if (_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Admins.ToString())
                {
                    notification.AddRange(_context.Notifications.Where(z => z.GroupId == groupName || z.GroupId == UserType.Admins.ToString()).Include(a => a.NotificationParams)
                   .Include(a => a.NotificationType)
                   .ThenInclude(a => a.NotificationTypeTranslates)
                   .OrderByDescending(x => x.NotifyDate));
                }
                else
                {
                    notification.AddRange(_context.Notifications.Where(z => z.GroupId == groupName).Include(a => a.NotificationParams)
                  .Include(a => a.NotificationType)
                  .ThenInclude(a => a.NotificationTypeTranslates)
                  .OrderByDescending(x => x.NotifyDate));

                }

                var notifications = notification
                   .Select(a => new NotificationMsg
                   {

                       Id = a.Id,
                       NotificationTypeId = a.NotificationTypeId,
                       NotificationTypeName = a.NotificationType.NotificationTypeTranslates.FirstOrDefault(s => s.NotificationTypeId == a.NotificationTypeId).TypeName,
                       Title = string.Format(a.NotificationType.NotificationTypeTranslates.FirstOrDefault(b => b.LanguageId == langId)?.TypeName ?? "", a.NotificationParams.OrderBy(b => b.Id).Select(b => b.Param).ToArray()),
                       Date = a.NotifyDate,
                       Url = a.Url,

                       IsSeen = (a.SeenUserIdsList ?? "").Contains(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                   })
                   .OrderByDescending(x => x.Date).ToList();

                if (count > 0)
                    notifications = notifications.ToList().Take(count).ToList();



                return notifications;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetNotificationMsg), ex);
                return null;
            }
        }

        public async Task<IEnumerable<NotificationMsg>> GetPAginatedNotificationsAsync(int pageSize, int pageNumber)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var groupName = GetMyGroupId();
                var notification = new List<Core.Domains.Notifications.Notification>();
                var query = _context.Notifications.AsQueryable().AsNoTracking();
                if (_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Admins.ToString())
                {
                    notification.AddRange(query.Where(z => z.GroupId == groupName || z.GroupId == UserType.Admins.ToString()).Include(a => a.NotificationParams)
                   .Include(a => a.NotificationType)
                   .ThenInclude(a => a.NotificationTypeTranslates)
                   .OrderByDescending(x => x.NotifyDate));
                }
                else
                {
                    notification.AddRange(query.Where(z => z.GroupId == groupName).Include(a => a.NotificationParams)
                  .Include(a => a.NotificationType)
                  .ThenInclude(a => a.NotificationTypeTranslates)
                  .OrderByDescending(x => x.NotifyDate));

                }

                var notifications = notification.Skip(pageSize * (pageNumber - 1)).Take(pageSize)
                   .Select(a => new NotificationMsg
                   {

                       Id = a.Id,
                       NotificationTypeId = a.NotificationTypeId,
                       NotificationTypeName = a.NotificationType.NotificationTypeTranslates.FirstOrDefault(s => s.NotificationTypeId == a.NotificationTypeId).TypeName,
                       Title = string.Format(a.NotificationType.NotificationTypeTranslates.FirstOrDefault(b => b.LanguageId == langId)?.TypeName ?? "", a.NotificationParams.OrderBy(b => b.Id).Select(b => b.Param).ToArray()),
                       Date = a.NotifyDate,
                       Url = a.Url,

                       IsSeen = (a.SeenUserIdsList ?? "").Contains(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                   })
                   .OrderByDescending(x => x.Date).ToList();



                return notifications;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetNotificationMsg), ex);
                return null;
            }
        }


        public async Task SendMobileNotification(string Title, string Body, string Image)
        {
            var path = _env.ContentRootPath;
            path += "\\firebase.json";
            FirebaseApp app;
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "myApp");
            }
            catch (Exception)
            {
                app = FirebaseApp.GetInstance("myApp");
            }

            var fcm = FirebaseMessaging.GetMessaging(app);
          
            Message message = new()
            {
                
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Title = Title,
                        Body = Body,
                    }
                },

                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = Title,
                    Body = Body,

                },
               
                Data = new Dictionary<string, string>()
                {
                    { "Title", Title },
                    { "Body", Body },
                },

                Topic = "all",

            };

           var result = await fcm.SendAsync(message);
        }
    }
}