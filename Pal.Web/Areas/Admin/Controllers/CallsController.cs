using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Enums;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Call;
using Pal.Services.CRM.Calls;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Languages;
using Pal.Services.Logger;
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
    [Authorize]
    [Area("Admin")]
    public class CallsController : BaseController
    {
        private readonly ICallService _callService;
        private readonly ILookupsService _lookupsService;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public CallsController(
          ILanguageService languageService, ILocalizationService localizationService, IMapper mapper, ICallService callService, ILookupsService lookupsService, ILoggerService logger) : base(languageService, localizationService)
        {
            _callService = callService;
            _lookupsService = lookupsService;
            _logger = logger;
            _mapper = mapper;
        }
        private async Task GetComboBoxes()
        {
            ViewBag.Employee = (await _lookupsService.GetEmployeeAsLookup()).ToSelectList();
            ViewBag.Customers = (await _lookupsService.GetCustomerAsLookup()).ToSelectList();
            ViewBag.Leads = (await _lookupsService.GetLeadsAsLookup()).ToSelectList();
            ViewBag.CallTypes = (await _lookupsService.GetCallType()).ToSelectList();
            ViewBag.CallPurpose = (await _lookupsService.GetCallPurpose()).ToSelectList();
            ViewBag.CallStatus = (await _lookupsService.GetCallStatus()).ToSelectList();
            ViewBag.CallResult = (await _lookupsService.GetCallResult()).ToSelectList();
            
            
            ViewBag.Customers1 = await _lookupsService.GetCustomerAsLookup();
            ViewBag.Leads1 = await _lookupsService.GetLeadsAsLookup();
        }

        [Authorize(Roles = "Call_List, SuperAdmin")]
        public IActionResult CallsList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallsList), ex);
                return NotFound();
            }
        }
        
        [Authorize(Roles = "Call_List, SuperAdmin")]
        public async Task<IActionResult> ScheduledCallsPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                
                var model = await _callService.GetAllAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.DataSolo.ScheduledCalls, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallsList), ex);
                return NotFound();
            }
        }

        [Authorize(Roles = "Call_List, SuperAdmin")]
        public async Task<IActionResult> DoneCallsPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var model = await _callService.GetAllAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.DataSolo.DoneCalls, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallsList), ex);
                return NotFound();
            }
        }
        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Call_Add, SuperAdmin")]
        public async Task<IActionResult> CallAdd()
        {
            try
            {
                var model = new CallDTO
                {
                };
               await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallAdd), ex);
                return NotFound();
            }

        }
        //--------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Call_Add, SuperAdmin")]
        public async Task<IActionResult> CallAdd(CallDTO model)
        {
            model.CustomerName = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                var result = await _callService.AddAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Call_List, SuperAdmin")]
        public async Task<IActionResult> CallUpdate(int id)
        {
            try
            {
                var project = await _callService.GetCallByIdAsync(id);
                await GetComboBoxes();
                return View(project);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallUpdate), ex);
                return NotFound();
            }

        }
        //------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Call_Edit, SuperAdmin")]
        public async Task<IActionResult> CallUpdate(CallDTO model)
        {
            try
            {
                var result = await _callService.UpdateAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallUpdate), ex);
                return NotFound();
            }
        }
        //------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Call_Remove, SuperAdmin")]
        public async Task<IActionResult> CallDelete(int id)
        {
            try
            {
                var result = await _callService.DeleteAsync(id);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Call_Add, SuperAdmin")]
        public async Task<IActionResult> CallSchedule()
        {
            try
            {
                var model = new CallDTO
                {
                };
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CallController" + nameof(CallSchedule), ex);
                return NotFound();
            }

        }
        //--------------------------------------------------------------------------------------
    }
}
