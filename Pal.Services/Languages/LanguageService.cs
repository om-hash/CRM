using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Pal.Core.Domains.Languages;
using Pal.Data.Contexts;
using Pal.Services.Caching;
using Pal.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Services.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheService<List<Language>> _cacheService;

        private readonly ApplicationDbContext _context;
        private readonly ILoggerService _logger;
        private List<Language> _cachedLanguages;

        public LanguageService(ApplicationDbContext context,
            ILoggerService logger, IHttpContextAccessor httpContextAccessor, ICacheService<List<Language>> cacheService)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cacheService = cacheService;
        }

        public IQueryable<Language> GetLanguages()
        {
            try
            {
                return _context.Languages.Where(x=> x.IsActive == true);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetLanguages), ex);
                return null;
            }
        }

        public Language GetLanguageByCulture(string culture)
        {
            try
            {
                return _context.Languages.FirstOrDefault(x =>
                    x.Culture.Trim().ToLower() == culture.Trim().ToLower());
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetLanguageByCulture), ex);
                return null;
            }
        }

        public async Task<Language> GetLanguageFromRequestAsync()
        {
            try
            {
                if (_httpContextAccessor.HttpContext?.Request == null)
                    return null;

                //get request culture
                var requestCulture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture;
                if (requestCulture == null)
                    return null;

                var currentCultureString = requestCulture.Culture.Name;//.Split('-')[0].Trim();
                if (_cachedLanguages == null)
                    await SetCachedLanguages();


                //return _cachedLanguages.FirstOrDefault(a => a.Culture.Equals(currentCultureString, StringComparison.InvariantCultureIgnoreCase));

                _cachedLanguages = await _cacheService.GetAsync("currentWorkingLanguages");
                if (_cachedLanguages != null)
                {
                    return _cachedLanguages.FirstOrDefault(a => a.Culture.Equals(currentCultureString, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    //try to get language by culture name
                    var languages = GetLanguages().ToList();
                    await _cacheService.SetAsync("currentWorkingLanguages", languages, TimeSpan.FromDays(15));
                    var requestLanguage = languages.FirstOrDefault(language =>
                       language.Culture.Equals(currentCultureString, StringComparison.InvariantCultureIgnoreCase));
                    return requestLanguage;
                }
            }
            catch (Exception ex)
            {
                //_ = _logger.LogErrorAsync(nameof(GetLanguageFromRequestAsync), ex);
                return null;
            }

        }

        //-----------------------------------------------------------------
        private async Task SetCachedLanguages()
        {
            _cachedLanguages = await _cacheService.GetAsync("currentWorkingLanguages");
            if (_cachedLanguages == null)
            {
                var languages = GetLanguages().ToList();
                await _cacheService.SetAsync("currentWorkingLanguages", languages, TimeSpan.FromDays(15));
                _cachedLanguages = languages;
            }
        }
        //-----------------------------------------------------------------
        public List<Language> GetAllLanguages()
        {
            if (_cachedLanguages == null)
                SetCachedLanguages().Wait();

            return _cachedLanguages;
        }

        //-----------------------------------------------------------------
        public async Task<int> GetLanguageIdFromRequestAsync()
        {
            try
            {
                var lang = await GetLanguageFromRequestAsync();
                Console.WriteLine("GetLanguageIdFromRequestAsync");
                return lang?.Id ?? 1;
            }
            catch (Exception ex)
            {
                //_ = _logger.LogErrorAsync(nameof(GetLanguageIdFromRequestAsync), ex);
                return 1;
            }
        }

        //-----------------------------------------------------------------
        public string GetCurrentLayoutDirection()
        {
            return GetLanguageFromRequestAsync().Result.IsRtl ? "rtl" : "ltr";
        }

        //-----------------------------------------------------------------
        public bool IsLayoutRtl()
        {
            return GetLanguageFromRequestAsync().Result?.IsRtl ?? false;
        }
    }

    
}
