using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pal.Core.Domains.Lookups;
using Pal.Core.Enums;
using Pal.Services.DataServices.Lookups;

using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.PalFunctions;
using Pal.Services.WebWorkContext;
using Pal.Web.Extensions;
using Pal.Web.Models;
using Pal.Web.Models.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Controllers
{
    public class HomeController : BaseController
    {
      

        private readonly ILoggerService _logger;
        private readonly IWebWorkContext _webWorkContext;



        public HomeController(ILoggerService logger, ILanguageService languageService, ILocalizationService localizationService,
            IWebWorkContext webWorkContext)
            : base(languageService, localizationService)
        {
            _logger = logger;
            _webWorkContext = webWorkContext;
            //SetGlobals();
        }

        //-------------------------------------------------------------------------------------+
        public async Task GetFillteringLookups()
        {
            try
            {
                //ViewData["PaymentMethod"] = (await _lookupsService.GetSysPaymentType()).ToSelectList();
                //ViewData["RealEstateTypes"] = (await _lookupsService.GetSysRealEstateTypes(null)).ToSelectList();
                //ViewData["RealEstateFeatures"] = (await _lookupsService.GetSysFeatures(null)).ToSelectList();
                //ViewData["RealEstateRoomsCounts"] = (await _lookupsService.GetSysRoomsCounts(null)).ToSelectList();
                //ViewData["HighestRealEstatePrice"] = await _lookupsService.GetSysHighestRealEstatePrice();
                //ViewData["LowestRealEstatePrice"] = await _lookupsService.GetSysLowestRealEstatePrice();
                //ViewData["HighestArea"] = await _lookupsService.GetSysHighestRealEstateArea();
                //ViewData["LowestArea"] = await _lookupsService.GetSysLowestRealEstateArea();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("RealEstatesControllerView" + nameof(GetFillteringLookups), ex);
            }

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var langId =await _languageService.GetLanguageIdFromRequestAsync();
                HomeViewModel model = new HomeViewModel();
                //model.RealEstates = await _realEstateService.GetFeaturedRealEstates(langId);
                //model.Projects = _projectService.GetFeaturedProjects(langId).Result.ToList();
                //model.Regions = await _regionService.GetFeaturedRegions(langId);
                //model.Advertisements = (await _advertisement.GetAdvertisementsList(null)).Data;
                await GetFillteringLookups();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(Index), ex);
                return NotFound();
            }

        }

        public IActionResult PrivacyPolicy()
        {
            try
            {
                //var model = await _generalInforService.GetGeneralInfo();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(PrivacyPolicy), ex);
                return NotFound();
            }

        }
        public IActionResult TermsAndConditions()
        {
            try
            {
                //var model = await _generalInforService.GetGeneralInfo();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(TermsAndConditions), ex);
                return NotFound();
            }

        }
        public IActionResult TermsOfUse()
        {
            try
            {
                //var model = await _generalInforService.GetGeneralInfo();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(TermsOfUse), ex);
                return NotFound();
            }

        }

        public IActionResult About()
        {
            try
            {
                //var aboutUs = await _aboutUsService.AboutUsInformation();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(About), ex);
                return NotFound();
            }

        }

        public IActionResult Contact()
        {
            try
            {
                //var contactUs = await _contactUsService.GetContactUsInfo();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(Contact), ex);
                return NotFound();
            }

        }

        [HttpPost]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            try
            {
                Response.Cookies.Append(
               CookieRequestCultureProvider.DefaultCookieName,
               CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
               new CookieOptions
               {
                   Expires = DateTimeOffset.UtcNow.AddDays(365)
               }
           );

                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(ChangeLanguage), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(Error), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------------------------
        public ActionResult TestTus()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------
        //public async Task<IActionResult> GetRegistrationFeatures()
        //{
        //    try
        //    {
        //        var model = await _frontendInfo.GetRegistrationFeatures();
        //        if (model != null)
        //            return Json(new ResponseResult(ResponseType.Success, model.ToString()));

        //        else
        //            return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = _logger.LogErrorAsync("HomeController" + nameof(GetRegistrationFeatures), ex);
        //        return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
        //    }

        //}
        //------------------------------------------------------------------------------------------
        //public async Task<IActionResult> Search(string searchText, bool isForRent = false, bool isForSale = false)
        //{
        //    try
        //    {
        //        if (searchText == null)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        var result = await _frontendInfo.Search(searchText, isForRent, isForSale);
        //        return View(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = _logger.LogErrorAsync("HomeController" + nameof(Search), ex);
        //        return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
        //    }
        //}
        //------------------------------------------------------------------------------------------
        public IActionResult OurServices()
        {
            try
            {
                //var Service = await _ourServicesService.GetServicesForView();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(OurServices), ex);
                return NotFound();
            }
        }
        //------------------------------------------------------------------------------------------
        public IActionResult OurServiceDetails(int id)
        {

            try
            {
                //var Service = await _ourServicesService.GetServiceByIdForView(id);
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(OurServiceDetails), ex);
                return NotFound();
            }
        }

        public IActionResult AutoCompleteCityOrRegion(string text)
        {
            try
            {
                if (!String.IsNullOrEmpty(text))
                {
                    if (!CultureInfo.InvariantCulture.EnglishName.Contains("English"))
                    {
                        text = text.Replace("i" , "İ");
                        text = text.Replace("ı", "I");
                        text = text.Replace("ü", "Ü");
                        text = text.Replace("ö", "Ö");
                        text = text.Replace("ğ", "Ğ");
                    }
                    text = text.ToUpper();

                    //var model = await _lookupsService.GetSysCountries(text);
                    //model.AddRange(await _lookupsService.GetSysCities(text));
                    //model.AddRange(await _lookupsService.GetSysRegions(text));
                    //model.AddRange(await _lookupsService.GetSysNeighborhoods(text));
                    //var sortedRes = model.OrderBy(i => i.Name.IndexOf(text)).ToList();
                    
                    return null;
                }
                
                return Json("");
            }
            catch(Exception ex)
            {
                _ = _logger.LogErrorAsync("HomeController" + nameof(AutoCompleteCityOrRegion), ex);
                return NotFound();
            }
        }
        //------------------------------------------------------------------------------------------
    }
}
