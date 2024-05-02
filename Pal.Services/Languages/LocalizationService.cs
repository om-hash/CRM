using Pal.Core.Domains.Languages;
using Pal.Core.Enums.ActivityLog;
using Pal.Data.Contexts;
using Pal.Services.Caching;
using Pal.Services.Logger;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Pal.Services.Languages
{

    public class LocalizationService : ILocalizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService<string> _resourceCache;
        private readonly bool cashActivate = true;
        //private readonly bool storeInCacheAsList = false;
        private const string LocalizationCacheKey = "LocalizationCacheKey";
        private readonly ILoggerService _logger;

        public LocalizationService(ApplicationDbContext context, ICacheService<string> resourceCache,
            ILoggerService logger)
        {
            _context = context;
            _resourceCache = resourceCache;
            _logger = logger;
        }

        //---------------------------------------------------------------------------------------------------
        public string GetStringResource(string resourceKey, int languageId)
        {
            try
            {
                //return resourceKey;
                if (cashActivate)
                {// Use Caching

                    string resourcesJson = null;
                    List<LanguageStringResourceSemplified> resources = new();


                    CheckIsFirstRun();
                    string dynamicCacheKey = $"Local_{languageId}_{resourceKey}";
                    resourcesJson = _resourceCache.GetAsync(dynamicCacheKey).Result;
                    if (string.IsNullOrEmpty(resourcesJson))
                    {
                        resourcesJson = _context.LanguageStringResources.FirstOrDefault(x =>
                              x.ResourceName.Trim().ToLower() == resourceKey.Trim().ToLower()
                              && x.LanguageId == languageId)?.ResourceValue ?? "";

                        _resourceCache.SetAsync(dynamicCacheKey, resourcesJson, TimeSpan.FromDays(30));
                        //return "";
                    }
                    return resourcesJson;
                }
                else // Don't Use Caching
                {

                    return _context.LanguageStringResources.FirstOrDefault(x =>
                      x.ResourceName.Trim().ToLower() == resourceKey.Trim().ToLower()
                      && x.LanguageId == languageId)?.ResourceValue ?? resourceKey;
                    //return resourceKey;
                }


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetStringResource) + " (" + resourceKey + ")", ex, Importance.Low);
                return null;
            }

        }


        private void CheckIsFirstRun()
        {
            //TODO Log
            var check = _resourceCache.GetAsync("LocalizationHasInitilized").Result;
            if (check != "true")
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
                    _resourceCache.SetAsync(dynamicCacheKey, a.ResourceValue, TimeSpan.FromDays(30)).Wait();
                });

                _resourceCache.SetAsync("LocalizationHasInitilized", "true", TimeSpan.FromDays(30)).Wait();

            }
        }

        public void Dispose()
        {

        }
    }
}
