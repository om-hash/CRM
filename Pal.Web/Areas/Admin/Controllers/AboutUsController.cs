using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Enums;
using Pal.Core.Enums.Roles;
using Pal.Data.DTOs.AboutUs;
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
    public class AboutUsController : BaseController
    {
        private readonly ILoggerService _logger;
        public AboutUsController(ILoggerService logger, Services.Languages.ILanguageService languageService, ILocalizationService localizationService, INotificationService _notificationService)
            : base(languageService, localizationService)
        { 
            _logger = logger;
        }
        [Authorize(Roles = "WebSiteInfo_Aboutus_Show, SuperAdmin")]
        public IActionResult Index()
        {
            try
            {
                //var model = await _AboutUsService.GetAboutUsForUpdate();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AboutUsController" + nameof(Index), ex);
                return NotFound();

            }


        }
        [Authorize(Roles = "WebSiteInfo_Aboutus_Edit, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AboutUsCreateDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.ModelNotValid, "ModelNotValid"));

                //var response = await _AboutUsService.AboutUsUpdate(model);
                //if (response == true)
                //    return Json(new ResponseResult(ResponseType.Success));

                //else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be Update!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AboutUsController" + nameof(Index), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }
    }
}
