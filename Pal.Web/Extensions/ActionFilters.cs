using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pal.Core.Enums.Account;
using Pal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Extensions
{
    public class CheckCompanyVerifiedAttribute : IAsyncActionFilter
    {


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //// Do something before the action executes.
            //await next();
            //// Do something after the action executes.
            var userType = context.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value;
            if (userType == UserType.Companies.ToString())
            {
                if (int.TryParse(context.HttpContext.User.FindFirst(PalClaimType.CompanyId.ToString()).Value, out int compId))
                {
                    //if (!await _companyService.IsCompanyVerified(compId))
                    //{
                    //    context.Result = new RedirectResult("/Admin/Dashboard/VerifiedPending");
                    //    return;
                    //}
                }
                else
                {
                    //TODO Log
                    context.Result = new RedirectResult("/Account/Login");
                    return;
                }
            }

            await next();
        }
    }
}
