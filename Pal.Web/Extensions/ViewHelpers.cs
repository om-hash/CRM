using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Lookups;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Lookups;
using Pal.Services.Languages;
using Pal.Services.WebWorkContext;
using Pal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Extensions
{
   

    //----------------------------------------------------------------------
    //----------------------------------------------------------------------
    public class AddressViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly ILanguageService _languageService;
        public AddressViewComponent(ApplicationDbContext context, IWebWorkContext workContext, ILanguageService languageService)
        {
            _context = context;
            _workContext = workContext;
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var model = new FullAddressViewModel
                {
                    Countries = await _context.SysCountries.Select(a => new SysCountryDTO
                    {
                        Id = a.Id,
                        CountryName = a.SysCountryTranslates.FirstOrDefault(x => x.LanguageId == langId).CountryName
                    }).ToListAsync(),
                    Cities = await _context.SysCities.Select(a => new SysCityDTO
                    {
                        Id = a.Id,
                        CountryId = a.CountryId,
                        CityName = a.SysCityTranslates.FirstOrDefault(x => x.LanguageId == langId).CityName
                    }).ToListAsync(),
                    Regions = await _context.SysRegions.Select(a => new SysRegionDTO
                    {
                        Id = a.Id,
                        CityId = a.CityId,
                        RegionName = a.SysRegionTranslates.FirstOrDefault(x => x.LanguageId == langId).RegionName
                    }).ToListAsync(),
                    Neighborhoods = await _context.SysNeighborhoods.Select(a => new SysNeighborhoodDTO
                    {
                        Id = a.Id,
                        RegionId = a.RegionId,
                        NeighborhoodName = a.SysNeighborhoodTranslates.FirstOrDefault(x => x.LanguageId == langId).NeighborhoodName
                    }).ToListAsync(),
                };
                return View("_AddressEditor",model);
            }
            catch (Exception)
            {
                return View("_AddressEditor");
            }

        
        }
    }
}
