using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Languages;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Language;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.Languages;
using Pal.Services.Caching;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.LanguegService
{
    public class LanguageService : ILanguagesService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICacheService<List<Language>> _languagesCacheService;
        private readonly ICacheService<string> _stringResourcesCacheService;
        private readonly ILoggerService _logger;
        private readonly ICacheService<string> _resourceCache;
        private readonly ILanguageService _languagesService;


        public LanguageService(ApplicationDbContext context, IMapper mapper, ILanguageService languagesService, ICacheService<List<Language>> LanguagesCacheService, ICacheService<string> stringResourcesCacheService, ApplicationDbContext applicationDbContext,
            ILoggerService logger, ICacheService<string> cacheService)
        {
            _context = context;
            _languagesService = languagesService;
            _mapper = mapper;
            _languagesCacheService = LanguagesCacheService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _resourceCache = cacheService;
            _stringResourcesCacheService = stringResourcesCacheService;
        }



        //-----------------------------------------------------------------
        private async Task<bool> SetCachForlanguges()
        {
            var Cachedlanguages = await _languagesCacheService.GetAsync("currentWorkingLanguages");
            if (Cachedlanguages == null)
            {
                var languages = _languagesService.GetLanguages().ToList();
                await _languagesCacheService.SetAsync("currentWorkingLanguages", languages, TimeSpan.FromDays(15));
                return true;
            }
            else
            {
                _languagesCacheService.Delete("currentWorkingLanguages");
                var languages = _languagesService.GetLanguages().ToList();
                await _languagesCacheService.SetAsync("currentWorkingLanguages", languages, TimeSpan.FromDays(15));
                return true;
            }
        }
        //-----------------------------------------------------------------
        private async Task<bool> SetCachForStringResources()
        {
            var Cachedlanguages = await _stringResourcesCacheService.GetAsync("LocalizationHasInitilized");
            if (Cachedlanguages != "true")
            {
                // Store each key as new cache item
                _context.LanguageStringResources.Select(a => new LanguageStringResourceSemplified
                {
                    LanguageId = a.LanguageId,
                    ResourceName = a.ResourceName,
                    ResourceValue = a.ResourceValue,
                }).ToList().ForEach(a =>
                {
                    var dynamicCacheKey = $"Local_{a.LanguageId}_{a.ResourceName}";
                    _stringResourcesCacheService.SetAsync(dynamicCacheKey, a.ResourceValue, TimeSpan.FromDays(30)).Wait();
                });
                _stringResourcesCacheService.SetAsync("LocalizationHasInitilized", "true", TimeSpan.FromDays(30)).Wait();
                return true;
            }
            else
            {
                _stringResourcesCacheService.Delete("LocalizationHasInitilized");

                _context.LanguageStringResources.Select(a => new LanguageStringResourceSemplified
                {
                    LanguageId = a.LanguageId,
                    ResourceName = a.ResourceName,
                    ResourceValue = a.ResourceValue,
                }).ToList().ForEach(a =>
                {
                    var dynamicCacheKey = $"Local_{a.LanguageId}_{a.ResourceName}";
                    _stringResourcesCacheService.SetAsync(dynamicCacheKey, a.ResourceValue, TimeSpan.FromDays(30)).Wait();
                });
                _stringResourcesCacheService.SetAsync("LocalizationHasInitilized", "true", TimeSpan.FromDays(30)).Wait();
                return true;
            }
        }
        //-----------------------------------------------------------------
        public async Task<bool> AddLanguage(LanguageCreateDTO newLanguageCreateDTO)
        {
            try
            {
                var maxid = _context.Languages.Max(x => x.Id);
                var newlanguage = new Language()
                {
                    Id = maxid + 1,
                    Culture = newLanguageCreateDTO.Culture,
                    IsDefault = false,
                    IsRtl = newLanguageCreateDTO.IsRtl,
                    Name = newLanguageCreateDTO.Name,
                    Shortcut = newLanguageCreateDTO.Shortcut,
                    IsActive = newLanguageCreateDTO.IsActive,
                };
                await _context.Languages.AddAsync(newlanguage);
                await _applicationDbContext.Languages.AddAsync(newlanguage);
                await _context.SaveChangesAsync();
                await _applicationDbContext.SaveChangesAsync();

                //getting the string resorces;
                var newlanguageString = await _context.LanguageStringResources.Where(x => x.LanguageId == 1).Select(a => new LanguageStringResource
                {
                    LanguageId = newlanguage.Id,
                    ResourceName = a.ResourceName,
                    ResourceValue = a.ResourceValue,
                    StringResourceGroup = a.StringResourceGroup,
                }).ToListAsync();
                //foreach (var oldlang in newlanguageString)
                //{
                //    oldlang.LanguageId = newlanguage.Id;
                //}
                _context.LanguageStringResources.AddRange(newlanguageString);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(AddLanguage), ActionType = ActionType.Add, TransReferenceId = newlanguage.Id.ToString(), TransType = LogTransType.Language });
                await SetCachForlanguges();
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(AddLanguage), ex);
                return false;
            }

        }
        //-----------------------------------------------------------------
        public async Task<bool> StringResourceUpdate(int id, string value)
        {
            try
            {
                var newStringResource = await _context.LanguageStringResources.FirstOrDefaultAsync(x => x.Id == id);
                newStringResource.ResourceValue = value;
                _context.Update(newStringResource);
                await _context.SaveChangesAsync();

                string dynamicCacheKey = $"Local_{newStringResource.LanguageId}_{newStringResource.ResourceName}";

                var resourcesJson = _resourceCache.GetAsync(dynamicCacheKey).Result;
                if (resourcesJson != null)
                    _resourceCache.Delete(dynamicCacheKey);
                await _resourceCache.SetAsync(dynamicCacheKey, value, TimeSpan.FromDays(30));
                await SetCachForStringResources();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(StringResourceUpdate), ActionType = ActionType.Update, TransReferenceId = newStringResource.Id.ToString(), TransType = LogTransType.Language });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(StringResourceUpdate), ex);
                return false;
            }
        }


        //-----------------------------------------------------------------
        public async Task<LanguageStringResourceDTO> GetStringResourcesById(int id)
        {
            try
            {
                var srtingResource = await _context.LanguageStringResources.FirstOrDefaultAsync(x => x.Id == id);
                var model = _mapper.Map<LanguageStringResourceDTO>(srtingResource);
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetStringResourcesById), ex);
                return null;
            }
        }
        //-----------------------------------------------------------------
        public async Task<LanguageCreateDTO> GetLanguageById(int id)
        {
            try
            {
                var stringSersorces = await _context.LanguageStringResources.Where(x => x.LanguageId == id).Select(x => new LanguageStringResourceDTO
                {

                    Id = x.Id,
                    LanguageId = x.LanguageId,
                    ResourceName = x.ResourceName,
                    ResourceValue = x.ResourceValue,
                    StringResourceGroup = x.StringResourceGroup,
                    TextStringResourceGroup = x.StringResourceGroup.ToString(),

                }).ToListAsync();

                var language = await _context.Languages.Select(x => new LanguageCreateDTO
                {
                    Id = x.Id,
                    Culture = x.Culture,
                    IsDefault = x.IsDefault,
                    Name = x.Name,
                    Shortcut = x.Shortcut,
                    IsRtl = x.IsRtl,
                    IsActive = x.IsActive,
                    LanguageStringResourceDTOs = stringSersorces,
                }).FirstOrDefaultAsync(x => x.Id == id);

                return language;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetLanguageById), ex);
                return null;
            }
        }

        //-----------------------------------------------------------------
        public async Task<bool> LanguageUpdate(LanguageCreateDTO newLanguageCreateDTO)
        {
            try
            {
                var language = _mapper.Map<Language>(newLanguageCreateDTO);
                _context.Update(language);
                _applicationDbContext.Update(language);
                await _context.SaveChangesAsync();
                await _applicationDbContext.SaveChangesAsync();

                await SetCachForlanguges();

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(LanguageUpdate), ActionType = ActionType.Update, TransReferenceId = language.Id.ToString(), TransType = LogTransType.Language });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LanguageUpdate), ex);
                return false;
            }
        }

        //-----------------------------------------------------------------
        public async Task<bool> AddNewResource(string key, string value, int langId)
        {
            try
            {
                if (await _context.LanguageStringResources.AnyAsync(a => a.LanguageId == langId && a.ResourceName == key))
                {
                    // edit

                }
                else
                {
                    //add
                    var resource = new LanguageStringResource { LanguageId = langId, ResourceName = key, ResourceValue = value };
                    _context.LanguageStringResources.Add(resource);
                }

                await _context.SaveChangesAsync();

                string dynamicCacheKey = $"Local_{langId}_{key}";

                var resourcesJson = _resourceCache.GetAsync(dynamicCacheKey).Result;
                if (resourcesJson != null)
                    _resourceCache.Delete(dynamicCacheKey);
                await _resourceCache.SetAsync(dynamicCacheKey, value, TimeSpan.FromDays(30));

                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(AddNewResource), ex);
                return false;
            }
        }

        public async Task<SyncPaginatedListModel<LanguageCreateDTO>> GetAllLanguages(DataManagerRequest dm)
        {
            try
            {

                var query = _context.Languages.AsQueryable().AsNoTracking()
                    .Select(x => new LanguageCreateDTO
                    {
                        Culture = x.Culture,
                        IsRtl = x.IsRtl,
                        IsDefault = x.IsDefault,
                        Name = x.Name,
                        Shortcut = x.Shortcut,
                        Id = x.Id,
                        IsActive = x.IsActive,
                    });

                return await SyncGridOperations<LanguageCreateDTO>.PagingAndFilterAsync(query, dm);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAllLanguages), ex);
                return null;
            }
        }

        public async Task<bool> DelelteLanguage(int id)
        {
            try
            {
                if (id == 1 || id == 2)
                    return false;


                var languages = await _context.Languages.FirstOrDefaultAsync(x => x.Id == id);
                var strignRecorce = await _context.LanguageStringResources.Where(x => x.LanguageId == id).ToListAsync();

                _context.LanguageStringResources.RemoveRange(strignRecorce);
                _context.Languages.Remove(languages);
                _applicationDbContext.Remove(languages);
                await _context.SaveChangesAsync();
                await _applicationDbContext.SaveChangesAsync();
                await SetCachForlanguges();

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DelelteLanguage), ActionType = ActionType.Delete, TransReferenceId = languages.Id.ToString(), TransType = LogTransType.Language });
                return true;


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DelelteLanguage), ex);
                return false;
            }


        }
    }
}
