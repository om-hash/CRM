using Pal.Data.DTOs;
using Pal.Data.DTOs.Company;
using Pal.Data.VMs.Company;

using Syncfusion.EJ2.Base;

using System.Threading.Tasks;

namespace Pal.Services.DataServices.Companies
{
    public interface ICompanyService
    {
        public Task<SyncPaginatedListModel<CompanyListDTO>> GetAllAsListAsync(MyDataManagerRequest dm);
        public Task<CreateNewCompanyDTO> GetByIdAsync(int id);
        Task<int> GetCompanyIdByUserId(string userId);
        public Task<MyResponseResult> CreateNewCompany(CreateNewCompanyDTO createNewCompanyDTO);
        public Task<MyResponseResult> UpdateCompany(CreateNewCompanyDTO createNewCompanyDTO);
        public Task<MyResponseResult> DeleteCompany(int id);
        public Task CompanyApprove(int id);
        public Task CompanyDisApprove(int id);

        #region For View Model
        public Task<CompanyViewModel> GetCompanyByIdForView(int id, int? languageId = null);
        public Task<CompanyViewModel> GetCompanyDetailsByIdForView(int id);
        Task<bool> IsCompanyVerified(int compId);
        Task<string> GetCompanyName(int compId);
        Task<bool> UpdateMyCompany(CreateNewCompanyDTO model);
        Task<bool> AcceptTearmAndConditions(int id);
        Task<bool> CheckIfFirstTime(int id);
        Task<SyncPaginatedListModel<CompanyListDTO>> GetAllAsListToEditAsync(DataManagerRequest dm);
        Task<MyResponseResult> DeletePermenantly(int id);
        Task<MyResponseResult> Restore(int id);
        #endregion
    }
}
