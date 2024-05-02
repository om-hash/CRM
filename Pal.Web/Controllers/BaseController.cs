using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Enums.Account;
using Pal.Data.DTOs.Notifications;
using Pal.Services.Languages;
using Pal.Services.Notifications;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Pal.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        //--------------------------------------------------------------------------------
        public BaseController(ILanguageService languageService, ILocalizationService localizationService)
        {
            _languageService = languageService;
            _localizationService = localizationService;
        }

        //--------------------------------------------------------------------------------
        public HtmlString Localize(string resourceKey, params object[] args)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;

            var language = _languageService.GetLanguageByCulture(currentCulture);
            if (language != null)
            {
                var ResourceValue = _localizationService.GetStringResource(resourceKey, language.Id);
                if (string.IsNullOrEmpty(ResourceValue))
                {
                    return new HtmlString(resourceKey);
                }

                return new HtmlString((args == null || args.Length == 0)
                    ? ResourceValue
                    : string.Format(ResourceValue, args));
            }

            return new HtmlString(resourceKey);
        }


        //--------------------------------------------------------------------------------
        public void AddErrorsToModelState()
        {
            foreach (var item in ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }
        }

        //--------------------------------------------------------------------------------
        public string GetUserType()
        {
            try
            {
                return User.FindFirst(PalClaimType.UserType.ToString()).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }


        //--------------------------------------------------------------------------------
        public bool AmICompany()
        {
            try
            {
                return User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Companies.ToString();
            }
            catch (Exception)
            {
                return false;
            }
        }  
        public bool AmISales()
        {
            try
            {
                return User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Sales.ToString();
            }
            catch (Exception)
            {
                return false;
            }
        }

        //--------------------------------------------------------------------------------
        public bool AmICustomer()
        {
            try
            {
                return User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Customers.ToString();
            }
            catch (Exception)
            {
                return false;
            }
        }

        //--------------------------------------------------------------------------------
        public bool AmIAdvisor()
        {
            try
            {
                return User.FindFirst(PalClaimType.UserType.ToString()).Value == UserType.Adviser.ToString();
            }
            catch (Exception)
            {
                return false;
            }
        }

        //--------------------------------------------------------------------------------
        public long GetCustomerId()
        {
            try
            {
                return long.Parse(User.FindFirst(PalClaimType.CustomerId.ToString()).Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //--------------------------------------------------------------------------------
        public int GetCompanyId()
        {
            try
            {
                return int.Parse(User.FindFirst(PalClaimType.CompanyId.ToString()).Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int GetSalesId()
        {
            try
            {
                return int.Parse(User.FindFirst(PalClaimType.SalesId.ToString()).Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //--------------------------------------------------------------------------------
        public int GetAdvisorId()
        {
            try
            {
                return int.Parse(User.FindFirst(PalClaimType.AdvisorId.ToString()).Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }


    }
}
