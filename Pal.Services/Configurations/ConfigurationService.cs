using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Config;
using Pal.Data.Contexts;
using Pal.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.Configurations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private WebsiteConfiguration Config;
        private readonly ILoggerService _logger;
        //-----------------------------------------------------------------------------
        public ConfigurationService(ApplicationDbContext ApplicationDbContext,
            ILoggerService logger)
        {
            _logger = logger;
            _ApplicationDbContext = ApplicationDbContext;
            CacheConfig();
        }



        //-----------------------------------------------------------------------------
        public WebsiteConfiguration GetWebsiteConfiguration()
        {
            try
            {
                return Config;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetWebsiteConfiguration), ex);
                return null;
            }
          
        }
        //-----------------------------------------------------------------------------
        public void SetWebsiteConfiguration(WebsiteConfiguration model)
        {
            try
            {
                var config = _ApplicationDbContext.WebsiteConfigurations.FirstOrDefault();
                config.IsUseIFrameTemplateInControlPanel = model.IsUseIFrameTemplateInControlPanel;
                _ApplicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SetWebsiteConfiguration), ex);
            }
        }

        //-----------------------------------------------------------------------------


        public  string GetGoogleMapsAPIKey()
        {
            try
            {

                return Config.GoogleMapsAPIKey;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetGoogleMapsAPIKey), ex);
                return null;
            }

        }

        private void CacheConfig()
        {
            try
            {
                if (Config == null)
                {
                    Config = _ApplicationDbContext.WebsiteConfigurations.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CacheConfig), ex);
            }
           
        }
    }
}
