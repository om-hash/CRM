using Pal.Data.DTOs;
using Pal.Data.DTOs.Sales;
using Pal.Data.VMs.Sales;

using Syncfusion.EJ2.Base;

using System.Threading.Tasks;

namespace Pal.Services.DataServices.Sales
{
    public interface ISalesService
    {
        public Task<SyncPaginatedListModel<SalesListDTO>> GetAllAsListAsync(MyDataManagerRequest dm);
        public Task<CreateNewSalesDTO> GetByIdAsync(int id);
        Task<int> GetSalesIdByUserId(string userId);
        public Task<MyResponseResult> CreateNewSales(CreateNewSalesDTO createNewSalesDTO);
        public Task<MyResponseResult> UpdateSales(CreateNewSalesDTO createNewSalesDTO);
        public Task<MyResponseResult> DeleteSales(int id);
        public Task SalesApprove(int id);
        public Task SalesDisApprove(int id);

        #region For View Model
        public Task<SalesViewModel> GetSalesByIdForView(int id, int? languageId = null);
        public Task<SalesViewModel> GetSalesDetailsByIdForView(int id);
        Task<bool> IsSalesVerified(int compId);
        Task<string> GetSalesName(int compId);
        Task<bool> UpdateMySales(CreateNewSalesDTO model);
        Task<bool> AcceptTearmAndConditions(int id);
        Task<bool> CheckIfFirstTime(int id);
        Task<SyncPaginatedListModel<SalesListDTO>> GetAllAsListToEditAsync(DataManagerRequest dm);
        Task<MyResponseResult> DeletePermenantly(int id);
        Task<MyResponseResult> Restore(int id);
        #endregion
    }
}
