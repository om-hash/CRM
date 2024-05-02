using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Lookups;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using Pal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly ILookupsService _lookupsService;
        private readonly ILoggerService _logger;

        public AddressController(ILoggerService logger, ApplicationDbContext context, IWebWorkContext workContext, ILookupsService lookupsService)
        {
            _context = context;
            _workContext = workContext;
            _lookupsService = lookupsService;
            _logger = logger;
        }
        //public async Task<IActionResult> GetAddressLookups()
        //{
        //    try
        //    {
        //        var langId = await _workContext.GetLanguageIdFromRequestAsync();
        //        var model =await _lookupsService.GetAddressLookups();
        //        return Json(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = _logger.LogErrorAsync("AddressController" + nameof(GetAddressLookups), ex);
        //        return NotFound();
        //    }
        //}

        public async Task<IActionResult> GetCountryLookup()
        {
            try
            {
                //var langId = await _workContext.GetLanguageIdFromRequestAsync();
                var model = await _lookupsService.GetCountries();
                return Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AddressController" + nameof(GetCountryLookup), ex);
                return NotFound();
            }
        }

        public async Task<IActionResult> GetCityLookup(int id)
        {
            try
            {
                var model = await _lookupsService.GetCitiesByCountryId(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AddressController" + nameof(GetCityLookup), ex);
                return NotFound();
            }
        }

        public async Task<IActionResult> GetRegionLookup(int id)
        {
            try
            {
                //var langId = await _workContext.GetLanguageIdFromRequestAsync();
                var model = await _lookupsService.GetRegionsByCityId(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AddressController" + nameof(GetRegionLookup), ex);
                return NotFound();
            }
        }

        public async Task<IActionResult> GetNeighborhoodLookup(int id)
        {
            try
            {
                //var langId = await _workContext.GetLanguageIdFromRequestAsync();

                var model = await _lookupsService.GetNeighborhoodsByRegionId(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("AddressController" + nameof(GetNeighborhoodLookup), ex);
                return NotFound();
            }
        }
    }
}
