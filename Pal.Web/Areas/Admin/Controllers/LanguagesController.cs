using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Enums;
using Pal.Data.DTOs.Languages;
using Pal.Services.DataServices.LanguegService;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LanguagesController : BaseController
    {
        private readonly ILanguagesService _langService;
        private readonly ILoggerService _logger;

        public LanguagesController(ILanguagesService langService, ILoggerService logger, Services.Languages.ILanguageService languageService, ILocalizationService localizationService, INotificationService _notificationService)
            : base(languageService, localizationService)

        {
            _langService = langService;
            _logger = logger;

        }

        //---------------------------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_List, SuperAdmin")]
        public IActionResult LanguageList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(LanguageList), ex);
                return NotFound();
            }
        }

        //---------------------------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_List, SuperAdmin")]
        public async Task<IActionResult> LanguagesPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var model = await _langService.GetAllLanguages(dm);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(LanguageList), ex);
                return NotFound();
            }

        }
        //--------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_Add, SuperAdmin")]
        public IActionResult LanguageAdd()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(LanguageAdd), ex);
                return NotFound();
            }
        
        }


        //---------------------------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_Add, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LanguageAdd(LanguageCreateDTO languageCreateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.ModelNotValid, "ModelNotValid"));

                var resopns = await _langService.AddLanguage(languageCreateDTO);
                if (resopns == true)
                    return Json(new ResponseResult(ResponseType.Success));
                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(LanguageAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //---------------------------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_List, SuperAdmin")]
        public async Task<IActionResult> LanguageUpdate(int id)
        {
            try
            {
                var model = await _langService.GetLanguageById(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(LanguageUpdate), ex);
                return NotFound();
            }
    
        }


        //---------------------------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_Edit, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LanguageUpdate(LanguageCreateDTO languageCreateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.ModelNotValid, "ModelNotValid"));

                var resopns = await _langService.LanguageUpdate(languageCreateDTO);
                if (resopns == true)
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(LanguageUpdate), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
          
        }



        //---------------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewResource(string ResourceKey, string ResourceValue, string ResourceValue2, string ResourceValue3)
        {
            try
            {
                var resopns = await _langService.AddNewResource(ResourceKey, ResourceValue, 1);
                await _langService.AddNewResource(ResourceKey, ResourceValue2, 2);
                await _langService.AddNewResource(ResourceKey, ResourceValue3, 3);
                if (resopns == true)
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(AddNewResource), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
       
        }

        //---------------------------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_Edit, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> StringResourceUpdate(int id, string value)
        {
            try
            {
                var resopns = await _langService.StringResourceUpdate(id, value);
                if (resopns == true)
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(StringResourceUpdate), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
         
        }


        //---------------------------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Language_Remove, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LanguageDelelte(int id)
        {
            try
            {
                if (await _langService.DelelteLanguage(id))
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be Delete!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LanguagesController" + nameof(LanguageDelelte), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
         
        }
    }
}
