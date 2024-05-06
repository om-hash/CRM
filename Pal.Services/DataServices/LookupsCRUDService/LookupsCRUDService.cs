using AutoMapper;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Lookups;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Core.Enums.ActivityLog;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Lookups;
using Pal.Data.DTOs.Lookups;
using Pal.Services.Caching;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using Syncfusion.EJ2.Base;
using Syncfusion.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace Pal.Services.DataServices.LookupsCRUDService
{
    public class LookupsCRUDService : ILookupsCRUDService
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebWorkContext _workContext;
        private readonly ILocalizationService _localization;
        private readonly ILoggerService _logger;
        private readonly ILanguageService _languageService;
        private readonly ICacheService<List<LookupsCachingModel>> _cacheService;

        #endregion

        #region CTor

        public LookupsCRUDService(IMapper mapper, ApplicationDbContext context, ILocalizationService localization, IWebWorkContext workContext,
            ILoggerService logger, ICacheService<List<LookupsCachingModel>> cacheService, ILanguageService languageService)
        {
            _mapper = mapper;
            _context = context;
            _localization = localization;
            _workContext = workContext;
            _logger = logger;
            _cacheService = cacheService;
            _languageService = languageService;
        }

        #endregion

        #region GeneralForLookup
        public async Task<bool> RemoveAllLookupKeys()
        {
            try
            {
                var model = await _cacheService.RemoveLookUpKeys();
                if (model)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(RemoveAllLookupKeys), ex);
                return false;
            }
        }
        #endregion

        #region Nationality
        //------------------------------------------------------------------------------------
        public async Task<List<SysNationalityDTO>> NationalitiesList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysNationalityDTO> result = await _context.SysNationalities.Select(n => new SysNationalityDTO
                {
                    Id = n.Id,
                    NationalityName = n.SysNationalityTranslates.FirstOrDefault(a => a.LanguageId == langId).NationalityName,
                    Translates = n.SysNationalityTranslates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.NationalityName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NationalitiesList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysNationalityDTO> NationalityAdd(SysNationalityDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysNationalitiesCacheKey");
                var sysNationality = _mapper.Map<SysNationality>(model);
                var newId = await _context.SysNationalities.AnyAsync() ? await _context.SysNationalities.MaxAsync(a => a.Id) + 1 : 1;
                sysNationality.Id = newId;
                sysNationality.SysNationalityTranslates = model.Translates.Select(a => new SysNationalityTranslate
                {
                    NationalityId = newId,
                    NationalityName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(sysNationality);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = sysNationality.SysNationalityTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(NationalityAdd), ActionType = ActionType.Add, TransReferenceId = sysNationality.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NationalityAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysNationalityDTO> NationalityUpdate(SysNationalityDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysNationalitiesCacheKey");
                var SysNationality = _mapper.Map<SysNationality>(model);
                SysNationality.SysNationalityTranslates = model.Translates.Select(a => new SysNationalityTranslate
                {
                    Id = a.Id,
                    NationalityId = model.Id,
                    NationalityName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(SysNationality);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(NationalityUpdate), ActionType = ActionType.Update, TransReferenceId = SysNationality.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<SysNationalityDTO>(SysNationality);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NationalityUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> NationalityDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetSysNationalitiesCacheKey");
                if (await _context.Customers.AnyAsync(a => a.NationalityId == id))
                {
                    return false;
                }

                var model = await _context.SysNationalities.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(NationalityDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NationalityDelete), ex);
                return false;
            }
        }

        #endregion

        #region Payment Types
        //------------------------------------------------------------------------------------
        public async Task<List<SysPaymentTypesDTO>> PaymentTypesList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysPaymentTypesDTO> result = await _context.SysPaymentTypes.Select(n => new SysPaymentTypesDTO
                {
                    Id = n.Id,
                    PaymentTypeName = n.SysPaymentTypeTranslates.FirstOrDefault(a => a.LanguageId == langId).PaymentTypeName,
                    Translates = n.SysPaymentTypeTranslates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.PaymentTypeName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(PaymentTypesList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysPaymentTypesDTO> PaymentTypesAdd(SysPaymentTypesDTO model)
        {
            try
            {
                _cacheService.Delete("SysPaymentTypeCacheKey");
                var sysPaymentType = _mapper.Map<SysPaymentType>(model);
                var newId = await _context.SysPaymentTypes.AnyAsync() ? await _context.SysPaymentTypes.MaxAsync(a => a.Id) + 1 : 1;
                sysPaymentType.Id = newId;
                sysPaymentType.SysPaymentTypeTranslates = model.Translates.Select(a => new SysPaymentTypeTranslate
                {
                    PaymentTypeId = newId,
                    PaymentTypeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(sysPaymentType);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = sysPaymentType.SysPaymentTypeTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(PaymentTypesAdd), ActionType = ActionType.Add, TransReferenceId = sysPaymentType.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(PaymentTypesAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysPaymentTypesDTO> PaymentTypesUpdate(SysPaymentTypesDTO model)
        {
            try
            {
                _cacheService.Delete("SysPaymentTypeCacheKey");
                var SysPaymentType = _mapper.Map<SysPaymentType>(model);
                SysPaymentType.SysPaymentTypeTranslates = model.Translates.Select(a => new SysPaymentTypeTranslate
                {
                    Id = a.Id,
                    PaymentTypeId = model.Id,
                    PaymentTypeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(SysPaymentType);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(PaymentTypesUpdate), ActionType = ActionType.Update, TransReferenceId = SysPaymentType.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<SysPaymentTypesDTO>(SysPaymentType);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(PaymentTypesUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> PaymentTypesDelete(int id)
        {
            try
            {
                _cacheService.Delete("SysPaymentTypeCacheKey");
                //if (await _context.Projects.AnyAsync(a => a.ProjectTypeId == id))
                //{
                //    return false;
                //}

                var model = await _context.SysPaymentTypes.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(PaymentTypesDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(PaymentTypesDelete), ex);
                return false;
            }
        }

        #endregion

        #region Country
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetSysCountries()
        {
            var langId = await _languageService.GetLanguageIdFromRequestAsync();
            return await _context.SysCountries.Select(x => new ComboboxModel
            {
                Id = x.Id,
                Name = x.SysCountryTranslates.FirstOrDefault(x => x.LanguageId == langId).CountryName
            }).ToListAsync();
        }

        //------------------------------------------------------------------------------------
        public async Task<List<SysCountryDTO>> CountryList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysCountryDTO> result = await _context.SysCountries.Select(a => new SysCountryDTO
                {
                    Id = a.Id,
                    CountryName = a.SysCountryTranslates.FirstOrDefault(a => a.LanguageId == langId).CountryName,
                    Translates = a.SysCountryTranslates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.CountryName
                    }).ToList()
                }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CountryList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysCountryDTO> CountryAdd(SysCountryDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysCountriesCacheKey");
                var newId = await _context.SysCountries.AnyAsync() ? await _context.SysCountries.MaxAsync(a => a.Id) + 1 : 1;
                model.Id = newId;
                SysCountry country = _mapper.Map<SysCountry>(model);
                country.SysCountryTranslates = model.Translates.Select(t => new SysCountryTranslate
                {
                    CountryId = newId,
                    CountryName = t.Name,
                    LanguageId = t.LanguageId
                }).ToList();
                await _context.AddAsync(country);
                await _context.SaveChangesAsync();
                model.Translates.ForEach(t =>
                {
                    t.Id = country.SysCountryTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CountryAdd), ActionType = ActionType.Add, TransReferenceId = country.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CountryAdd), ex);

                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysCountryDTO> CountryUpdate(SysCountryDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysCountriesCacheKey");
                var sysCountry = _mapper.Map<SysCountry>(model);
                sysCountry.SysCountryTranslates = model.Translates.Select(a => new SysCountryTranslate
                {
                    Id = a.Id,
                    CountryId = model.Id,
                    CountryName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(sysCountry);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CountryUpdate), ActionType = ActionType.Update, TransReferenceId = sysCountry.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<SysCountryDTO>(sysCountry);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CountryUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> CountryDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetSysCountriesCacheKey");
                if (await _context.SysCities.AnyAsync(a => a.CountryId == id) || await _context.Customers.AnyAsync(a => a.CountryId == id))
                    return false;


                var model = await _context.SysCountries.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CountryDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CountryDelete), ex);

                return false;
            }
        }
        #endregion

        #region City

        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetSysCities(int countryId)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var result = await _context.SysCities.Where(c => c.CountryId == countryId).Select(x => new ComboboxModel
                {
                    Id = x.Id,
                    Name = x.SysCityTranslates.FirstOrDefault(x => x.LanguageId == langId).CityName
                }).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysCities), ex);
                return null;
            }

        }

        //------------------------------------------------------------------------------------
        public async Task<List<SysCityDTO>> CityList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysCityDTO> result = await _context.SysCities.Select(a => new SysCityDTO
                {
                    Id = a.Id,
                    CityName = a.SysCityTranslates.FirstOrDefault(a => a.LanguageId == langId).CityName,
                    CountryId = a.CountryId,
                    CountryName = a.SysCountry.SysCountryTranslates.FirstOrDefault(a => a.LanguageId == langId).CountryName,
                    Translates = a.SysCityTranslates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.CityName,
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CityList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysCityDTO> CityAdd(SysCityDTO model)
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                _cacheService.Delete("GetSysCitiesCacheKey");
                var newId = await _context.SysCities.AnyAsync() ? await _context.SysCities.MaxAsync(a => a.Id) + 1 : 1;
                model.Id = newId;
                SysCity sysCity = _mapper.Map<SysCity>(model);
                sysCity.SysCityTranslates = model.Translates.Select(t => new SysCityTranslate
                {
                    CityId = newId,
                    CityName = t.Name,
                    LanguageId = t.LanguageId
                }).ToList();
                await _context.AddAsync(sysCity);
                await _context.SaveChangesAsync();
                model.CountryName = _context.SysCountries.Include(c => c.SysCountryTranslates).FirstOrDefault(c => c.Id == model.CountryId)
                                    .SysCountryTranslates.FirstOrDefault(t => t.LanguageId == langId).CountryName;
                model.Translates.ForEach(t =>
                {
                    t.Id = sysCity.SysCityTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CityAdd), ActionType = ActionType.Add, TransReferenceId = sysCity.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CityAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysCityDTO> CityUpdate(SysCityDTO model)
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                _cacheService.Delete("GetSysCitiesCacheKey");
                var sysCity = _mapper.Map<SysCity>(model);
                sysCity.SysCityTranslates = model.Translates.Select(a => new SysCityTranslate
                {
                    Id = a.Id,
                    CityId = model.Id,
                    CityName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(sysCity);
                await _context.SaveChangesAsync();
                model.CountryName = _context.SysCountries.Include(c => c.SysCountryTranslates).FirstOrDefault(c => c.Id == model.CountryId)
                                    .SysCountryTranslates.FirstOrDefault(t => t.LanguageId == langId).CountryName;
                model.Translates.ForEach(t =>
                {
                    t.Id = sysCity.SysCityTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CityUpdate), ActionType = ActionType.Update, TransReferenceId = sysCity.Id.ToString(), TransType = LogTransType.Lookups });
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CityUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> CityDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetSysCitiesCacheKey");
                if (await _context.SysRegions.AnyAsync(a => a.CityId == id) || await _context.Companies.AnyAsync(a => a.CityId == id))
                    return false;


                var model = await _context.SysCities.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CityDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CityDelete), ex);
                return false;
            }
        }
        #endregion

        #region Region
        //----------------------------------------------------------------------------------------
        public async Task<List<ComboboxModel>> GetSysRegions()
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                return await _context.SysRegions.Select(x => new ComboboxModel
                {
                    Id = x.Id,
                    Name = x.SysRegionTranslates.FirstOrDefault(x => x.LanguageId == langId).RegionName
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSysRegions), ex);
                return null;
            }

        }

        //------------------------------------------------------------------------------------
        public async Task<SyncPaginatedListModel<SysRegionDTO>> RegionList(DataManagerRequest dm)
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {

                var query = _context.SysRegions.AsQueryable().AsNoTracking().Select(a => new SysRegionDTO
                {
                    Id = a.Id,
                    RegionName = a.SysRegionTranslates.FirstOrDefault(a => a.LanguageId == langId).RegionName,
                    CountryId = a.CountryId,
                    CountryName = a.SysCity.SysCountry.SysCountryTranslates.FirstOrDefault(c => c.LanguageId == langId).CountryName,
                    CityId = a.CityId,
                    CityName = a.SysCity.SysCityTranslates.FirstOrDefault(a => a.LanguageId == langId).CityName,
                    Translates = a.SysRegionTranslates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.RegionName,
                    }).ToList()
                });

                return await SyncGridOperations<SysRegionDTO>.PagingAndFilterAsync(query, dm);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(RegionList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysRegionDTO> RegionAdd(SysRegionDTO model)
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                _cacheService.Delete("GetSysRegionsCacheKey");
                var newId = await _context.SysRegions.AnyAsync() ? await _context.SysRegions.MaxAsync(a => a.Id) + 1 : 1;
                model.Id = newId;
                SysRegion sysRegion = _mapper.Map<SysRegion>(model);
                sysRegion.SysRegionTranslates = model.Translates.Select(t => new SysRegionTranslate
                {
                    RegionId = newId,
                    RegionName = t.Name,
                    LanguageId = t.LanguageId
                }).ToList();
                await _context.AddAsync(sysRegion);
                await _context.SaveChangesAsync();
                model.CityName = _context.SysCities.Include(c => c.SysCityTranslates).FirstOrDefault(c => c.Id == model.CityId)
                                    .SysCityTranslates.FirstOrDefault(t => t.LanguageId == langId).CityName;
                model.CountryName = _context.SysCountries.Include(c => c.SysCountryTranslates).FirstOrDefault(c => c.Id == model.CountryId)
                                    .SysCountryTranslates.FirstOrDefault(t => t.LanguageId == langId).CountryName;

                model.Translates.ForEach(t =>
                {
                    t.Id = sysRegion.SysRegionTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(RegionAdd), ActionType = ActionType.Add, TransReferenceId = sysRegion.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(RegionAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysRegionDTO> RegionUpdate(SysRegionDTO model)
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                _cacheService.Delete("GetSysRegionsCacheKey");
                var sysRegion = _mapper.Map<SysRegion>(model);
                sysRegion.SysRegionTranslates = model.Translates.Select(a => new SysRegionTranslate
                {
                    Id = a.Id,
                    RegionId = model.Id,
                    RegionName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(sysRegion);
                await _context.SaveChangesAsync();
                model.CityName = _context.SysCities.Include(c => c.SysCityTranslates).FirstOrDefault(c => c.Id == model.CityId)
                                    .SysCityTranslates.FirstOrDefault(t => t.LanguageId == langId).CityName;
                model.CountryName = _context.SysCountries.Include(c => c.SysCountryTranslates).FirstOrDefault(c => c.Id == model.CountryId)
                                    .SysCountryTranslates.FirstOrDefault(t => t.LanguageId == langId).CountryName;
                model.Translates.ForEach(t =>
                {
                    t.Id = sysRegion.SysRegionTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(RegionUpdate), ActionType = ActionType.Update, TransReferenceId = sysRegion.Id.ToString(), TransType = LogTransType.Lookups });
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(RegionUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> RegionDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetSysRegionsCacheKey");
                if (await _context.SysNeighborhoods.AnyAsync(a => a.RegionId == id))
                    return false;


                var model = await _context.SysRegions.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(RegionDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(RegionDelete), ex);
                return false;
            }
        }
        #endregion

        #region Neighborhood

        //------------------------------------------------------------------------------------
        public async Task<SyncPaginatedListModel<SysNeighborhoodDTO>> NeighborhoodList(DataManagerRequest dm)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();

                var result = _context.SysNeighborhoods.AsQueryable().AsNoTracking()
                    .Select(a => new SysNeighborhoodDTO
                    {
                        Id = a.Id,
                        NeighborhoodName = a.SysNeighborhoodTranslates.FirstOrDefault(a => a.LanguageId == langId).NeighborhoodName,
                        RegionId = a.RegionId,
                        RegionName = a.SysRegion.SysRegionTranslates.FirstOrDefault(a => a.LanguageId == langId).RegionName,
                        CityId = a.SysRegion.SysCity.Id,
                        CountryId = a.SysRegion.CountryId,
                        CountryName = a.SysRegion.SysCity.SysCountry.SysCountryTranslates.FirstOrDefault(c => c.LanguageId == langId).CountryName,
                        CityName = a.SysRegion.SysCity.SysCityTranslates.FirstOrDefault(c => c.LanguageId == langId).CityName,
                        Translates = a.SysNeighborhoodTranslates.Select(t => new ComboboxModelTranslate
                        {
                            Id = t.Id,
                            LanguageId = t.LanguageId,
                            Name = t.NeighborhoodName,
                        }).ToList()
                    });

                return await SyncGridOperations<SysNeighborhoodDTO>.PagingAndFilterAsync(result, dm);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NeighborhoodList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysNeighborhoodDTO> NeighborhoodAdd(SysNeighborhoodDTO model)
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                _cacheService.Delete("GetSysNeighborhoodsCacheKey");
                var newId = await _context.SysNeighborhoods.AnyAsync() ? await _context.SysNeighborhoods.MaxAsync(a => a.Id) + 1 : 1;
                model.Id = newId;
                SysNeighborhood sysNeighborhood = _mapper.Map<SysNeighborhood>(model);
                sysNeighborhood.SysNeighborhoodTranslates = model.Translates.Select(t => new SysNeighborhoodTranslate
                {
                    NeighborhoodId = newId,
                    NeighborhoodName = t.Name,
                    LanguageId = t.LanguageId
                }).ToList();
                await _context.AddAsync(sysNeighborhood);
                await _context.SaveChangesAsync();
                model.RegionName = _context.SysRegions.Include(c => c.SysRegionTranslates).FirstOrDefault(c => c.Id == model.RegionId)
                                    .SysRegionTranslates.FirstOrDefault(t => t.LanguageId == langId).RegionName;
                model.CountryName = _context.SysCountries.Include(c => c.SysCountryTranslates).FirstOrDefault(c => c.Id == model.CountryId)
                                    .SysCountryTranslates.FirstOrDefault(t => t.LanguageId == langId).CountryName;
                model.CityName = _context.SysCities.Include(c => c.SysCityTranslates).FirstOrDefault(c => c.Id == model.CityId)
                                                    .SysCityTranslates.FirstOrDefault(t => t.LanguageId == langId).CityName;

                model.Translates.ForEach(t =>
                {
                    t.Id = sysNeighborhood.SysNeighborhoodTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(NeighborhoodAdd), ActionType = ActionType.Add, TransReferenceId = sysNeighborhood.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NeighborhoodAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysNeighborhoodDTO> NeighborhoodUpdate(SysNeighborhoodDTO model)
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                _cacheService.Delete("GetSysNeighborhoodsCacheKey");
                var sysNeighborhood = _mapper.Map<SysNeighborhood>(model);
                sysNeighborhood.SysNeighborhoodTranslates = model.Translates.Select(a => new SysNeighborhoodTranslate
                {
                    Id = a.Id,
                    NeighborhoodId = model.Id,
                    NeighborhoodName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(sysNeighborhood);
                await _context.SaveChangesAsync();
                model.RegionName = _context.SysRegions.Include(c => c.SysRegionTranslates).FirstOrDefault(c => c.Id == model.RegionId)
                                    .SysRegionTranslates.FirstOrDefault(t => t.LanguageId == langId).RegionName;
                model.CountryName = _context.SysCountries.Include(c => c.SysCountryTranslates).FirstOrDefault(c => c.Id == model.CountryId)
                                    .SysCountryTranslates.FirstOrDefault(t => t.LanguageId == langId).CountryName;
                model.CityName = _context.SysCities.Include(c => c.SysCityTranslates).FirstOrDefault(c => c.Id == model.CityId)
                                                    .SysCityTranslates.FirstOrDefault(t => t.LanguageId == langId).CityName;

                model.Translates.ForEach(t =>
                {
                    t.Id = sysNeighborhood.SysNeighborhoodTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(NeighborhoodUpdate), ActionType = ActionType.Update, TransReferenceId = sysNeighborhood.Id.ToString(), TransType = LogTransType.Lookups });
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NeighborhoodUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> NeighborhoodDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetSysNeighborhoodsCacheKey");
                //if (await _context.Projects.AnyAsync(a => a.NeighborhoodId == id) || await _context.RealEstates.AnyAsync(a => a.NeighborhoodId == id))
                //    return false;


                var model = await _context.SysNeighborhoods.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(NeighborhoodDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(NeighborhoodDelete), ex);
                return false;
            }
        }
        #endregion

        #region Company Category 

        public async Task<SysCompanyCategoryDTO> CompanyCategoryAdd(SysCompanyCategoryDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysCompanyCategoriesCacheKey");
                var SysCateogry = _mapper.Map<SysCompanyCategory>(model);
                var newId = await _context.SysCompanyCategories.AnyAsync() ? await _context.SysCompanyCategories.MaxAsync(a => a.Id) + 1 : 1;
                SysCateogry.Id = newId;
                SysCateogry.SysCompanyCategoryTranslates = model.Translates.Select(a => new SysCompanyCategoryTranslate
                {
                    CompanyCategoryId = newId,
                    CompanyCategoryName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(SysCateogry);
                await _context.SaveChangesAsync();

                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = SysCateogry.SysCompanyCategoryTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CompanyCategoryAdd), ActionType = ActionType.Add, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CompanyCategoryAdd), ex);
                return null;
            }
        }
        public async Task<bool> CompanyCategoryDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetSysCompanyCategoriesCacheKey");
                if (await _context.Companies.AnyAsync(a => a.CompanyCategoryId == id))
                {
                    return false;
                }

                var model = await _context.SysCompanyCategories.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CompanyCategoryDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CompanyCategoryDelete), ex);
                return false;
            }
        }
        public async Task<List<SysCompanyCategoryDTO>> CompanyCategoryList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysCompanyCategoryDTO> result = await _context.SysCompanyCategories.Select(a => new SysCompanyCategoryDTO
                {
                    Id = a.Id,
                    CompanyCategoryName = a.SysCompanyCategoryTranslates.FirstOrDefault(a => a.LanguageId == langId).CompanyCategoryName,
                    Translates = a.SysCompanyCategoryTranslates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.CompanyCategoryName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CompanyCategoryList), ex);
                return null;
            }
        }
        public async Task<SysCompanyCategoryDTO> CompanyCategoryUpdate(SysCompanyCategoryDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysCompanyCategoriesCacheKey");
                var SysCateogry = _mapper.Map<SysCompanyCategory>(model);
                SysCateogry.SysCompanyCategoryTranslates = model.Translates.Select(a => new SysCompanyCategoryTranslate
                {
                    Id = a.Id,
                    CompanyCategoryId = model.Id,
                    CompanyCategoryName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(SysCateogry);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CompanyCategoryUpdate), ActionType = ActionType.Update, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<SysCompanyCategoryDTO>(SysCateogry);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CompanyCategoryUpdate), ex);
                return null;
            }
        }

        #endregion


        #region Sales Category 

        public async Task<SysSalesCategoryDTO> SalesCategoryAdd(SysSalesCategoryDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysSalesCategoriesCacheKey");
                var SysCateogry = _mapper.Map<SysSalesCategory>(model);
                var newId = await _context.SysSalesCategories.AnyAsync() ? await _context.SysSalesCategories.MaxAsync(a => a.Id) + 1 : 1;
                SysCateogry.Id = newId;
                SysCateogry.SysSalesCategoryTranslates = model.Translates.Select(a => new SysSalesCategoryTranslate
                {
                    SalesCategoryId = newId,
                    SalesCategoryName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(SysCateogry);
                await _context.SaveChangesAsync();

                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = SysCateogry.SysSalesCategoryTranslates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(SalesCategoryAdd), ActionType = ActionType.Add, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SalesCategoryAdd), ex);
                return null;
            }
        }
        public async Task<bool> SalesCategoryDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetSysSalesCategoriesCacheKey");
                if (await _context.sales.AnyAsync(a => a.SalesCategoryId == id))
                {
                    return false;
                }

                var model = await _context.SysSalesCategories.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(SalesCategoryDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SalesCategoryDelete), ex);
                return false;
            }
        }
        public async Task<List<SysSalesCategoryDTO>> SalesCategoryList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysSalesCategoryDTO> result = await _context.SysSalesCategories.Select(a => new SysSalesCategoryDTO
                {
                    Id = a.Id,
                    SalesCategoryName = a.SysSalesCategoryTranslates.FirstOrDefault(a => a.LanguageId == langId).SalesCategoryName,
                    Translates = a.SysSalesCategoryTranslates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.SalesCategoryName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SalesCategoryList), ex);
                return null;
            }
        }
        public async Task<SysSalesCategoryDTO> SalesCategoryUpdate(SysSalesCategoryDTO model)
        {
            try
            {
                _cacheService.Delete("GetSysSalesCategoriesCacheKey");
                var SysCateogry = _mapper.Map<SysSalesCategory>(model);
                SysCateogry.SysSalesCategoryTranslates = model.Translates.Select(a => new SysSalesCategoryTranslate
                {
                    Id = a.Id,
                    SalesCategoryId = model.Id,
                    SalesCategoryName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(SysCateogry);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(SalesCategoryUpdate), ActionType = ActionType.Update, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<SysSalesCategoryDTO>(SysCateogry);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SalesCategoryUpdate), ex);
                return null;
            }
        }

        #endregion

        #region CRM

        #region TaskStatus

        public async Task<SysTaskStatusDTO> TaskStatusAdd(SysTaskStatusDTO model)
        {
            try
            {
                _cacheService.Delete("GetTaskStatusesCacheKey");

                SysTaskStatus sysTaskStatus = new SysTaskStatus();
                var newId = await _context.SysTaskStatus.AnyAsync() ? await _context.SysTaskStatus.MaxAsync(a => a.Id) + 1 : 1;
                sysTaskStatus.Id = newId;
                sysTaskStatus.Translates = model.Translates.Select(a => new SysTaskStatusTranslate
                {
                    TaskStatusId = newId,
                    StatusName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();

                await _context.AddAsync(sysTaskStatus);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = sysTaskStatus.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(TaskStatusAdd), ActionType = ActionType.Add, TransReferenceId = sysTaskStatus.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(TaskStatusAdd), ex);
                return null;
            }
        }

        public async Task<bool> TaskStatusDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetTaskStatusesCacheKey");
                if (await _context.Tasks.AnyAsync(a => a.StatusId == id))
                {
                    return false;
                }

                var model = await _context.SysTaskStatus.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(TaskStatusDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(TaskStatusDelete), ex);
                return false;
            }
        }

        public async Task<List<SysTaskStatusDTO>> TaskStatusList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysTaskStatusDTO> result = await _context.SysTaskStatus.Select(n => new SysTaskStatusDTO
                {
                    Id = n.Id,
                    StatusName = n.Translates.FirstOrDefault(a => a.LanguageId == langId).StatusName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.StatusName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(TaskStatusList), ex);
                return null;
            }
        }

        public async Task<SysTaskStatusDTO> TaskStatusUpdate(SysTaskStatusDTO model)
        {
            try
            {
                _cacheService.Delete("GetTaskStatusesCacheKey");
                SysTaskStatus sysTaskStatus = new SysTaskStatus();
                sysTaskStatus.Id = model.Id;
                sysTaskStatus.Translates = model.Translates.Select(a => new SysTaskStatusTranslate
                {
                    Id = a.Id,
                    TaskStatusId = model.Id,
                    StatusName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();

                _context.Update(sysTaskStatus);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(TaskStatusUpdate), ActionType = ActionType.Update, TransReferenceId = sysTaskStatus.Id.ToString(), TransType = LogTransType.Lookups });
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(TaskStatusUpdate), ex);
                return null;
            }
        }

        #endregion

        #region CallType
        //------------------------------------------------------------------------------------
        public async Task<List<CallTypeDTO>> CallTypeList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<CallTypeDTO> result = await _context.SysCallTypes.Select(n => new CallTypeDTO
                {
                    Id = n.Id,
                    CallType = n.Translates.FirstOrDefault(a => a.LanguageId == langId).CallTypeName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.CallTypeName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallTypeList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallTypeDTO> CallTypeAdd(CallTypeDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallTypeCacheKey");
                var callType = _mapper.Map<SysCallType>(model);
                var newId = await _context.SysCallTypes.AnyAsync() ? await _context.SysCallTypes.MaxAsync(a => a.Id) + 1 : 1;
                callType.Id = newId;
                callType.Translates = model.Translates.Select(a => new SysCallTypeTranslate
                {
                    CallTypeId = newId,
                    CallTypeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(callType);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = callType.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallTypeAdd), ActionType = ActionType.Add, TransReferenceId = callType.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallTypeAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallTypeDTO> CallTypeUpdate(CallTypeDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallTypeCacheKey");
                var callType = _mapper.Map<SysCallType>(model);
                callType.Translates = model.Translates.Select(a => new SysCallTypeTranslate
                {
                    Id = a.Id,
                    CallTypeId = model.Id,
                    CallTypeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(callType);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallTypeUpdate), ActionType = ActionType.Update, TransReferenceId = callType.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<CallTypeDTO>(callType);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallTypeUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> CallTypeDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetCallTypeCacheKey");
                if (await _context.Calls.AnyAsync(a => a.CallTypeId == id))
                {
                    return false;
                }

                var model = await _context.SysCallTypes.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallTypeDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallTypeDelete), ex);
                return false;
            }
        }

        #endregion

        #region CallStatus
        //------------------------------------------------------------------------------------
        public async Task<List<CallStatusDTO>> CallStatusList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<CallStatusDTO> result = await _context.SysCallStatus.Select(n => new CallStatusDTO
                {
                    Id = n.Id,
                    CallStatusName = n.Translates.FirstOrDefault(a => a.LanguageId == langId).CallStatusName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.CallStatusName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallStatusList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallStatusDTO> CallStatusAdd(CallStatusDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallStatusCacheKey");
                var callType = _mapper.Map<SysCallStatus>(model);
                var newId = await _context.SysCallStatus.AnyAsync() ? await _context.SysCallStatus.MaxAsync(a => a.Id) + 1 : 1;
                callType.Id = newId;
                callType.Translates = model.Translates.Select(a => new SysCallStatusTranslate
                {
                    CallStatusId = newId,
                    CallStatusName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(callType);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = callType.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallStatusAdd), ActionType = ActionType.Add, TransReferenceId = callType.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallTypeAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallStatusDTO> CallStatusUpdate(CallStatusDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallStatusCacheKey");
                var callType = _mapper.Map<SysCallStatus>(model);
                callType.Translates = model.Translates.Select(a => new SysCallStatusTranslate
                {
                    Id = a.Id,
                    CallStatusId = model.Id,
                    CallStatusName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(callType);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallTypeUpdate), ActionType = ActionType.Update, TransReferenceId = callType.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<CallStatusDTO>(callType);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallStatusUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> CallStatusDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetCallStatusCacheKey");
                if (await _context.Calls.AnyAsync(a => a.CallStatusId == id))
                {
                    return false;
                }

                var model = await _context.SysCallStatus.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallStatusDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallTypeDelete), ex);
                return false;
            }
        }

        #endregion

        #region CallPurpose
        //------------------------------------------------------------------------------------
        public async Task<List<CallPurposeDTO>> CallPurposeList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<CallPurposeDTO> result = await _context.SysCallPurposes.Select(n => new CallPurposeDTO
                {
                    Id = n.Id,
                    CallPurpose = n.Translates.FirstOrDefault(a => a.LanguageId == langId).CallPurposeName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.CallPurposeName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallPurposeList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallPurposeDTO> CallPurposeAdd(CallPurposeDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallPurposeCacheKey");
                var callPurpose = _mapper.Map<SysCallPurpose>(model);
                var newId = await _context.SysCallPurposes.AnyAsync() ? await _context.SysCallPurposes.MaxAsync(a => a.Id) + 1 : 1;
                callPurpose.Id = newId;
                callPurpose.Translates = model.Translates.Select(a => new SysCallPurposeTranslate
                {
                    CallPurposeId = newId,
                    CallPurposeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(callPurpose);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = callPurpose.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallPurposeAdd), ActionType = ActionType.Add, TransReferenceId = callPurpose.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallPurposeAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallPurposeDTO> CallPurposeUpdate(CallPurposeDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallPurposeCacheKey");
                var callPurpose = _mapper.Map<SysCallPurpose>(model);
                callPurpose.Translates = model.Translates.Select(a => new SysCallPurposeTranslate
                {
                    Id = a.Id,
                    CallPurposeId = model.Id,
                    CallPurposeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(callPurpose);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallPurposeUpdate), ActionType = ActionType.Update, TransReferenceId = callPurpose.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<CallPurposeDTO>(callPurpose);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallStatusUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> CallPurposeDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetCallPurposeCacheKey");
                if (await _context.Calls.AnyAsync(a => a.CallPurposeId == id))
                {
                    return false;
                }

                var model = await _context.SysCallPurposes.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallPurposeDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallPurposeDelete), ex);
                return false;
            }
        }

        #endregion

        #region CallResult
        //------------------------------------------------------------------------------------
        public async Task<List<CallResultDTO>> CallResultList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<CallResultDTO> result = await _context.SysCallResults.Select(n => new CallResultDTO
                {
                    Id = n.Id,
                    CallResult = n.Translates.FirstOrDefault(a => a.LanguageId == langId).CallResultName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.CallResultName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallResultList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallResultDTO> CallResultAdd(CallResultDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallResultCacheKey");
                var callPurpose = _mapper.Map<SysCallResult>(model);
                var newId = await _context.SysCallResults.AnyAsync() ? await _context.SysCallResults.MaxAsync(a => a.Id) + 1 : 1;
                callPurpose.Id = newId;
                callPurpose.Translates = model.Translates.Select(a => new SysCallResultTranslate
                {
                    CallResultId = newId,
                    CallResultName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(callPurpose);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = callPurpose.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallResultAdd), ActionType = ActionType.Add, TransReferenceId = callPurpose.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallResultAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<CallResultDTO> CallResultUpdate(CallResultDTO model)
        {
            try
            {
                _cacheService.Delete("GetCallResultCacheKey");
                var callPurpose = _mapper.Map<SysCallResult>(model);
                callPurpose.Translates = model.Translates.Select(a => new SysCallResultTranslate
                {
                    Id = a.Id,
                    CallResultId = model.Id,
                    CallResultName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(callPurpose);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallResultUpdate), ActionType = ActionType.Update, TransReferenceId = callPurpose.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<CallResultDTO>(callPurpose);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallResultUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> CallResultsDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetCallResultCacheKey");
                if (await _context.Calls.AnyAsync(a => a.CallResultId == id))
                {
                    return false;
                }

                var model = await _context.SysCallResults.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CallResultsDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CallResultsDelete), ex);
                return false;
            }
        }

        #endregion

        #region DealStage
        //------------------------------------------------------------------------------------
        public async Task<List<DealStageDTO>> DealStageList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<DealStageDTO> result = await _context.SysDealStages.Select(n => new DealStageDTO
                {
                    Id = n.Id,
                    DealStage = n.Translates.FirstOrDefault(a => a.LanguageId == langId).StageName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.StageName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealStageList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<DealStageDTO> DealStageAdd(DealStageDTO model)
        {
            try
            {
                _cacheService.Delete("GetDealStageCacheKey");
                var dealStage = _mapper.Map<SysDealStage>(model);
                var newId = await _context.SysDealStages.AnyAsync() ? await _context.SysDealStages.MaxAsync(a => a.Id) + 1 : 1;
                dealStage.Id = newId;
                dealStage.Translates = model.Translates.Select(a => new SysDealStageTranslate
                {
                    StageId = newId,
                    StageName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(dealStage);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = dealStage.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DealStageAdd), ActionType = ActionType.Add, TransReferenceId = dealStage.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealStageAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<DealStageDTO> DealStageUpdate(DealStageDTO model)
        {
            try
            {
                _cacheService.Delete("GetDealStageCacheKey");
                var dealStage = _mapper.Map<SysDealStage>(model);
                dealStage.Translates = model.Translates.Select(a => new SysDealStageTranslate
                {
                    Id = a.Id,
                    StageId = model.Id,
                    StageName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(dealStage);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DealStageUpdate), ActionType = ActionType.Update, TransReferenceId = dealStage.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<DealStageDTO>(dealStage);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealStageUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> DealStageDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetDealStageCacheKey");
                if (await _context.Deals.AnyAsync(a => a.StageId == id))
                {
                    return false;
                }

                var model = await _context.SysDealStages.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DealStageDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealStageDelete), ex);
                return false;
            }
        }

        #endregion


        #region DealType
        //------------------------------------------------------------------------------------
        public async Task<List<DealTypeDTO>> DealTypeList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<DealTypeDTO> result = await _context.SysDealTypes.Select(n => new DealTypeDTO
                {
                    Id = n.Id,
                    DealType = n.Translates.FirstOrDefault(a => a.LanguageId == langId).TypeName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.TypeName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealTypeList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<DealTypeDTO> DealTypeAdd(DealTypeDTO model)
        {
            try
            {
                _cacheService.Delete("GetDealTypeCacheKey");
                var dealType = _mapper.Map<SysDealType>(model);
                var newId = await _context.SysDealTypes.AnyAsync() ? await _context.SysDealTypes.MaxAsync(a => a.Id) + 1 : 1;
                dealType.Id = newId;
                dealType.Translates = model.Translates.Select(a => new SysDealTypeTranslate
                {
                    DealTypeId = newId,
                    TypeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(dealType);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = dealType.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DealTypeAdd), ActionType = ActionType.Add, TransReferenceId = dealType.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealTypeAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<DealTypeDTO> DealTypeUpdate(DealTypeDTO model)
        {
            try
            {
                _cacheService.Delete("GetDealTypeCacheKey");
                var dealType = _mapper.Map<SysDealType>(model);
                dealType.Translates = model.Translates.Select(a => new SysDealTypeTranslate
                {
                    Id = a.Id,
                    DealTypeId = model.Id,
                    TypeName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(dealType);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DealTypeUpdate), ActionType = ActionType.Update, TransReferenceId = dealType.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<DealTypeDTO>(dealType);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealTypeUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> DealTypeDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetDealTypeCacheKey");
                if (await _context.Deals.AnyAsync(a => a.StageId == id))
                {
                    return false;
                }

                var model = await _context.SysDealTypes.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DealTypeDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealTypeDelete), ex);
                return false;
            }
        }

        #endregion

        #region JobTitle

        //------------------------------------------------------------------------------------
        public async Task<List<SysJobTitleDTO>> JobTitleList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<SysJobTitleDTO> result = await _context.SysJobTitles.Select(n => new SysJobTitleDTO
                {
                    Id = n.Id,
                    JobTitleName = n.Translates.FirstOrDefault(a => a.LanguageId == langId).JobTitleName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.JobTitleName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(JobTitleList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysJobTitleDTO> JobTitleAdd(SysJobTitleDTO model)
        {
            try
            {
                _cacheService.Delete("GetJobTitleCacheKey");
                var jobTitle = _mapper.Map<SysJobTitle>(model);
                var newId = await _context.SysJobTitles.AnyAsync() ? await _context.SysJobTitles.MaxAsync(a => a.Id) + 1 : 1;
                jobTitle.Id = newId;
                jobTitle.Translates = model.Translates.Select(a => new SysJobTitleTranslate
                {
                    JobTitleId = newId,
                    JobTitleName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(jobTitle);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = jobTitle.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(JobTitleAdd), ActionType = ActionType.Add, TransReferenceId = jobTitle.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(JobTitleAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<SysJobTitleDTO> JobTitleUpdate(SysJobTitleDTO model)
        {
            try
            {
                _cacheService.Delete("GetJobTitleCacheKey");
                var jobTitle = _mapper.Map<SysJobTitle>(model);
                jobTitle.Translates = model.Translates.Select(a => new SysJobTitleTranslate
                {
                    Id = a.Id,
                    JobTitleId = model.Id,
                    JobTitleName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(jobTitle);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(JobTitleUpdate), ActionType = ActionType.Update, TransReferenceId = jobTitle.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<SysJobTitleDTO>(jobTitle);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(JobTitleUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> JobTitleDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetJobTitleCacheKey");
                //if (await _context.Employees.AnyAsync(a => a.JobTitle == id))
                //{
                //    return false;
                //}

                var model = await _context.SysJobTitles.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(JobTitleDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(JobTitleDelete), ex);
                return false;
            }
        }

        #endregion

        #region Lead Source
        public async Task<List<LeadSourceDTO>> LeadSourceList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<LeadSourceDTO> result = await _context.SysLeadSources.Select(n => new LeadSourceDTO
                {
                    Id = n.Id,
                    LeadSource = n.Translates.FirstOrDefault(z => z.LanguageId == langId).SourceName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.SourceName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadSourceList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<LeadSourceDTO> LeadSourceAdd(LeadSourceDTO model)
        {
            try
            {
                _cacheService.Delete("GetLeadSourcesCacheKey");
                var leadSource = _mapper.Map<SysLeadSource>(model);
                var newId = await _context.SysLeadSources.AnyAsync() ? await _context.SysLeadSources.MaxAsync(a => a.Id) + 1 : 1;
                leadSource.Id = newId;
                leadSource.Translates = model.Translates.Select(a => new SysLeadSourceTranslate
                {
                    LeadSourceId = newId,
                    SourceName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(leadSource);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = leadSource.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadSourceAdd), ActionType = ActionType.Add, TransReferenceId = leadSource.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealStageAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<LeadSourceDTO> LeadSourceUpdate(LeadSourceDTO model)
        {
            try
            {
                _cacheService.Delete("GetLeadSourcesCacheKey");
                var leadSource = _mapper.Map<SysLeadSource>(model);
                leadSource.Translates = model.Translates.Select(a => new SysLeadSourceTranslate
                {
                    Id = a.Id,
                    LeadSourceId = model.Id,
                    SourceName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(leadSource);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadSourceUpdate), ActionType = ActionType.Update, TransReferenceId = leadSource.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<LeadSourceDTO>(leadSource);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealStageUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> LeadSourceDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetLeadSourcesCacheKey");
                if (await _context.Customers.AnyAsync(a => a.LeadSourceId == id))
                {
                    return false;
                }

                var model = await _context.SysLeadSources.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadSourceDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadSourceDelete), ex);
                return false;
            }
        }
        #endregion

        #region Lead Rating
        public async Task<List<LeadRatingDTO>> LeadRatingList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<LeadRatingDTO> result = await _context.SysLeadRatings.Select(n => new LeadRatingDTO
                {
                    Id = n.Id,
                    LeadRatin = n.Translates.FirstOrDefault(z => z.LanguageId == langId).RateName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.RateName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadRatingList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<LeadRatingDTO> LeadRatingAdd(LeadRatingDTO model)
        {
            try
            {
                _cacheService.Delete("GetLeadRatingCacheKey");
                var leadRating = _mapper.Map<SysLeadRating>(model);
                var newId = await _context.SysLeadRatings.AnyAsync() ? await _context.SysLeadRatings.MaxAsync(a => a.Id) + 1 : 1;
                leadRating.Id = newId;
                leadRating.Translates = model.Translates.Select(a => new SysLeadRatingTranslate
                {
                    LeadRatingId = newId,
                    RateName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(leadRating);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = leadRating.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadRatingAdd), ActionType = ActionType.Add, TransReferenceId = leadRating.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadRatingAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<LeadRatingDTO> LeadRatingUpdate(LeadRatingDTO model)
        {
            try
            {
                _cacheService.Delete("GetLeadRatingCacheKey");
                var leadRating = _mapper.Map<SysLeadRating>(model);
                leadRating.Translates = model.Translates.Select(a => new SysLeadRatingTranslate
                {
                    Id = a.Id,
                    LeadRatingId = model.Id,
                    RateName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(leadRating);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadRatingUpdate), ActionType = ActionType.Update, TransReferenceId = leadRating.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<LeadRatingDTO>(leadRating);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadRatingUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> LeadRatingDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetLeadRatingCacheKey");
                if (await _context.Customers.AnyAsync(a => a.LeadRatingId == id))
                {
                    return false;
                }

                var model = await _context.SysLeadRatings.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadRatingDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadSourceDelete), ex);
                return false;
            }
        }
        #endregion

        #region Lead Status
        public async Task<List<LeadStatusDTO>> LeadStatusList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<LeadStatusDTO> result = await _context.SysLeadStatus.Select(n => new LeadStatusDTO
                {
                    Id = n.Id,
                    LeadStatus = n.Translates.FirstOrDefault(z => z.LanguageId == langId).StatusName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.StatusName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadStatusList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<LeadStatusDTO> LeadStatusAdd(LeadStatusDTO model)
        {
            try
            {
                _cacheService.Delete("GetLeadStatusCacheKey");
                var leadStatus = _mapper.Map<SysLeadStatus>(model);
                var newId = await _context.SysLeadStatus.AnyAsync() ? await _context.SysLeadStatus.MaxAsync(a => a.Id) + 1 : 1;
                leadStatus.Id = newId;
                leadStatus.Translates = model.Translates.Select(a => new SysLeadStatusTranslate
                {
                    LeadStatusId = newId,
                    StatusName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(leadStatus);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = leadStatus.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadStatusAdd), ActionType = ActionType.Add, TransReferenceId = leadStatus.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadStatusAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<LeadStatusDTO> LeadStatusUpdate(LeadStatusDTO model)
        {
            try
            {
                _cacheService.Delete("GetLeadStatusCacheKey");
                var leadStatus = _mapper.Map<SysLeadStatus>(model);
                leadStatus.Translates = model.Translates.Select(a => new SysLeadStatusTranslate
                {
                    Id = a.Id,
                    LeadStatusId = model.Id,
                    StatusName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(leadStatus);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadStatusUpdate), ActionType = ActionType.Update, TransReferenceId = leadStatus.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<LeadStatusDTO>(leadStatus);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadRatingUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> LeadStatusDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetLeadStatusCacheKey");
                if (await _context.Customers.AnyAsync(a => a.LeadStatusId == id))
                {
                    return false;
                }

                var model = await _context.SysLeadStatus.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LeadStatusDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LeadStatusDelete), ex);
                return false;
            }
        }

        Task<SysAirConditionTypeDTO> ILookupsCRUDService.AirConditionTypesAdd(SysAirConditionTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.AirConditionTypesDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysAirConditionTypeDTO>> ILookupsCRUDService.AirConditionTypesList()
        {
            throw new NotImplementedException();
        }

        Task<SysAirConditionTypeDTO> ILookupsCRUDService.AirConditionTypesUpdate(SysAirConditionTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysHeatingTypeDTO> ILookupsCRUDService.HeatingTypesAdd(SysHeatingTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.HeatingTypesDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysHeatingTypeDTO>> ILookupsCRUDService.HeatingTypesList()
        {
            throw new NotImplementedException();
        }

        Task<SysHeatingTypeDTO> ILookupsCRUDService.HeatingTypesUpdate(SysHeatingTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysProjectFeatureDTO> ILookupsCRUDService.ProjectFeaturesAdd(SysProjectFeatureDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.ProjectFeaturesDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysProjectFeatureDTO>> ILookupsCRUDService.ProjectFeaturesList()
        {
            throw new NotImplementedException();
        }

        Task<SysProjectFeatureDTO> ILookupsCRUDService.ProjectFeaturesUpdate(SysProjectFeatureDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysProjectStatusDTO> ILookupsCRUDService.ProjectStatusAdd(SysProjectStatusDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.ProjectStatusDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysProjectStatusDTO>> ILookupsCRUDService.ProjectStatusList()
        {
            throw new NotImplementedException();
        }

        Task<SysProjectStatusDTO> ILookupsCRUDService.ProjectStatusUpdate(SysProjectStatusDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysProjectTypeDTO> ILookupsCRUDService.ProjectTypesAdd(SysProjectTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.ProjectTypesDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysProjectTypeDTO>> ILookupsCRUDService.ProjectTypesList()
        {
            throw new NotImplementedException();
        }

        Task<SysProjectTypeDTO> ILookupsCRUDService.ProjectTypesUpdate(SysProjectTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysRealEstateTypeDTO> ILookupsCRUDService.RealEstateTypesAdd(SysRealEstateTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.RealEstateTypesDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysRealEstateTypeDTO>> ILookupsCRUDService.RealEstateTypesList()
        {
            throw new NotImplementedException();
        }

        Task<SysRealEstateTypeDTO> ILookupsCRUDService.RealEstateTypesUpdate(SysRealEstateTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysRealStateClassDTO> ILookupsCRUDService.RealStateClassAdd(SysRealStateClassDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.RealStateClassDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysRealStateClassDTO>> ILookupsCRUDService.RealStateList()
        {
            throw new NotImplementedException();
        }

        Task<SysRealStateClassDTO> ILookupsCRUDService.RealStateClassUpdate(SysRealStateClassDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysRoomsCountDTO> ILookupsCRUDService.RoomsCountAdd(SysRoomsCountDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.RoomsCountDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysRoomsCountDTO>> ILookupsCRUDService.RoomsCountList()
        {
            throw new NotImplementedException();
        }

        Task<SysRoomsCountDTO> ILookupsCRUDService.RoomsCountUpdate(SysRoomsCountDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysPlacesTypeDTO> ILookupsCRUDService.PlacesTypeAdd(SysPlacesTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.PlacesTypeDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysPlacesTypeDTO>> ILookupsCRUDService.PlacesTypeList()
        {
            throw new NotImplementedException();
        }

        Task<SysPlacesTypeDTO> ILookupsCRUDService.PlacesTypeUpdate(SysPlacesTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<BalconieDirectionDTO> ILookupsCRUDService.BalconieDirectionUpdate(BalconieDirectionDTO model)
        {
            throw new NotImplementedException();
        }

        Task<List<BalconieDirectionDTO>> ILookupsCRUDService.BalconieDirectionList()
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.BalconieDirectionDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<BalconieDirectionDTO> ILookupsCRUDService.BalconieDirectionAdd(BalconieDirectionDTO model)
        {
            throw new NotImplementedException();
        }

        Task<SysViewTypeDTO> ILookupsCRUDService.ViewTypeAdd(SysViewTypeDTO model)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILookupsCRUDService.ViewTypeDelete(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<SysViewTypeDTO>> ILookupsCRUDService.ViewTypeList()
        {
            throw new NotImplementedException();
        }

        Task<SysViewTypeDTO> ILookupsCRUDService.ViewTypeUpdate(SysViewTypeDTO model)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Decision
        public async Task<List<DecisionDTO>> DecisionList()
        {
            int langId = await _languageService.GetLanguageIdFromRequestAsync();
            try
            {
                List<DecisionDTO> result = await _context.SysDecisions.Select(n => new DecisionDTO
                {
                    Id = n.Id,
                    DecisionName = n.Translates.FirstOrDefault(z => z.LanguageId == langId).DecisionName,
                    Translates = n.Translates.Select(t => new ComboboxModelTranslate
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        Name = t.DecisionName
                    }).ToList()
                }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DecisionList), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<DecisionDTO> DecisionAdd(DecisionDTO model)
        {
            try
            {
                _cacheService.Delete("GetDecisionsCacheKey");
                var decision = _mapper.Map<SysDecision>(model);
                var newId = await _context.SysDecisions.AnyAsync() ? await _context.SysDecisions.MaxAsync(a => a.Id) + 1 : 1;
                decision.Id = newId;
                decision.Translates = model.Translates.Select(a => new SysDecisionTranslate
                {
                    DecisionId = newId,
                    DecisionName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                await _context.AddAsync(decision);
                await _context.SaveChangesAsync();
                model.Id = newId;
                model.Translates.ForEach(t =>
                {
                    t.Id = decision.Translates.Where(tran => tran.LanguageId == t.LanguageId).FirstOrDefault().Id;
                });
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DecisionAdd), ActionType = ActionType.Add, TransReferenceId = decision.Id.ToString(), TransType = LogTransType.Lookups });
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DecisionAdd), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<DecisionDTO> DecisionUpdate(DecisionDTO model)
        {
            try
            {
                _cacheService.Delete("GetDecisionsCacheKey");
                var decision = _mapper.Map<SysDecision>(model);
                decision.Translates = model.Translates.Select(a => new SysDecisionTranslate
                {
                    Id = a.Id,
                    DecisionId = model.Id,
                    DecisionName = a.Name,
                    LanguageId = a.LanguageId,
                }).ToList();
                _context.Update(decision);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DecisionUpdate), ActionType = ActionType.Update, TransReferenceId = decision.Id.ToString(), TransType = LogTransType.Lookups });
                return _mapper.Map<DecisionDTO>(decision);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DealStageUpdate), ex);
                return null;
            }
        }

        //------------------------------------------------------------------------------------
        public async Task<bool> DecisionDelete(int id)
        {
            try
            {
                _cacheService.Delete("GetDecisionsCacheKey");
                if (await _context.SysDecisions.AnyAsync(a => a.Id == id))
                {
                    return false;
                }

                var model = await _context.SysDecisions.FirstOrDefaultAsync(a => a.Id == id);
                _context.Remove(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DecisionDelete), ActionType = ActionType.Delete, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Lookups });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DecisionDelete), ex);
                return false;
            }
        }

        public Task<SysSalesCategoryDTO> CompanyCategoryAdd(SysSalesCategoryDTO model)
        {
            throw new NotImplementedException();
        }

        Task<List<SysSalesCategoryDTO>> ILookupsCRUDService.CompanyCategoryList()
        {
            throw new NotImplementedException();
        }

        public Task<SysSalesCategoryDTO> CompanyCategoryUpdate(SysSalesCategoryDTO model)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }
}
