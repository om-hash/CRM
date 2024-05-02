using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Domains.Lookups;
using Pal.Core.Enums;
using Pal.Data.DTOs.CRM.Lookups;
using Pal.Data.DTOs.Lookups;
using Pal.Data.DTOs.Region;
using Pal.Services.DataServices.Lookups;
using Pal.Services.DataServices.LookupsCRUDService;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.PalFunctions;
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
    public class LookupsController : BaseController
    {
        private readonly ILookupsCRUDService _lookupsService;
        private readonly ILookupsService _lookupsService1;
        private readonly ILoggerService _logger;
        public LookupsController(ILanguageService languageService, ILookupsService lookupsService1, ILocalizationService localizationService, ILookupsCRUDService lookupsService, INotificationService _notificationService, ILoggerService logger)
            : base(languageService, localizationService)
        {
            _lookupsService = lookupsService;
            _lookupsService1 = lookupsService1;
            _logger = logger;
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Index), ex);
                return NotFound();
            }
       
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCachingKeys()
        {
            try
            {
                var result = await _lookupsService.RemoveAllLookupKeys();
                if (result)
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be Clean!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RemoveCachingKeys), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        #region AirConditionTypes
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> AirConditionTypes()
        {
            try
            {
                var model = await _lookupsService.AirConditionTypesList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(AirConditionTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AirConditionTypesAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));
                
                SysAirConditionTypeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.AirConditionTypesAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));
                
                
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(AirConditionTypesAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AirConditionTypesUpdate(SysAirConditionTypeDTO model)
        {
            try
            {
                SysAirConditionTypeDTO result = await _lookupsService.AirConditionTypesUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(AirConditionTypesUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AirConditionTypesDelete(int id)
        {
            try
            {
                
                bool result = await _lookupsService.AirConditionTypesDelete(id);
                if(result == true)
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(AirConditionTypesDelete), ex);
                return Json(new ResponseResult(ResponseType.Error,ex.GetError()));
            }
        }
        #endregion


        #region HeatingTypes
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> HeatingTypes()
        {
            try
            {
                var model = await _lookupsService.HeatingTypesList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(HeatingTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> HeatingTypesAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysHeatingTypeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.HeatingTypesAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(HeatingTypesAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> HeatingTypesUpdate(SysHeatingTypeDTO model)
        {
            try
            {
                SysHeatingTypeDTO result = await _lookupsService.HeatingTypesUpdate(model);

                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(HeatingTypesUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> HeatingTypesDelete(int id)
        {
            try
            {

                bool result = await _lookupsService.HeatingTypesDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(HeatingTypesDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Nationality
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> Nationality()
        {
            try
            {
                var model = await _lookupsService.NationalitiesList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Nationality), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> NationalityAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysNationalityDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.NationalityAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(NationalityAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> NationalityUpdate(SysNationalityDTO model)
        {
            try
            {
                SysNationalityDTO result = await _lookupsService.NationalityUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(NationalityUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> NationalityDelete(int id)
        {
            try
            {

                bool result = await _lookupsService.NationalityDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(NationalityDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Payment Types
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> PaymentTypes()
        {
            try
            {
                var model = await _lookupsService.PaymentTypesList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PaymentTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> PaymentTypesAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysPaymentTypesDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.PaymentTypesAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PaymentTypesAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> PaymentTypesUpdate(SysPaymentTypesDTO model)
        {
            try
            {
                SysPaymentTypesDTO result = await _lookupsService.PaymentTypesUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PaymentTypesUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> PaymentTypesDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.PaymentTypesDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PaymentTypesDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Project Features
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> ProjectFeatures()
        {
            try
            {
                var model = await _lookupsService.ProjectFeaturesList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectFeatures), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> ProjectFeaturesAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysProjectFeatureDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.ProjectFeaturesAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectFeaturesAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> ProjectFeaturesUpdate(SysProjectFeatureDTO model)
        {
            try
            {
                SysProjectFeatureDTO result = await _lookupsService.ProjectFeaturesUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectFeaturesUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> ProjectFeaturesDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.ProjectFeaturesDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectFeaturesDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Project Statuses
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> ProjectStatuses()
        {
            try
            {
                var model = await _lookupsService.ProjectStatusList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectStatuses), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> ProjectStatusAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysProjectStatusDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.ProjectStatusAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectStatusAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
      

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> ProjectStatusUpdate(SysProjectStatusDTO model)
        {
            try
            {
                SysProjectStatusDTO result = await _lookupsService.ProjectStatusUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectStatusUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> ProjectStatusDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.ProjectStatusDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectStatusDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Project Types
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> ProjectTypes()
        {
            try
            {
                var model = await _lookupsService.ProjectTypesList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
    [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ProjectTypesAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysProjectTypeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.ProjectTypesAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectTypesAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ProjectTypesUpdate(SysProjectTypeDTO model)
        {
            try
            {
                SysProjectTypeDTO result = await _lookupsService.ProjectTypesUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectTypesUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ProjectTypesDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.ProjectTypesDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ProjectTypesDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region RealEstate Types
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> RealEstateTypes()
        {
            try
            {
                ViewBag.RealEstateClassId = (await _lookupsService1.GetSysRealEstateClasses()).ToSelectList();
                var model = await _lookupsService.RealEstateTypesList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealEstateTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RealEstateTypesAdd(List<ComboboxModelTranslate> translates,int classId)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysRealEstateTypeDTO model = new()
                {
                    Id = 0,
                    RealEstateClassId = classId,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.RealEstateTypesAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealEstateTypesAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RealEstateTypesUpdate(SysRealEstateTypeDTO model)
        {
            try
            {
                SysRealEstateTypeDTO result = await _lookupsService.RealEstateTypesUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealEstateTypesUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RealEstateTypesDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.RealEstateTypesDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealEstateTypesDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Rooms Count
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> RoomsCount()
        {
            try
            {
                var model = await _lookupsService.RoomsCountList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RoomsCount), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RoomsCountAdd(SysRoomsCountDTO model)
        {
            try
            {
                if (model == null)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                model = await _lookupsService.RoomsCountAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RoomsCountAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RoomsCountUpdate(SysRoomsCountDTO model)
        {
            try
            {
                SysRoomsCountDTO result = await _lookupsService.RoomsCountUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RoomsCountUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RoomsCountDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.RoomsCountDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RoomsCountDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region  Countries
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> Countries()
        {
            try
            {
                var model = await _lookupsService.CountryList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Countries), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysCountryDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.CountryAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CountryAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryUpdate(SysCountryDTO model)
        {
            try
            {
                SysCountryDTO result = await _lookupsService.CountryUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CountryUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.CountryDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CountryDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region  Cities

        //-------------------------------------------------------------------------

        public async Task<IActionResult> GetCities(int countryId)
        {
            try
            {
                var result = (await _lookupsService.GetSysCities(countryId)).ToSelectList();
                return Json(new ResponseResult(ResponseType.Success, result));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(GetCities), ex);
                return Content(ex.GetError());
            }
        }
        //-------------------------------------------------------------------------
 
        public async Task<IActionResult> Cities()
        {
            try
            {
                ViewBag.cbCountry = (await _lookupsService.GetSysCountries()).ToSelectList();
                var model = await _lookupsService.CityList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Cities), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> CityAdd(SysCityDTO model)
        {
            try
            {
                if (model == null)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                model = await _lookupsService.CityAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CityAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> CityUpdate(SysCityDTO model)
        {
            try
            {
                SysCityDTO result = await _lookupsService.CityUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CityUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> CityDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.CityDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CityDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region  Regions
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> Regions()
        {
            try
            {
                ViewBag.cbCountry = (await _lookupsService.GetSysCountries()).ToSelectList();
                ViewBag.cbCity = (await _lookupsService.GetSysCities(1)).ToSelectList();


                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Regions), ex);
                return Content(ex.GetError());
            }
        }
        
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> RegionsPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {

                var model = await _lookupsService.RegionList(dm);

                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Regions), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RegionAdd(SysRegionDTO model)
        {
            try
            {
                if (model == null)
                { 
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));
                }

                model.FullAddress = model.CountryId.ToString() + ", " + model.CityId.ToString();
                model = await _lookupsService.RegionAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RegionAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> RegionUpdate(SysRegionDTO model)
        {
            try
            {
                SysRegionDTO result = await _lookupsService.RegionUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RegionUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> RegionDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.RegionDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RegionDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region  Neighborhoods
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> Neighborhoods()
        {
            try
            {
                ViewBag.cbRegion = (await _lookupsService.GetSysRegions()).ToSelectList();

                //var model = await _lookupsService.NeighborhoodList();

                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Neighborhoods), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> NeighborhoodsPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {

                var model = await _lookupsService.NeighborhoodList(dm);

                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Neighborhoods), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> NeighborhoodAdd(SysNeighborhoodDTO model)
        {
            try
            {
                if (model == null)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                model = await _lookupsService.NeighborhoodAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(NeighborhoodAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        /*public async Task<IActionResult> AirConditionTypesUpdate(int id)
        {
            try
            {
                await _lookupsService.AirConditionTypesUpdate(model.Value);
                return Json(model.Value);
            }
            catch (Exception ex)
            {
                return Content(ex.GetError());
            }
        }*/

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> NeighborhoodUpdate(SysNeighborhoodDTO model)
        {
            try
            {
                SysNeighborhoodDTO result = await _lookupsService.NeighborhoodUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(NeighborhoodUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> NeighborhoodDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.NeighborhoodDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(NeighborhoodDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Places Types
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> PlacesTypes()
        {
            try
            {
                var model = await _lookupsService.PlacesTypeList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PlacesTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PlacesTypeAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysPlacesTypeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.PlacesTypeAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PlacesTypeAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PlacesTypeUpdate(SysPlacesTypeDTO model)
        {
            try
            {
                SysPlacesTypeDTO result = await _lookupsService.PlacesTypeUpdate(model);

                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PlacesTypeUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PlacesTypeDelete(int id)
        {
            try
            {

                bool result = await _lookupsService.PlacesTypeDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PlacesTypeDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region View Types
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> ViewTypes()
        {
            try
            {
                var model = await _lookupsService.ViewTypeList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ViewTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewTypeAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysViewTypeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.ViewTypeAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ViewTypeAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewTypeUpdate(SysViewTypeDTO model)
        {
            try
            {
                SysViewTypeDTO result = await _lookupsService.ViewTypeUpdate(model);

                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(ViewTypeUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewTypeDelete(int id)
        {
            try
            {

                bool result = await _lookupsService.ViewTypeDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Balconie Direction
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> BalconieDirections()
        {
            try
            {
                var model = await _lookupsService.BalconieDirectionList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(BalconieDirections), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> BalconieDirectionAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                BalconieDirectionDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.BalconieDirectionAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(BalconieDirectionAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> BalconieDirectionUpdate(BalconieDirectionDTO model)
        {
            try
            {
                BalconieDirectionDTO result = await _lookupsService.BalconieDirectionUpdate(model);

                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(BalconieDirectionUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> BalconieDirectionDelete(int id)
        {
            try
            {

                bool result = await _lookupsService.BalconieDirectionDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(BalconieDirectionDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion


        #region Comapny Category
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> CompanyCateogry()
        {
            try
            {
                var model = await _lookupsService.CompanyCategoryList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CompanyCateogry), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyCateogryAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysSalesCategoryDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.CompanyCategoryAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CompanyCateogryAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyCateogryUpdate(SysSalesCategoryDTO model)
        {
            try
            {
                SysSalesCategoryDTO result = await _lookupsService.CompanyCategoryUpdate(model);

                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CompanyCateogryUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyCateogryDelete(int id)
        {
            try
            {

                bool result = await _lookupsService.CompanyCategoryDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CompanyCateogryDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region CRM

        #region TaskStatus
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> TaskStatuses()
        {
            try
            {
                var model = await _lookupsService.TaskStatusList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(TaskStatuses), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> TaskStatusAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysTaskStatusDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.TaskStatusAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(TaskStatusAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> TaskStatusUpdate(SysTaskStatusDTO model)
        {
            try
            {
                SysTaskStatusDTO result = await _lookupsService.TaskStatusUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(TaskStatusUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> TaskStatusDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.TaskStatusDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(TaskStatusDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region CallType
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> CallTypes()
        {
            try
            {
                var model = await _lookupsService.CallTypeList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallTypes), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> CallTypeAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                CallTypeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.CallTypeAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(PaymentTypesAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> CallTypesUpdate(CallTypeDTO model)
        {
            try
            {
                if (model.Id == 1 || model.Id == 2)
                    return Json(new ResponseResult(ResponseType.HasTransactions));
                CallTypeDTO result = await _lookupsService.CallTypeUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallTypesUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> CallTypeDelete(int id)
        {
            try
            {
                if(id == 1 || id == 2)
                    return Json(new ResponseResult(ResponseType.HasTransactions));
                bool result = await _lookupsService.CallTypeDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallTypeDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region CallStatus
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> CallStatus()
        {
            try
            {
                var model = await _lookupsService.CallStatusList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallStatus), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> CallStatusAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                CallStatusDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.CallStatusAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallStatusAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> CallStatusUpdate(CallStatusDTO model)
        {
            try
            {
                CallStatusDTO result = await _lookupsService.CallStatusUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallStatusUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> CallStatusDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.CallStatusDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallStatusDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region CallPurpose
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> CallPurpose()
        {
            try
            {
                var model = await _lookupsService.CallPurposeList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallPurpose), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> CallPurposeAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                CallPurposeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.CallPurposeAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallPurposeAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> CallPurposeUpdate(CallPurposeDTO model)
        {
            try
            {
                CallPurposeDTO result = await _lookupsService.CallPurposeUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallPurposeUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> CallPurposeDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.CallPurposeDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallPurposeDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region CallResult
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> CallResult()
        {
            try
            {
                var model = await _lookupsService.CallResultList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallResult), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> CallResultAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                CallResultDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.CallResultAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallResultAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> CallResultUpdate(CallResultDTO model)
        {
            try
            {
                CallResultDTO result = await _lookupsService.CallResultUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallResultUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> CallResultDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.CallResultsDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(CallResultDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region DealStage
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> DealStage()
        {
            try
            {
                var model = await _lookupsService.DealStageList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealStage), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> DealStageAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                DealStageDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.DealStageAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealStageAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> DealStageUpdate(DealStageDTO model)
        {
            try
            {
                DealStageDTO result = await _lookupsService.DealStageUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealStageUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> DealStageDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.DealStageDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealStageDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region DealType
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> DealType()
        {
            try
            {
                var model = await _lookupsService.DealTypeList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealType), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> DealTypeAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                DealTypeDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.DealTypeAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealTypeAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> DealTypeUpdate(DealTypeDTO model)
        {
            try
            {
                DealTypeDTO result = await _lookupsService.DealTypeUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealTypeUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> DealTypeDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.DealTypeDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealTypeDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Job Title
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> JobTitles()
        {
            try
            {
                var model = await _lookupsService.JobTitleList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(JobTitles), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> JobTitleAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysJobTitleDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.JobTitleAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(JobTitleAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> JobTitleUpdate(SysJobTitleDTO model)
        {
            try
            {
                SysJobTitleDTO result = await _lookupsService.JobTitleUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(JobTitleUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> JobTitleDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.JobTitleDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(JobTitleDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Lead Source
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> LeadSource()
        {
            try
            {
                var model = await _lookupsService.LeadSourceList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadSource), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> LeadSourceAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                LeadSourceDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.LeadSourceAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DealStageAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> LeadSourceUpdate(LeadSourceDTO model)
        {
            try
            {
                LeadSourceDTO result = await _lookupsService.LeadSourceUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadSourceUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> LeadSourceDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.LeadSourceDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadSourceDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Lead Rating
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> LeadRating()
        {
            try
            {
                var model = await _lookupsService.LeadRatingList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadRating), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> LeadRatingAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                LeadRatingDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.LeadRatingAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadRatingAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> LeadRatingUpdate(LeadRatingDTO model)
        {
            try
            {
                LeadRatingDTO result = await _lookupsService.LeadRatingUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadRatingUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> LeadRatingDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.LeadRatingDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadRatingDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Lead Rating
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> LeadStatus()
        {
            try
            {
                var model = await _lookupsService.LeadStatusList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadStatus), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> LeadStatusAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                LeadStatusDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.LeadStatusAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadStatusAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> LeadStatusUpdate(LeadStatusDTO model)
        {
            try
            {
                LeadStatusDTO result = await _lookupsService.LeadStatusUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadStatusUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> LeadStatusDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.LeadStatusDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(LeadStatusDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Decision
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Decision_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> Decision()
        {
            try
            {
                var model = await _lookupsService.DecisionList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(Decision), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Decision_Lookup_Add, SuperAdmin")]
        public async Task<IActionResult> DecisionAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                DecisionDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.DecisionAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DecisionAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Decision_Lookup_Edit, SuperAdmin")]
        public async Task<IActionResult> DecisionUpdate(DecisionDTO model)
        {
            try
            {
                DecisionDTO result = await _lookupsService.DecisionUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DecisionUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Decision_Lookup_Remove, SuperAdmin")]
        public async Task<IActionResult> DecisionDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.DecisionDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(DecisionDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #endregion


        #region RealStateClass
        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_List, SuperAdmin")]
        public async Task<IActionResult> RealStates()
        {
            try
            {
                var model = await _lookupsService.RealStateList();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealStates), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Add, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RealStateClassAdd(List<ComboboxModelTranslate> translates)
        {
            try
            {
                if (translates == null || translates.Count < 1)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                SysRealStateClassDTO model = new()
                {
                    Id = 0,
                    Translates = translates.Select(a => new ComboboxModelTranslate { LanguageId = a.LanguageId, Name = a.Name }).ToList(),
                };

                model = await _lookupsService.RealStateClassAdd(model);

                if (model != null)
                    return Json(new ResponseResult(ResponseType.Success, model));

                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم الحفظ"));


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealStateClassAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RealStateClassUpdate(SysRealStateClassDTO model)
        {
            try
            {
                SysRealStateClassDTO result = await _lookupsService.RealStateClassUpdate(model);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, model));
                else
                    return Json(new ResponseResult(ResponseType.Error, "لم يتم التحديث"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealStateClassUpdate), ex);
                return Content(ex.GetError());
            }
        }

        //-------------------------------------------------------------------------
        [Authorize(Roles = "Settings_Lookup_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RealStateClassDelete(int id)
        {
            try
            {
                bool result = await _lookupsService.RealStateClassDelete(id);
                if (result == true)
                    return Json(new ResponseResult(ResponseType.Success));
                return Json(new ResponseResult(ResponseType.HasTransactions));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LookupsController" + nameof(RealStateClassDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

    }
}
