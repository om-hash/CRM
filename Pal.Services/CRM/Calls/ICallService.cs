using Pal.Core.Domains;
using Pal.Core.Domains.Communications;
using Pal.Core.Enums;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Call;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.CRM.Calls
{
    public interface ICallService : IRepositoryBase<Call>
    {
        Task<int> AddAsync(CallDTO model);
        Task<ResponseType> DeleteAsync(int id);
        Task<SyncPaginatedListModel<CallListDTO>> GetAllAsync(DataManagerRequest dm);
        Task<CallDTO> GetCallByIdAsync(int id);
        Task<int> UpdateAsync(CallDTO model);
    }
}
