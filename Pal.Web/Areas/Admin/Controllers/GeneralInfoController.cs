using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Enums;
using Pal.Data.DTOs.GeneralInfo;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class GeneralInfoController : BaseController
    {
        private readonly ILoggerService _logger;
        public GeneralInfoController(Services.Languages.ILanguageService languageService, ILocalizationService localizationService, INotificationService _notificationService, ILoggerService logger)
            : base(languageService, localizationService)
        {
            _logger = logger;
        }

        [Authorize(Roles = "WebSiteInfo_GeneralInfo_Show, SuperAdmin")]
        public IActionResult Index()
        {
            try
            {
                //var model = await _generalInforService.GeneralInfoForUpdate();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("GeneralInfoController" + nameof(Index), ex);
                return NotFound();
            }
           
        }
        [Authorize(Roles = "WebSiteInfo_GeneralInfo_Edit, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(GeneralInfoDTO generalInfoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.ModelNotValid, "ModelNotValid"));

                //var resopns = await _generalInforService.GeneralInfoUpdate(generalInfoDTO);
                //if (resopns == true)
                //    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("GeneralInfoController" + nameof(Index), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
       
        }
    }
}
