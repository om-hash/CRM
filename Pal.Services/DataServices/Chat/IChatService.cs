using Pal.Core.Domains.Chat;
using Pal.Core.Enums.Chat;
using Pal.Data.DTOs.Chat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.Chat
{
    public interface IChatService
    {
        Task<List<ChatInboxListDTO>> GetAdminInbox(ChatType chatType, int empId, string role);
        Task<List<ChatInboxListDTO>> GetCompanyInbox(ChatType chatType, int companyId);
        Task<List<ChatMessage>> GetChatMessages(ChatType ChatType, string ReferenceId1, string ReferenceId2, string userId, bool role);
        Task<ChatMessageDTO> SendMsg(ChatMessageDTO model);


        #region Public UI Methods

        Task<IList<ChatInboxListDTO>> GetCustomerInbox(ChatType chatType, long customerId);
		Task<List<ChatInboxListDTO>> GetCustomerInboxForAdviosor(ChatType chatType, int AdviosorId);
        Task<List<ChatMessage>> GetMassegesForAdmin(int ChatId);
        Task<List<ChatInboxListDTO>> GetAllInboxForAdmin();
        Task<List<ChatMessage>> GetChatMessages(int inboxId);

        #endregion
    }
}