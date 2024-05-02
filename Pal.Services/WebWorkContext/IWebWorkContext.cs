using Pal.Core.Domains.Account;
using Pal.Core.Domains.Languages;
using Pal.Core.Enums.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.WebWorkContext
{
    public interface IWebWorkContext
    {
        
        string GetUserType();
        string GetMyCustomerId();
        string GetMyCompanyId();
        string GetMySalesId();
        
        Task<ApplicationUser> GetMyUserDetails();

        Task<string> GetMyUserFullName(string userId);
		Task<string> GetCustomerIdByUserId(string userId);
        Task<string> GetCustomerEmailById(string id);
        Task<int> GetEmployeeIdByUserId(string id);
        string GetMyUserId();
        bool IsUserEmployee(string id);
        string GetUserIdByAdvisorId(int id);
        string IsFieldRequired(string module, string field);
        Task<bool> IsFieldRequiredAsync(string module, string field);
    }
}
