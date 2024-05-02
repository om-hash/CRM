using Pal.Core.Domains.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.Configurations
{
    public interface IConfigurationService
    {
        string GetGoogleMapsAPIKey();
        public WebsiteConfiguration GetWebsiteConfiguration();

        public void SetWebsiteConfiguration(WebsiteConfiguration model);
 
    }
}
