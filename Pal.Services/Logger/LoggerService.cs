using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.SystemErrors;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Data.Contexts;
using Pal.Services.WebWorkContext;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pal.Services.Logger
{
    
    public class LoggerService : ILoggerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebWorkContext _webWorkContext;

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IWebWorkContext webWorkContext)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _webWorkContext = webWorkContext;
        }

        //--------------------------------------------------------------------------------------------
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        //--------------------------------------------------------------------------------------------
        public Task LogDebugAsync(string message)
        {
            logger.Debug(message);
            return Task.CompletedTask;
        }
        //--------------------------------------------------------------------------------------------
        public void LogError(string message)
        {
            logger.Error(message);
        }
        //--------------------------------------------------------------------------------------------
        public Task LogErrorAsync(string action, Exception exception, Importance importance)
        {
            Console.WriteLine("Error: " + action + "  ||  " + exception.Message);
            //if (importance != Importance.Low) { }
            return Task.CompletedTask;
            SystemError error = new SystemError()
            {
                Id = Guid.NewGuid(),
                ActionName = action,
                ExceptionMessage = exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                Importance = importance,
                LogDate = DateTime.Now,
                //UserType = _webWorkContext.GetUserType(),
                UserId = _webWorkContext.GetMyUserId(),
            };
            _context.SystemErrors.Add(error);
            _context.SaveChanges();

            return Task.CompletedTask;
        }
        //--------------------------------------------------------------------------------------------
        public void LogInfo(string message)
        {
            logger.Info(message);
        }
        //--------------------------------------------------------------------------------------------
        public Task LogInfoAsync(string message)
        {
            logger.Info(message);
            return Task.CompletedTask;
        }
        //--------------------------------------------------------------------------------------------
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
        //--------------------------------------------------------------------------------------------
        public Task LogWarnAsync(string message)
        {
            logger.Info(message);
            return Task.CompletedTask;
        }

        //--------------------------------------------------------------------------------------------
        public async Task LogInfoAsync(ActivityLog model)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                string userReferenceId = (await GetCustomerIdByUserId(userId)) ?? "Admin";
                model.IsCustomer = true;
                model.UserName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                model.UserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                model.UserPhone = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";
                model.UserId = userId;
                model.UserTypeReferenceId = userReferenceId;
                model.UserType = GetUserTypeAsEnum();
               
            }
            model.TransDate = DateTime.Now;
            _context.ActivityLogs.Add(model);
            await _context.SaveChangesAsync();
        }
        //--------------------------------------------------------------------------------------------
        private UserType GetUserTypeAsEnum()
        {
            var userType = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value;

            switch (userType)
            {
                case "Admins":
                    return UserType.Admins;
                case "Companies":
                    return UserType.Companies;
                case "RealStateAgents":
                    return UserType.RealStateAgents;
                case "Customers":
                    return UserType.Customers;
                case "Adviser":
                    return UserType.Adviser;
                case "Unknown":
                    return UserType.Unknown;
            }
            return UserType.Unknown;
        }
        //--------------------------------------------------------------------------------------------
        private async Task<string> GetCustomerIdByUserId(string userId)
        {
            var CustomerId = await _context.Users.Where(x => x.Id == userId).Select(a => a.ReferenceId).FirstOrDefaultAsync();
            return CustomerId;
        }

        //--------------------------------------------------------------------------------------------

        public async Task LogVisitAsync(string LocalStorageKey)
        {
            try
            {
                var model = new ActivityLog()
                {
                    TransDate = DateTime.Now,
                    ActionType = ActionType.Visite,
                    UserType = UserType.Customers,
                    TransType = LogTransType.Visite,
                };
                await _context.ActivityLogs.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception )
            {

            }
        }
    }
}