using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pal.Core.Domains.Account;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Statistics;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Statistics.Charts;
using Pal.Services.DataServices.statistics;
using Pal.Services.FileManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Pal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DashboardController : BaseController
    {
        private readonly INotificationService _notificationService;
        private readonly ILoggerService _logger;
        private readonly IStatisticsService _statisticsService;
        private readonly ApplicationDbContext _sysdb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggerService _loggerService;
        private readonly IFileManagerService _file;



        public DashboardController(ApplicationDbContext db,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILoggerService loggerService,
            UserManager<ApplicationUser> userManager, ILoggerService logger, IFileManagerService file, INotificationService notificationService, IStatisticsService statisticsService) : base(languageService, localizationService)
        {
            _notificationService = notificationService;
            _logger = logger;
            _statisticsService = statisticsService;
            _sysdb = db;
            _userManager = userManager;
            _loggerService = loggerService;
            _file = file;
        }

        //----------------------------------------------------------------------------------
        //[ServiceFilter(typeof(CheckCompanyVerifiedAttribute))]
        public IActionResult Index()
        {
            try
            {
                if (User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Adviser.ToString())
                    return Redirect("/Admin/Advisors/Index");
                if (User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Lawyer.ToString())
                    return Redirect("/Admin/Lawyers/Index");

                var userType = User.FindFirst(PalClaimType.UserType.ToString()).Value;
                if (userType != UserType.Companies.ToString() && userType != UserType.Admins.ToString() && userType != UserType.Adviser.ToString())
                    return Redirect("/");

                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(Index), ex);
                return NotFound();
            }
        }

        //----------------------------------------------------------------------------------
        public class FileUploadModel
        {
            public IFormFile imgFile { get; set; }
        }
        public IActionResult ChangeLogo(FileUploadModel img)
        {
            var x = _file.UploadLogo(img.imgFile);
            return Ok();
        }
        
        //----------------------------------------------------------------------------------
        public IActionResult VerifiedPending()
        {
            return View();
        }

        //----------------------------------------------------------------------------------
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var resopns = await _notificationService.GetNotificationsAsync(25);
                if (resopns != null)
                    return Json(new ResponseResult(ResponseType.Success, resopns));
                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot Load Notifications"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(GetNotifications), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }

        //----------------------------------------------------------------------------------
        public async Task<IActionResult> SetNotificatoinSeen(long id)
        {
            try
            {
                await _notificationService.SetNotificationSeen(id);
                return Json(new ResponseResult(ResponseType.Success));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(SetNotificatoinSeen), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }
        //----------------------------------------------------------------------------------
        public IActionResult AccessDenied()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(AccessDenied), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //----------------------------------------------------------------------------------

        public async Task<IActionResult> GetStatistics(DateTimeType Date, DateTime? dateF, DateTime? dateT)
        {
            try
            {

                var model = await _statisticsService.GetStatistics(Date, dateF, dateT);
                if (User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Companies.ToString())
                    return PartialView("_StatisticsChartsForCompany", model);

                return PartialView("_StatisticsCharts", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(GetStatistics), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //----------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetMostPropertiesStatistics(DateTimeType Date, DateTime? dateF, DateTime? dateT, string ActionName)
        {
            try
            {
                var model = await _statisticsService.MostPropertiesVisitingCount(Date, dateF, dateT, ActionName);
                return PartialView("_LineChart", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(GetMostPropertiesStatistics), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //----------------------------------------------------------------------------------
        public async Task<IActionResult> LatestPropertiesAdded(LogTransType actionType)
        {
            try
            {
                var model = await _statisticsService.LatestPropertiesAdded(actionType);
                return PartialView("_BoardList", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(LatestPropertiesAdded), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //----------------------------------------------------------------------------------
        public async Task<IActionResult> MostActiveCutomers()
        {
            try
            {
                var model = await _statisticsService.MostActiveCustomer();
                return PartialView("_BoardList2", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(MostActiveCutomers), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //----------------------------------------------------------------------------------

        public IActionResult MostActiveAdvisors()
        {
            try
            {
                //var model = await _statisticsService.MostAgentsActive();
                //return PartialView("_PyramidChart", model);
                return null;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(MostActiveAdvisors), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //----------------------------------------------------------------------------------
        public async Task<IActionResult> DealsStatistics()
        {
            try
            {
                var model = await _statisticsService.GetDealStatistics();
                return PartialView("_BubbleChart", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(MostActiveAdvisors), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //----------------------------------------------------------------------------------

        public IActionResult RealEstatesListForCompany()
        {
            try
            {
                //var model = await _realEstatesService.GetAllAsListAsync(new Syncfusion.EJ2.Base.DataManagerRequest(), false);
                return PartialView("_RealEstateListForDashboard"/*, model*/);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("RealEstatesController" + nameof(RealEstatesListForCompany), ex);
                return NotFound();
            }

        }
        //------------------------------------------------------------------------------------
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var resopns = await _notificationService.GetNotificationsAsync(0);
                return View(resopns);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DashboardController" + nameof(GetAllNotifications), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }
        //------------------------------------------------------------------------------------
        public IActionResult ChangePassword(string UserId)
        {
            ViewBag.UserId = UserId;
            return View();
        }
        //------------------------------------------------------------------------------------
        public IActionResult EditableTable(ChangePasswordVM model)
        {
            return View();
        }
        //------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(model.Id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (result.Succeeded)
                {
                    return Json(new ResponseResult(ResponseType.Success));
                }
                else
                {
                    return Json(new ResponseResult(ResponseType.Error, Localize(result.Errors.First().Code.ToString()).ToString()));
                }
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ChangePassword), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }


    }
}
