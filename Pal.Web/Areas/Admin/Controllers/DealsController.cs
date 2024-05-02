using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Domains.Customers;
using Pal.Core.Domains.Deals;
using Pal.Core.Enums;
using Pal.Data.DTOs.CRM.Deals;
using Pal.Services.CRM.Deals;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.PalFunctions;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Syncfusion.EJ2.Base;
using System;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DealsController : BaseController
    {
        private readonly IDealsService _dealService;
        private readonly ILookupsService _lookupsService;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public DealsController(
          ILanguageService languageService, ILocalizationService localizationService, IMapper mapper, IDealsService dealService, 
          ILookupsService lookupsService, ILoggerService logger) : base(languageService, localizationService)
        {
            _dealService = dealService;
            _lookupsService = lookupsService;
            _logger = logger;
            _mapper = mapper;
        }
        private async Task GetComboBoxes()
        {
            ViewBag.Employee = (await _lookupsService.GetEmployeeAsLookup()).ToSelectList();
            ViewBag.Customers = (await _lookupsService.GetCustomerAsLookup()).ToSelectList();
            ViewBag.DealTypes = (await _lookupsService.GetDealType()).ToSelectList();
            ViewBag.DealStage = (await _lookupsService.GetDealStage()).ToSelectList();
            ViewBag.LeadSource = (await _lookupsService.GetLeadSources()).ToSelectList();


            ViewBag.Customers1 = await _lookupsService.GetCustomerAsLookup();
            ViewBag.Leads1 = await _lookupsService.GetLeadsAsLookup();
        }

        [Authorize(Roles = "Deal_List, SuperAdmin")]
        public IActionResult DealsList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealsList), ex);
                return NotFound();
            }
        }
        
        
        [Authorize(Roles = "Deal_List, SuperAdmin")]
        public async Task<IActionResult> DealsPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var model = await _dealService.GetAllAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealsList), ex);
                return NotFound();
            }
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Deal_Add, SuperAdmin")]
        public async Task<IActionResult> DealAdd()
        {
            try
            {
                var model = new DealDTO();
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealAdd), ex);
                return NotFound();
            }

        }

        //--------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Deal_Add, SuperAdmin")]
        public async Task<IActionResult> DealAdd(DealDTO model)
        {
            model.CustomerName = model.CustomerName ?? string.Empty;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                var result = await _dealService.AddAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Deal_List, SuperAdmin")]
        public async Task<IActionResult> DealUpdate(int id)
        {
            try
            {
                var project = await _dealService.GetDealByIdAsync(id);
                await GetComboBoxes();
                return View(project);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealUpdate), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Deal_Edit, SuperAdmin")]
        public async Task<IActionResult> DealUpdate(DealDTO model)
        {
            try
            {
                var result = await _dealService.UpdateAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealUpdate), ex);
                return NotFound();
            }
        }

        //------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Deal_Remove, SuperAdmin")]
        public async Task<IActionResult> DealDelete(int id)
        {
            try
            {
                var result = await _dealService.DeleteAsync(id);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Deal_Add, SuperAdmin")]
        public async Task<IActionResult> DealSchedule()
        {
            try
            {
                var model = new DealDTO
                {
                };
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealSchedule), ex);
                return NotFound();
            }

        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Deal_List, SuperAdmin")]
        public async Task<IActionResult> DealDetails(int id)
        {
            try
            {
                var deal = await _dealService.GetDealDetails(id);
                var sdsa = ViewBag.DealStage = (await _lookupsService.GetDealStage()).ToSelectList();
                return View(deal);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(DealDetails), ex);
                return NotFound();
            }

        }

        [Authorize(Roles = "Deal_Edit, SuperAdmin")]
        public async Task<IActionResult> ChangeDealStage(int id, int stageId)
        {
            try
            {
                var result = await _dealService.ChangeDealStage(id, stageId);
                if (result)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("DealController" + nameof(ChangeDealStage), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
    }
}
