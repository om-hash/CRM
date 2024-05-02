using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Pal.Core.Domains.Account;
using Pal.Core.Domains.Required;
using Pal.Core.Enums.Account;
using Pal.Data.Contexts;
using Pal.Services.Caching;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pal.Services.WebWorkContext
{
    public class WebWorkContext : IWebWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly ICacheService<List<RequiredFields>> _cachedRequiredFields;


        //-----------------------------------------------------------------
        public WebWorkContext(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ICacheService<List<RequiredFields>> cachedRequiredFields)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _cachedRequiredFields = cachedRequiredFields;
        }

        /// <summary>
        /// Get language from the request
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the found language
        /// </returns>





        //-----------------------------------------------------------------
        public string GetUserType()
        {
            try
            {
                var userTypeString = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value;
                return userTypeString;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetUserType), ex);
                return null;
            }
        }


        //-----------------------------------------------------------------
        public string GetMyCustomerId()
        {
            try
            {
                if (_httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.CustomerId.ToString()) != null)
                {
                    var customerString = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.CustomerId.ToString()).Value;
                    return customerString;
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetMyCustomerId), ex);
                return "0";
            }
        }


        //-----------------------------------------------------------------
        public string GetMyCompanyId()
        {
            try
            {
                try
                {
                    var result = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.CompanyId.ToString())?.Value ?? "0";
                    return result;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetMyCompanyId), ex);
                return "0";
            }
        }
        public string GetMySalesId()
        {
            try
            {
                try
                {
                    var result = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.CompanyId.ToString())?.Value ?? "0";
                    return result;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetMyCompanyId), ex);
                return "0";
            }
        }



        //-----------------------------------------------------------------------------------------

        public async Task<ApplicationUser> GetMyUserDetails()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);
                return user;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetMyUserDetails), ex, Importance.VeryHigh);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------
        public async Task<string> GetMyUserFullName(string userId)
        {
            try
            {
                var user = await _context.Users.Select(u => new { u.Id, u.FullName }).FirstOrDefaultAsync(a => a.Id == userId);
                return user.FullName;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetMyUserDetails), ex, Importance.VeryHigh);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------
        public async Task<string> GetCustomerIdByUserId(string userId)
        {
            try
            {
                var CustomerId = await _context.Users.Where(x => x.Id == userId).Select(a => a.ReferenceId).FirstOrDefaultAsync();
                return CustomerId;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetCustomerIdByUserId), ex, Importance.VeryHigh);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------
        public async Task<string> GetCustomerEmailById(string id)
        {
            try
            {
                var Email = await _context.Users.Where(x => x.UserType == UserType.Customers && x.ReferenceId == id).Select(x => x.Email).FirstOrDefaultAsync();
                return Email;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetCustomerEmailById), ex, Importance.VeryHigh);
                return null;
            }
        }

        //----------------------------------------------------------------------------------------------
        public async Task<int> GetEmployeeIdByUserId(string id)
        {
            try
            {
                var Id = await _context.Employees.Where(x => x.UserId == id).Select(x => x.Id).FirstOrDefaultAsync();
                return Id;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetEmployeeIdByUserId), ex, Importance.VeryHigh);
                return 0;
            }
        }

        //----------------------------------------------------------------------------------------------
        public string GetMyUserId()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userId;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetMyUserId), ex, Importance.VeryHigh);
                return null;
            }
        }

        //-------------------------------------------------------------------------------------------
        public bool IsUserEmployee(string id)
        {
            try
            {
                var result = _context.Employees.Any(a => a.UserId == id);
                return result;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(IsUserEmployee), ex, Importance.VeryHigh);
                return false;
            }
        }

        //-------------------------------------------------------------------------------------------
        public string GetUserIdByAdvisorId(int id)
        {
            try
            {
                var UserID = _context.Users.Where(x => x.ReferenceId == id.ToString() && x.UserType == UserType.Adviser).Select(a => a.Id).FirstOrDefault();
                return UserID;
            }
            catch (Exception)
            {
                //_ = _logger.LogErrorAsync(nameof(GetUserIdByAdvisorId), ex, Importance.VeryHigh);
                return "";
            }
        }

        //============//============//============//============//============//============//============
        // Not Used
        public async Task<bool> IsFieldRequiredAsync(string module, string field)
        {
            try
            {
                var cashedData = await _cachedRequiredFields.GetAsync("RequiredFields");
                if (cashedData == null)
                {
                    cashedData = await _context.RequiredFields.ToListAsync();
                    await _cachedRequiredFields.SetAsync("RequiredFields", cashedData, TimeSpan.FromMinutes(60));
                }
                var isRequired = cashedData.Any(a => a.ReferenceType == module && a.Required.Contains(field.Trim()));
                return isRequired;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //============//============//============//============//============
        public string IsFieldRequired(string module, string field)
        {
            try
            {
                var cashedData = _cachedRequiredFields.GetAsync("RequiredFields").Result;
                if (cashedData == null)
                {
                    cashedData = _context.RequiredFields.ToList();
                    _cachedRequiredFields.SetAsync("RequiredFields", cashedData, TimeSpan.FromMinutes(60)).Wait();
                }
                var isRequired = cashedData.Any(a => a.ReferenceType == module
                        && a.Required.Split(",").Any(a => a == field.Trim())
                        );
                return isRequired ? "*" : "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

}
