using Pal.Core.Domains;
using Pal.Core.Domains.Deals;
using Pal.Core.Enums;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Deals;
using Syncfusion.EJ2.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.CRM.Deals
{
    public interface IDealsService : IRepositoryBase<Deal>
    {
        Task<int> AddAsync(DealDTO model);
        Task<bool> ChangeDealStage(int id, int stageID);
        Task<ResponseType> DeleteAsync(int id);
        Task<SyncPaginatedListModel<DealListDTO>> GetAllAsync(DataManagerRequest dm);
        Task<DealDTO> GetDealByIdAsync(int id);
        Task<DealDetialsDTO> GetDealDetails(int id);
        Task<int> UpdateAsync(DealDTO model);
    }
}
