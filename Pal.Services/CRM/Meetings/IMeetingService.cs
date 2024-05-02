using Pal.Core.Domains;
using Pal.Core.Domains.Communications;
using Pal.Core.Enums;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Meeting;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.CRM.Meetings
{
    public interface IMeetingService : IRepositoryBase<Meeting>
    {
        Task<int> AddAsync(MeetingDTO model);
        Task<ResponseType> DeleteAsync(int id);
        Task<SyncPaginatedListModel<MeetingListDTO>> GetAllAsync(DataManagerRequest dm);
        Task<MeetingDTO> GetMeetingByIdAsync(int id);
        Task<int> UpdateAsync(MeetingDTO model);
    }
}
