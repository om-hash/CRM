using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Lookups;
using Pal.Core.Enums.Account;
using Pal.Core.VMs;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Lookups;
using Pal.Data.VMs.Address;
using Pal.Data.VMs.Search;
using Pal.Services.Caching;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.Lookups
{
    public class LookupsService : ILookupsService
    {
        #region Fields and CTor
        
        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private List<SysCountry> SysCountries;
        //private List<SysCity> SysCities;
        //private List<SysRegion> SysRegions;
        //private List<SysNeighborhood> SysNeighborhoods;
        private readonly ILoggerService _logger;
        private readonly ILanguageService _languageService;


        // caching
        private readonly ICacheService<List<LookupsCachingModel>> _cacheService;

        //private List<LookupsCachingModel> _cachedProjctStatuses;
        private List<LookupsCachingModel> _cachedDecision;
        private List<LookupsCachingModel> _cachedDecisionAsLookup;

        private List<LookupsCachingModel> _cachedPaymentTypes;
        private List<LookupsCachingModel> _cachedViewTypes;
        private List<LookupsCachingModel> _cachedBlaconieDirections;
        private List<LookupsCachingModel> _cachedProjectFeatures;
        private List<LookupsCachingModel> _cachedCompaniesAsLookup;
        private List<LookupsCachingModel> _cachedSalesAsLookup;
        //private List<LookupsCachingModel> _cachedRealEstateFeatures;
        //private List<LookupsCachingModel> _cachedProjectsAsLookup;
        //private List<LookupsCachingModel> _cachedRealEstatesAsLookup;
        //private List<LookupsCachingModel> _cachedRealEstateType;
        //private List<LookupsCachingModel> _cachedRealEstateTypeByClassId;
        //private List<LookupsCachingModel> _cachedRealEstateClass;
        private List<LookupsCachingModel> _cachedHeatingTypes;
        private List<LookupsCachingModel> _cachedCompanyCategories;
        private List<LookupsCachingModel> _cachedSalesCategories;
        private List<LookupsCachingModel> _cachedPlacesTypes;
        private List<LookupsCachingModel> _cachedAirConditionTypes;
        private List<LookupsCachingModel> _cachedNationalities;
        private List<LookupsCachingModel> _cachedProjectTypes;
        private List<LookupsCachingModel> _cachedCountries;
        private List<LookupsCachingModel> _cachedCities;
        private List<LookupsCachingModel> _cachedRegions;
        private List<LookupsCachingModel> _cachedNeighborhoods;
        private List<LookupsCachingModel> _cashedLeadSources;
        private List<LookupsCachingModel> _cashedLeadStatus;
        private List<LookupsCachingModel> _cashedLeadRating;
        private List<LookupsCachingModel> _cachedEmployeeAsLookup;
        private List<LookupsCachingModel> _cachedCustomerAsLookup;
        private List<LookupsCachingModel> _cachedLeadAsLookup;
        private List<LookupsCachingModel> _cachedDealAsLookup;
        private List<LookupsCachingModel> _cachedCallType;
        private List<LookupsCachingModel> _cachedCallStatus;
        private List<LookupsCachingModel> _cachedCallPurpose;
        private List<LookupsCachingModel> _cachedCallResult;
        private List<LookupsCachingModel> _cachedDealStage;
        private List<LookupsCachingModel> _cachedDealType;
        private List<LookupsCachingModel> _cachedTaskStatuses;
        private List<LookupsCachingModel> _cachedJobTitles;
        //private List<LookupsCachingModel> _cachedAdvisorsAsLookup;


        public LookupsService(ApplicationDbContext db, ApplicationDbContext context, IWebWorkContext workContext, ILanguageService languageService,
            ILoggerService logger, IHttpContextAccessor httpContextAccessor, ICacheService<List<LookupsCachingModel>> cacheService)
        {
            _context = context;
            
            _workContext = workContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cacheService = cacheService;
            _languageService = languageService;
        }

        #endregion

        public enum AddressType
        {
            country,
            city,
            region,
            neighborhood,
        }
        #region CP Methods

        #region Payment Type
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetSysPaymentType()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedPaymentTypes == null)
                    await SetCachedPaymentTypes();

                return _cachedPaymentTypes
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysPaymentType), ex);
                return null;
            }

        }

        private async Task SetCachedPaymentTypes()
        {
            _cachedPaymentTypes = await _cacheService.GetAsync("SysPaymentTypeCacheKey");
            if (_cachedPaymentTypes == null)
            {
                _cachedPaymentTypes = await _context.SysPaymentTypes.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.SysPaymentTypeTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.PaymentTypeName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("SysPaymentTypeCacheKey", _cachedPaymentTypes, TimeSpan.FromDays(30)).Wait();
            }
        }

        #endregion

        #region View Type
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetSysViewType()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedViewTypes == null)
                    await SetCachedViewTypes();

                return _cachedViewTypes
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysViewType), ex);
                return null;
            }

        }

        private async Task SetCachedViewTypes()
        {
            _cachedViewTypes = await _cacheService.GetAsync("SysViewTypeCacheKey");
            if (_cachedViewTypes == null)
            {
                _cachedViewTypes = await _context.SysViewTypes.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.SysViewTypeTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.ViewTypeName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("SysViewTypeCacheKey", _cachedViewTypes, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion

        #region Companies As Lookup
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetCompaniesAsLookup()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedCompaniesAsLookup == null)
                    await SetCachedCompaniesAsLookup();

                return _cachedCompaniesAsLookup
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompaniesAsLookup), ex);
                return null;
            }

        } 
        public async Task<List<ComboboxModel>> GetSalesAsLookup()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedSalesAsLookup == null)
                    await SetCachedSalesAsLookup();

                return _cachedSalesAsLookup
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSalesAsLookup), ex);
                return null;
            }

        }

        private async Task SetCachedCompaniesAsLookup()
        {
            _cachedCompaniesAsLookup = await _cacheService.GetAsync("GetCompaniesAsLookupCacheKey");
            if (_cachedCompaniesAsLookup == null)
            {
                _cachedCompaniesAsLookup = await _context.Companies.Where(c => !c.IsDeleted).Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.CompanyTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CompanyName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetCompaniesAsLookupCacheKey", _cachedCompaniesAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }

        private async Task SetCachedSalesAsLookup()
        {
            _cachedCompaniesAsLookup = await _cacheService.GetAsync("GetCompaniesAsLookupCacheKey");
            if (_cachedCompaniesAsLookup == null)
            {
                _cachedCompaniesAsLookup = await _context.sales.Where(c => !c.IsDeleted).Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.SalesTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.SalesName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetCompaniesAsLookupCacheKey", _cachedCompaniesAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion

        #region Company Categories
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetSysCompanyCategories()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedCompanyCategories == null)
                    await SetCachedCompanyCategories();

                return _cachedCompanyCategories
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysCompanyCategories), ex);
                return null;
            }


        }  
        private async Task SetCachedCompanyCategories()
        {
            _cachedCompanyCategories = await _cacheService.GetAsync("GetSysCompanyCategoriesCacheKey");
            if (_cachedCompanyCategories == null)
            {
                _cachedCompanyCategories = await _context.SysCompanyCategories.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.SysCompanyCategoryTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CompanyCategoryName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetSysCompanyCategoriesCacheKey", _cachedCompanyCategories, TimeSpan.FromDays(30)).Wait();
            }
        }      

        public async Task<List<ComboboxModel>> GetSyssSalesCategories()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedSalesCategories == null)
                    await SetCachedSalesCategories();

                return _cachedCompanyCategories
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSyssSalesCategories), ex);
                return null;
            }


        }

        private async Task SetCachedSalesCategories()
        {
            _cachedSalesCategories = await _cacheService.GetAsync("GetSysSalesCategoriesCacheKey");
            if (_cachedSalesCategories == null)
            {
                _cachedSalesCategories = await _context.SysSalesCategories.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.SysSalesCategoryTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.SalesCategoryName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetSysSalesCategoriesCacheKey", _cachedSalesCategories, TimeSpan.FromDays(30)).Wait();
            }
        } 

        #endregion
        
        #region Nationalities
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetSysNationalities()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedNationalities == null)
                    await SetCachedSysNationalities();

                return _cachedNationalities
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysNationalities), ex);
                return null;
            }

        }

        private async Task SetCachedSysNationalities()
        {
            _cachedNationalities = await _cacheService.GetAsync("GetSysNationalitiesCacheKey");
            if (_cachedNationalities == null)
            {
                _cachedNationalities = await _context.SysNationalities.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.SysNationalityTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.NationalityName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetSysNationalitiesCacheKey", _cachedNationalities, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion


        public async Task<dynamic> GetUsersAsLookup(int userType, string userId)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();


                var users = _context.Users.Where(a => a.UserType == (UserType)userType).Select(a => new
                {
                    Id = a.Id,
                    Name = a.UserName
                }).ToList();

                //users = users.Where(a => !_context.Employees.Any(e => e.UserId == a.Id && e.UserId != userId && !e.IsDeleted)).ToList();

                return users;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetUsersAsLookup), ex);
                return null;
            }

        }

        #region LeadSources
        public async Task<List<ComboboxModel>> GetLeadSources()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cashedLeadSources == null)
                    await SetCachedLeadSources();

                return _cashedLeadSources
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetLeadSources), ex);
                return null;
            }
        }

        private async Task SetCachedLeadSources()
        {
            _cashedLeadSources = await _cacheService.GetAsync("GetLeadSourcesCacheKey");
            if (_cashedLeadSources == null)
            {
                _cashedLeadSources = await _context.SysLeadSources.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.SourceName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetLeadSourcesCacheKey", _cashedLeadSources, TimeSpan.FromDays(30)).Wait();
            }
        }

        #endregion


        #region LeadStatus
        public async Task<List<ComboboxModel>> GetLeadStatus()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cashedLeadStatus == null)
                    await SetCachedLeadStatus();

                return _cashedLeadStatus
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetLeadStatus), ex);
                return null;
            }
        }

        private async Task SetCachedLeadStatus()
        {
            _cashedLeadStatus = await _cacheService.GetAsync("GetLeadStatusCacheKey");
            if (_cashedLeadStatus == null)
            {
                _cashedLeadStatus = await _context.SysLeadStatus.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.StatusName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetLeadStatusCacheKey", _cashedLeadStatus, TimeSpan.FromDays(30)).Wait();
            }
        }

        #endregion

        #region LeadRating
        public async Task<List<ComboboxModel>> GetLeadRating()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cashedLeadRating == null)
                    await SetCachedLeadRating();

                return _cashedLeadRating
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetLeadRating), ex);
                return null;
            }
        }

        private async Task SetCachedLeadRating()
        {
            _cashedLeadRating = await _cacheService.GetAsync("GetLeadRatingCacheKey");
            if (_cashedLeadRating == null)
            {
                _cashedLeadRating = await _context.SysLeadRatings.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.RateName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetLeadRatingCacheKey", _cashedLeadRating, TimeSpan.FromDays(30)).Wait();
            }
        }

        #endregion

        
        #endregion

        #region Address

        //----------------------------------------------------------------------------------------
        public async Task<FullAddressViewModel> GetAddressLookups()
        {
            await SetCachedAddress();
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var model = new FullAddressViewModel
                {
                    Countries = _cachedCountries.Select(a => new SysCountryDTO
                    {
                        Id = a.Id,
                        CountryName = a.Translates.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    }).ToList(),
                    Cities = _cachedCities.Select(a => new SysCityDTO
                    {
                        Id = a.Id,
                        CountryId = a.ParentId,
                        CityName = a.Translates.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    }).ToList(),
                    Regions = _cachedRegions.Select(a => new SysRegionDTO
                    {
                        Id = a.Id,
                        CityId = a.ParentId,
                        RegionName = a.Translates.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    }).ToList(),
                    Neighborhoods = _cachedNeighborhoods.Select(a => new SysNeighborhoodDTO
                    {
                        Id = a.Id,
                        RegionId = a.ParentId,
                        NeighborhoodName = a.Translates.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    }).ToList(),
                };
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAddressLookups), ex);
                return null;
            }

        }

        public async Task<List<ComboboxModel>> GetSysCountries()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedCountries == null)
                    await SetCachedCountries();

                return _cachedCountries
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysCountries), ex);
                return null;
            }

        }
        public async Task<List<ComboboxModel>> GetSysCity(string text = "")
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var query = _cachedCities;
                if (_cachedCities == null)
                    await SetCachedCities();
                
                return _cachedCities
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates.FirstOrDefault(x => x.LanguageId == langId).Name
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysCity), ex);
                return null;
            }

        }
        
        public async Task<List<AddressSearchComboboxModel>> GetSysCountries(string text = "")
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var res = _context.SysCountryTranslates.Where(ct => ct.CountryName.Contains(text) && ct.LanguageId == langId).OrderBy(ct => ct.CountryName.IndexOf(text)).Select(r => new AddressSearchComboboxModel
                {
                    Id = r.CountryId,
                    Name = r.CountryName,
                    Parents = "",
                    ItemType = ItemType.Country

                }).Take(10).ToList();

                return res;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysCountries), ex);
                return null;
            }

        }

        public async Task<List<AddressSearchComboboxModel>> GetSysCities(string text = "")
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var res = _context.SysCityTranslates.Where(ct => ct.CityName.Contains(text) && ct.LanguageId == langId).OrderBy(ct => ct.CityName.IndexOf(text)).Select(r => new AddressSearchComboboxModel
                {
                    Id = r.CityId,
                    Name = r.CityName,
                    Parents = _context.SysCountryTranslates.FirstOrDefault(c => c.LanguageId == langId && c.CountryId == r.City.CountryId).CountryName,
                    ItemType = ItemType.City

                }).Take(10).ToList();

                return res;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysCities), ex);
                return null;
            }

        }

        public async Task<List<AddressSearchComboboxModel>> GetSysRegions(string text = "")
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var res = _context.SysRegionTranslates.Where(rt => !rt.SysRegion.IsDeleted && rt.RegionName.Contains(text) && rt.LanguageId == langId).OrderBy(rt => rt.RegionName.IndexOf(text)).Select(r => new AddressSearchComboboxModel
                {
                    Id = r.RegionId,
                    Name = r.RegionName,
                    Parents = _context.SysCityTranslates.FirstOrDefault(c => c.CityId == r.SysRegion.CityId && c.LanguageId == r.LanguageId).CityName,
                    ItemType = ItemType.Region
                }).Take(10).ToList();

                return res;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysRegions), ex);
                return null;
            }

        }

        public async Task<List<AddressSearchComboboxModel>> GetSysNeighborhoods(string text = "")
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var res = _context.SysNeighborhoodTranslates.Where(nt => nt.NeighborhoodName.Contains(text) && nt.LanguageId == langId).OrderBy(nt => nt.NeighborhoodName.IndexOf(text)).Select(n => new AddressSearchComboboxModel
                {
                    Id = n.NeighborhoodId,
                    Name = n.NeighborhoodName,
                    Parents = _context.SysRegionTranslates.FirstOrDefault(r => r.LanguageId == langId && r.RegionId == n.SysNeighborhood.RegionId).RegionName,
                    ItemType = ItemType.Neighborhood

                }).Take(10).ToList();

                return res;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysNeighborhoods), ex);
                return null;
            }

        }

        public string GetAddressByNeighborhoodIdAllLanguages(int neighborhoodId)
        {
            SetCachedAddress().Wait();
            try
            {
                var neighborhood = _cachedNeighborhoods.Select(a => new { a.Translates, a.Id, a.ParentId }).FirstOrDefault(n => n.Id == neighborhoodId);
                var region = _cachedRegions.Select(a => new { a.Translates, a.Id, a.ParentId }).FirstOrDefault(n => n.Id == neighborhood.ParentId);
                var city = _cachedCities.Select(a => new { a.Translates, a.Id, a.ParentId }).FirstOrDefault(n => n.Id == region.ParentId);
                var country = _cachedCountries.Select(a => new { a.Translates, a.Id }).FirstOrDefault(n => n.Id == city.ParentId);

                string address = string.Join(" ", country.Translates.Select(t => t.Name).ToList());
                address += string.Join(" ", city.Translates.Select(t => t.Name).ToList());
                address += string.Join(" ", region.Translates.Select(t => t.Name).ToList());
                address += string.Join(" ", neighborhood.Translates.Select(t => t.Name).ToList());

                return address;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAddressByNeighborhoodIdAllLanguages), ex);
                return null;
            }
        }

        public string GetAddressByNeighborhoodId(int neighborhoodId)
        {
            SetCachedAddress().Wait();
            var langId = _languageService.GetLanguageIdFromRequestAsync().Result;
            try
            {
                var neighborhood = _cachedNeighborhoods.Select(a => new { a.Translates.FirstOrDefault(t => t.LanguageId == langId).Name, a.Id, a.ParentId }).FirstOrDefault(n => n.Id == neighborhoodId);
                var region = _cachedRegions.Select(a => new { a.Translates.FirstOrDefault(t => t.LanguageId == langId).Name, a.Id, a.ParentId }).FirstOrDefault(n => n.Id == neighborhood.ParentId);
                var city = _cachedCities.Select(a => new { a.Translates.FirstOrDefault(t => t.LanguageId == langId).Name, a.Id, a.ParentId }).FirstOrDefault(n => n.Id == region.ParentId);
                var country = _cachedCountries.Select(a => new { a.Translates.FirstOrDefault(t => t.LanguageId == langId).Name, a.Id }).FirstOrDefault(n => n.Id == city.ParentId);

                return neighborhood.Name +
                    region.Name +
                    city.Name +
                    country.Name;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAddressByNeighborhoodId), ex);
                return null;
            }
        }

        private async Task SetCachedAddress()
        {
            try
            {
                var langId = _languageService.GetLanguageIdFromRequestAsync().Result;

                if (_cachedCountries == null)
                    await SetCachedCountries();

                if (_cachedCities == null)
                    await SetCachedCities();

                if (_cachedRegions == null)
                    await SetCachedRegions();

                if (_cachedNeighborhoods == null)
                    await SetCachedNeighborhoods();

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SetCachedAddress), ex);
            }
        }

        private async Task SetCachedCountries()
        {
            _cachedCountries = await _cacheService.GetAsync("GetSysCountriesCacheKey");
            if (_cachedCountries == null)
            {
                _cachedCountries = await _context.SysCountries.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.SysCountryTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CountryName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetSysCountriesCacheKey", _cachedCountries, TimeSpan.FromDays(30)).Wait();
            }
        }

        private async Task SetCachedCities()
        {
            _cachedCities = await _cacheService.GetAsync("GetSysCitiesCacheKey");
            if (_cachedCities == null)
            {
                _cachedCities = await _context.SysCities.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    ParentId = a.CountryId,
                    Translates = a.SysCityTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CityName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetSysCitiesCacheKey", _cachedCities, TimeSpan.FromDays(30)).Wait();
            }
        }

        private async Task SetCachedRegions()
        {
            _cachedRegions = await _cacheService.GetAsync("GetSysRegionsCacheKey");
            if (_cachedRegions == null)
            {
                _cachedRegions = await _context.SysRegions.Where(r => !r.IsDeleted).Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    ParentId = a.CityId,
                    Translates = a.SysRegionTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.RegionName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetSysRegionsCacheKey", _cachedRegions, TimeSpan.FromDays(30)).Wait();
            }
        }

        private async Task SetCachedNeighborhoods()
        {
            _cachedNeighborhoods = await _cacheService.GetAsync("GetSysNeighborhoodsCacheKey");
            if (_cachedNeighborhoods == null)
            {
                _cachedNeighborhoods = await _context.SysNeighborhoods.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    ParentId = a.RegionId,
                    Translates = a.SysNeighborhoodTranslates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.NeighborhoodName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetSysNeighborhoodsCacheKey", _cachedNeighborhoods, TimeSpan.FromDays(30)).Wait();
            }
        }

        
        #endregion

        //----------------------------------------------------------------------------------------
        public List<ComboboxModel> GetSysWorkHours()
        {
            try
            {
                var ss = SysWorkHours.DefaultWorkHours.Select(a => new ComboboxModel { Id = a.Id, Name = a.TimeName }).ToList();
                return ss;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysWorkHours), ex);
                return null;
            }
        }

        #region Address Without Chaching 

        public async Task<List<ComboboxModel>> GetCountries()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                return await _context.SysCountries.Select(a => new ComboboxModel
                {
                    Id = a.Id,
                    Name = a.SysCountryTranslates.FirstOrDefault(x => x.LanguageId == langId).CountryName
                }).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<List<ComboboxModel>> GetCitiesByCountryId(int id)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                return await _context.SysCities.Where(c => c.CountryId == id).Select(a => new ComboboxModel
                {
                    Id = a.Id,
                    Name = a.SysCityTranslates.FirstOrDefault(x => x.LanguageId == langId).CityName
                }).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<List<ComboboxModel>> GetRegionsByCityId(int id)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                return await _context.SysRegions.Where(c => c.CityId == id).Select(a => new ComboboxModel
                {
                    Id = a.Id,
                    Name = a.SysRegionTranslates.FirstOrDefault(x => x.LanguageId == langId).RegionName
                }).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<List<NeighborhoodGroubingModel>> GetNeighborhoodsByRegionId(int id)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                return _context.SysNeighborhoods.Include(a => a.SysNeighborhoodTranslates).Where(c => c.RegionId == id).ToList().Select(a => new NeighborhoodGroubingModel
                {
                    Id = a.Id,
                    Semt = GetSubStringUntilOrEmpty(a.SysNeighborhoodTranslates?.FirstOrDefault(x => x.LanguageId == langId)?.NeighborhoodName ?? "", ","),
                    Name = a.SysNeighborhoodTranslates.FirstOrDefault(x => x.LanguageId == langId)?.NeighborhoodName.Split(",")[1]
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }

        private string GetSubStringUntilOrEmpty(string text, string stopAt = ",")
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(text))
                {
                    int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                    if (charLocation > 0)
                    {
                        return text.Substring(0, charLocation);
                    }
                }

                return String.Empty;
            }
            catch (Exception)
            {
                return null;
            }

        }
        //----------------------------------------------------------------------------------------------
        public async Task<AddressSearchComboboxModel> GetAddressByType(int id, ItemType type)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                switch (type)
                {
                    case ItemType.Country:
                        return await _context.SysCountryTranslates.Where(c => c.CountryId == id && c.LanguageId == langId).Select(a => new AddressSearchComboboxModel
                        {
                            Id = a.CountryId,
                            Name = a.CountryName,
                            ItemType = ItemType.Country
                        }).FirstOrDefaultAsync();
                     
                    case ItemType.City:
                        return await _context.SysCityTranslates.Where(c => c.CityId == id && c.LanguageId == langId).Select(a => new AddressSearchComboboxModel
                        {
                            Id = a.CityId,
                            Name = a.CityName,
                            ItemType = ItemType.City
                        }).FirstOrDefaultAsync();
                    case ItemType.Region:
                        return await _context.SysRegionTranslates.Where(c => c.RegionId == id && c.LanguageId == langId).Select(a => new AddressSearchComboboxModel
                        {
                            Id = a.RegionId,
                            Name = a.RegionName,
                            ItemType = ItemType.Region
                        }).FirstOrDefaultAsync();
                    case ItemType.Neighborhood:
                        return await _context.SysNeighborhoodTranslates.Where(c => c.NeighborhoodId == id && c.LanguageId == langId).Select(a => new AddressSearchComboboxModel
                        {
                            Id = a.NeighborhoodId,
                            Name = a.NeighborhoodName,
                             ItemType = ItemType.Neighborhood
                        }).FirstOrDefaultAsync();
                }
                return null;
              
            }
            catch (Exception)
            {
                return null;
            }

        }
       
        #endregion
        //---------------------------------------------------------------------------------------------
        #region CRM
        #region Employee As Lookup
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetEmployeeAsLookup()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedEmployeeAsLookup == null)
                    await SetCachedEmployeeAsLookup();

                return _cachedEmployeeAsLookup
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompaniesAsLookup), ex);
                return null;
            }

        }

        private async Task SetCachedEmployeeAsLookup()
        {
            _cachedEmployeeAsLookup = await _cacheService.GetAsync("GetEmployeeAsLookupCacheKey");
            if (_cachedEmployeeAsLookup == null)
            {
                _cachedEmployeeAsLookup = await _context.Employees.Where(c => !c.IsDeleted).Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Name = a.FullName,
                }).ToListAsync();

                _cacheService.SetAsync("GetEmployeeAsLookupCacheKey", _cachedEmployeeAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion

        //----------------------------------------------------------------------------------------
        #region Customer As Lookup
        public async Task<List<ComboboxModel>> GetCustomerAsLookup()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedCustomerAsLookup == null)
                    await SetCachedCustomerAsLookup();

                var x = _cachedCustomerAsLookup
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                    }).ToList() ?? null;
                return x;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompaniesAsLookup), ex);
                return null;
            }

        }
        private async Task SetCachedCustomerAsLookup()
        {
            _cachedCustomerAsLookup = await _cacheService.GetAsync("GetCutomerAsLookupCacheKey");
            if (_cachedCustomerAsLookup == null)
            {
                _cachedCustomerAsLookup = await _context.Customers.Where(c => !c.IsDeleted && !c.IsLead).Select(a => new LookupsCachingModel
                {
                    Id = Convert.ToInt32(a.Id),
                    Name = a.FullName,
                }).ToListAsync();

                _cacheService.SetAsync("GetCutomerAsLookupCacheKey", _cachedCustomerAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion
        //---------------------------------------------------------------------------------------------

        #region Leads As Lookup
        public async Task<List<ComboboxModel>> GetLeadsAsLookup()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedLeadAsLookup == null)
                    await SetCachedLeadsrAsLookup();

                return _cachedLeadAsLookup
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                    }).ToList() ?? null;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompaniesAsLookup), ex);
                return null;
            }

        }
        private async Task SetCachedLeadsrAsLookup()
        {
            _cachedLeadAsLookup = await _cacheService.GetAsync("GetLeadAsLookupCacheKey");
            if (_cachedLeadAsLookup == null)
            {
                _cachedLeadAsLookup = await _context.Customers.Where(c => !c.IsDeleted && c.IsLead).Select(a => new LookupsCachingModel
                {
                    Id = Convert.ToInt32(a.Id),
                    Name = a.FullName,
                }).ToListAsync();

                _cacheService.SetAsync("GetLeadAsLookupCacheKey", _cachedLeadAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion
        //---------------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetDealAsLookup()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedDealAsLookup == null)
                    await SetCachedDealAsLookup();

                return _cachedCustomerAsLookup
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                    }).ToList() ?? null;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompaniesAsLookup), ex);
                return null;
            }

        }
        private async Task SetCachedDealAsLookup()
        {
            _cachedDealAsLookup = await _cacheService.GetAsync("GetDealAsLookupCacheKey");
            if (_cachedDealAsLookup == null)
            {
                _cachedDealAsLookup = await _context.Deals.Where(c => !c.IsDeleted).Select(a => new LookupsCachingModel
                {
                    Id = Convert.ToInt32(a.Id),
                    Name = a.DealName,
                }).ToListAsync();

                _cacheService.SetAsync("GetDealAsLookupCacheKey", _cachedDealAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }


        public async Task<List<ComboboxModel>> GetDecisionAsLookup()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedDecisionAsLookup == null)
                { await SetCachedDecisionAsLookup(); }

                return _cachedDecisionAsLookup
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                    }).ToList() ?? null;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetDecisionAsLookup), ex);
                return null;
            }

        }
        private async Task SetCachedDecision()
        {
            _cachedDecisionAsLookup = await _cacheService.GetAsync("GetDecisionAsLookupCacheKey");
            if (_cachedDecisionAsLookup == null)
            {
                _cachedDecisionAsLookup = await _context.SysDecisions.Select(a => new LookupsCachingModel
                {
                    Id = Convert.ToInt32(a.Id),
                    Name = a.Translates.FirstOrDefault(s => s.Id == a.Id).DecisionName,
                }).ToListAsync();

                _cacheService.SetAsync("GetDecisionAsLookupCacheKey", _cachedDecisionAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }

        //---------------------------------------------------------------------------------------------
        #region Decision
        public async Task<List<ComboboxModel>> GetSysDecision()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedDecision == null)
                    await SetCachedDecision();

                return _cachedDecision
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysDecision), ex);
                return null;
            }
        }
        private async Task SetCachedDecisionAsLookup()
        {
            _cachedDecisionAsLookup = await _cacheService.GetAsync("GetDecisionAsLookupCacheKey");
            if (_cachedDecisionAsLookup == null)
            {
                _cachedDecisionAsLookup = await _context.SysDecisions.Select(a => new LookupsCachingModel
                {
                    Id = Convert.ToInt32(a.Id),
                    Name = a.Translates.FirstOrDefault(s => s.Id == a.Id).DecisionName,
                }).ToListAsync();

                _cacheService.SetAsync("GetDecisionAsLookupCacheKey", _cachedDecisionAsLookup, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion
        //---------------------------------------------------------------------------------------------
        #region Task Statuses
        public async Task<List<ComboboxModel>> GetSysTaskStatuses()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedTaskStatuses == null)
                    await SetCachedTaskStatuses();

                return _cachedTaskStatuses
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysTaskStatuses), ex);
                return null;
            }
        }

        private async Task SetCachedTaskStatuses()
        {
            _cachedTaskStatuses = await _cacheService.GetAsync("GetTaskStatusesCacheKey");
            if (_cachedTaskStatuses == null)
            {
                _cachedTaskStatuses = await _context.SysTaskStatus.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.StatusName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetTaskStatusesCacheKey", _cachedTaskStatuses, TimeSpan.FromDays(30)).Wait();
            }
        }

        #endregion

        //---------------------------------------------------------------------------------------------
        #region Job Title
        public async Task<List<ComboboxModel>> GetSysJobTitles()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                if (_cachedJobTitles == null)
                    await SetCachedJobTitles();

                return _cachedJobTitles
                    .Select(a => new ComboboxModel
                    {
                        Id = a.Id,
                        Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                    }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysJobTitles), ex);
                return null;
            }
        }

        private async Task SetCachedJobTitles()
        {
            _cachedJobTitles = await _cacheService.GetAsync("GetJobTitleCacheKey");
            if (_cachedJobTitles == null)
            {
                _cachedJobTitles = await _context.SysJobTitles.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.JobTitleName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetJobTitleCacheKey", _cachedJobTitles, TimeSpan.FromDays(30)).Wait();
            }
        }

        #endregion

        #endregion
        //---------------------------------------------------------------------------------------------
        #region Call
        public async Task<List<ComboboxModel>> GetCallType()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cachedCallType == null)
                    await SetCachedCallType();

                return _cachedCallType
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCallType), ex);
                return null;
            }
        }

        private async Task SetCachedCallType()
        {
            _cachedCallType = await _cacheService.GetAsync("GetCallTypeCacheKey");
            if (_cachedCallType == null)
            {
                _cachedCallType = await _context.SysCallTypes.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CallTypeName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetCallTypeCacheKey", _cachedCallType, TimeSpan.FromDays(30)).Wait();
            }
        }
        #endregion
        //---------------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetCallStatus()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cachedCallStatus == null)
                    await SetCachedCallStatus();

                return _cachedCallStatus
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCallStatus), ex);
                return null;
            }
        }

        private async Task SetCachedCallStatus()
        {
            _cachedCallStatus = await _cacheService.GetAsync("GetCallStatusCacheKey");
            if (_cachedCallStatus == null)
            {
                _cachedCallStatus = await _context.SysCallStatus.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CallStatusName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetCallStatusCacheKey", _cachedCallStatus, TimeSpan.FromDays(30)).Wait();
            }
        }
        //---------------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetCallPurpose()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cachedCallPurpose == null)
                    await SetCachedCallPurpose();

                return _cachedCallPurpose
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCallPurpose), ex);
                return null;
            }
        }

        private async Task SetCachedCallPurpose()
        {
            _cachedCallPurpose = await _cacheService.GetAsync("GetCallPurposeCacheKey");
            if (_cachedCallPurpose == null)
            {
                _cachedCallPurpose = await _context.SysCallPurposes.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CallPurposeName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetCallPurposeCacheKey", _cachedCallPurpose, TimeSpan.FromDays(30)).Wait();
            }
        }
        //---------------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetCallResult()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cachedCallResult == null)
                    await SetCachedCallResult();

                return _cachedCallResult
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCallResult), ex);
                return null;
            }
        }

        private async Task SetCachedCallResult()
        {
            _cachedCallResult = await _cacheService.GetAsync("GetCallResultCacheKey");
            if (_cachedCallResult == null)
            {
                _cachedCallResult = await _context.SysCallResults.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.CallResultName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetCallResultCacheKey", _cachedCallResult, TimeSpan.FromDays(30)).Wait();
            }
        }
        //---------------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetDealStage()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cachedDealStage == null)
                    await SetCachedDealStage();

                return _cachedDealStage
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetDealStage), ex);
                return null;
            }
        }

        private async Task SetCachedDealStage()
        {
            _cachedDealStage = await _cacheService.GetAsync("GetDealStageCacheKey");
            if (_cachedDealStage == null)
            {
                _cachedDealStage = await _context.SysDealStages.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.StageName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetDealStageCacheKey", _cachedDealStage, TimeSpan.FromDays(30)).Wait();
            }
        }

        //---------------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetDealType()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                if (_cachedDealType == null)
                    await SetCachedDealType();

                return _cachedDealType
                  .Select(a => new ComboboxModel
                  {
                      Id = a.Id,
                      Name = a.Translates?.FirstOrDefault(x => x.LanguageId == langId)?.Name ?? ""
                  }).ToList();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetDealType), ex);
                return null;
            }
        }

        private async Task SetCachedDealType()
        {
            _cachedDealType = await _cacheService.GetAsync("GetDealTypeCacheKey");
            if (_cachedDealType == null)
            {
                _cachedDealType = await _context.SysDealTypes.Select(a => new LookupsCachingModel
                {
                    Id = a.Id,
                    Translates = a.Translates.Select(t => new LookupsTranslateCachingModel
                    {
                        Id = t.Id,
                        Name = t.TypeName,
                        LanguageId = t.LanguageId
                    }).ToList()
                }).ToListAsync();

                _cacheService.SetAsync("GetDealTypeCacheKey", _cachedDealType, TimeSpan.FromDays(30)).Wait();
            }
        }

        Task<List<ComboboxModel>> ILookupsService.GetProjectsAsLookup()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetProjectsAsLookup(int CompId)
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysRealEstateTypesByClassId(int classId)
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysRealEstateTypes(int? languageId)
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysRealEstateClasses()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysRoomsCounts(int? languageId)
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetAdvisorsAsLookups()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysLowestMeterAvragePrice()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysHighestMeterAvragePrice()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysHighestRealEstateAvgPrice()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysLowestRealEstateAvgPrice()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysHighestProjectArea()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysLowestProjectArea()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysLowestRealEstatePrice()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysHighestRealEstatePrice()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysHighestRealEstateArea()
        {
            throw new NotImplementedException();
        }

        Task<float> ILookupsService.GetSysLowestRealEstateArea()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetRealEstatesAsLookup()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysHeatingTypes()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysAirConditionTypes()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysBlaconieDirection()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysFeatures(int? languageId)
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysProjectStatus(int? languageId)
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysPlacesTypes()
        {
            throw new NotImplementedException();
        }

        Task<List<ComboboxModel>> ILookupsService.GetSysProjectType()
        {
            throw new NotImplementedException();
        }

        public Task<List<ComboboxModel>> GetSysSalesCategories()
        {
            throw new NotImplementedException();
        }


        //---------------------------------------------------------------------------------------------



    }

}
