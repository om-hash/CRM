using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Chat;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.Chat;
using Pal.Core.Enums.Notifications;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Chat;
using Pal.Data.DTOs.Notifications;
using Pal.Services.DataServices.Customers;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pal.Services.DataServices.Employees;
using Pal.Services.CRM.Calls;
using Pal.Data.DTOs.CRM.Call;
using Pal.Services.WebWorkContext;

namespace Pal.Services.DataServices.Chat
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerService _logger;
        private readonly INotificationService _notificationService;
        private readonly ICustomersService _customerService;
        private readonly ICallService _callService;
        private readonly IMapper _mapper;
        private readonly IEmployeesService _employeesService;
        private readonly IWebWorkContext _webWorkContext;

        //---------------------------------------------------------------------------------
        public ChatService(ApplicationDbContext context, ILoggerService logger,
            INotificationService notificationService, IMapper mapper, IWebWorkContext webWorkContext,
            ICustomersService customerService, IEmployeesService employeesService, ICallService callService)
        {
            _context = context;
            _logger = logger;
            _notificationService = notificationService;
            _mapper = mapper;
            _customerService = customerService;
            _employeesService = employeesService;
            _callService = callService;
            _webWorkContext = webWorkContext;
        }

        //---------------------------------------------------------------------------------
        public async Task<ChatMessageDTO> SendMsg(ChatMessageDTO model)
        {
            try
            {
                model.Id = new Guid();
                model.MsgDateUtc = DateTime.UtcNow;
                var chatMsg = _mapper.Map<ChatMessage>(model);
                var chatInbox = await GetChatInbox(model);
                if (chatMsg.InboxId == 0)
                {
                    chatMsg.InboxId = chatInbox.Id;
                }
                chatInbox.LastMsgDate = DateTime.Now;
                chatInbox.LastMsgContent = model.Content.Substring(0, model.Content.Length > 20 ? 20 : model.Content.Length);
                await _context.AddAsync(chatMsg);
                await _context.SaveChangesAsync();

                model.InboxId = chatMsg.InboxId;
                //model.ContactName = chatMsg.ChatInbox.ReferenceName2;
                await SendNotification(model, "");
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendMsg), ex);
                return null;
            }
        }

        //-----------------------------------------------------------------
        private async Task<ChatInbox> GetChatInbox(ChatMessageDTO model)
        {
            try
            {
                if (!await _context.ChatInbox.AnyAsync(a => a.ChatType == model.ChatType && a.ReferenceId1 == model.ReferenceId1 && a.ReferenceId2 == model.ReferenceId2))
                {
                    string ReferenceName1 = "";
                    string ReferenceName2 = "";

                    switch (model.ChatType)
                    {
                        case ChatType.AdminCompany:
                            ReferenceName1 = "Admin";
                            //ReferenceName2 = await _companyService.GetCompanyName(int.Parse(model.ReferenceId2));
                            break;

                        case ChatType.AdminCustomer:
                            ReferenceName1 = "Admin";
                            ReferenceName2 = await _customerService.GetCustomerName(int.Parse(model.ReferenceId2));
                            break;

                        case ChatType.CompanyCustomer:
                            //ReferenceName1 = await _companyService.GetCompanyName(int.Parse(model.ReferenceId1));
                            ReferenceName2 = await _customerService.GetCustomerName(int.Parse(model.ReferenceId2));
                            break;

                        case ChatType.AdvisorCustomer:
                            //ReferenceName1 = await _advisorService.GetAdvisorName(int.Parse(model.ReferenceId1));
                            ReferenceName2 = await _customerService.GetCustomerName(int.Parse(model.ReferenceId2));
                            break;

                        case ChatType.LawyerCustomer: 
                            //ReferenceName1 = await _lawyerService.GetLawyerName(int.Parse(model.ReferenceId1));
                            ReferenceName2 = await _customerService.GetCustomerName(int.Parse(model.ReferenceId2));
                            break;



                    }
                    // Ading New ChatInbox
                    ChatInbox chatInbox = new()
                    {
                        ChatType = model.ChatType,
                        LastMsgContent = model.Content.Substring(0, model.Content.Length > 20 ? 20 : model.Content.Length),
                        LastMsgDate = DateTime.Now,
                        ReferenceId1 = model.ReferenceId1,
                        ReferenceId2 = model.ReferenceId2,
                        ReferenceName1 = ReferenceName1,
                        ReferenceName2 = ReferenceName2
                    };

                                         
                    await _context.AddAsync(chatInbox);
                    await _context.SaveChangesAsync();
                    return chatInbox;
                }
                else
                {
                    return await _context.ChatInbox.Where(a => a.ChatType == model.ChatType && a.ReferenceId1 == model.ReferenceId1 && a.ReferenceId2 == model.ReferenceId2).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetChatInbox), ex);
                return null;
            }


        }


        //---------------------------------------------------------------------------------
        public async Task<List<ChatMessage>> GetChatMessages(ChatType ChatType, string ReferenceId1, string ReferenceId2, string userId, bool role)
        {
            try
            {
                var userType = _webWorkContext.GetUserType();
                if ((ChatType == ChatType.AdminCompany || ChatType == ChatType.AdminCustomer) && !role && userType == UserType.Admins.ToString())
                {
                    var empId = await _employeesService.GetEmployeeIdByUerIdAsync(userId);
                    var chat = await _context.ChatInbox.FirstOrDefaultAsync(a =>
                            a.ChatType == ChatType &&
                            a.ReferenceId1 == ReferenceId1 &&
                            a.ReferenceId2 == ReferenceId2);
                 
                    if (empId == 0)
                        return null;
                    if (chat.EmployeeId == 0)
                    {
                        // TODO: Add a call when new chat has been created 
                        if (chat.ChatType == ChatType.AdminCustomer)
                        {
                            CallDTO call = new CallDTO
                            {
                                CustomerId = long.Parse(ReferenceId2),
                                EmployeeId = empId,
                                CallTypeId = 1,
                                IsScheduled = false,
                                IsAssignedToTask = false,
                            };

                            await _callService.AddAsync(call);

                        }
                        chat.EmployeeId = empId;
                        _context.ChatInbox.Update(chat);
                        await _context.SaveChangesAsync();
                    }
                    else if (chat.EmployeeId != empId)
                        return null;
                }

                var masseges = await _context.ChatMessages.Where(a =>
                            a.ChatInbox.ChatType == ChatType &&
                            a.ChatInbox.ReferenceId1 == ReferenceId1 &&
                            a.ChatInbox.ReferenceId2 == ReferenceId2)
                    .ToListAsync();

                //if (masseges.Count == 0)
                //{
                //               var inbox =  new ChatInbox()
                //                {
                //                     ReferenceId1 = ReferenceId1,
                //                     ReferenceId2 = ReferenceId2,
                //                     ChatType = ChatType,
                //                };

                //                _context.ChatInbox.Add(inbox);
                //                await _context.SaveChangesAsync();
                //}

                return masseges;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetChatMessages), ex);
                return null;
            }
        }


        //---------------------------------------------------------------------------------
        public async Task<List<ChatMessage>> GetChatMessages(int inboxId)
        {
            try
            {
                //TODO Check if user has authority to view this chat

                var masseges = await _context.ChatMessages.Where(a => a.InboxId == inboxId).ToListAsync();

              
                return masseges;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetChatMessages), ex);
                return null;
            }
        }



        //---------------------------------------------------------------------------------
        public async Task<List<ChatInboxListDTO>> GetAdminInbox(ChatType chatType, int empId, string role)
        {
            try
            {
                UserType receiverType = UserType.Companies;
                switch (chatType)
                {
                    case ChatType.AdminCompany:
                        receiverType = UserType.Companies;
                        break;
                    case ChatType.AdminCustomer:
                        receiverType = UserType.Customers;
                        break;

                }
                var query = _context.ChatInbox.Where(a => a.ChatType == chatType).AsQueryable().AsNoTracking();
                if (role != "SuperAdmin")
                    query = query.Where(a => a.EmployeeId == empId || a.EmployeeId == 0);

                List<ChatInboxListDTO> data = await query.Select(a => new ChatInboxListDTO
                {
                    InboxId = a.Id,
                    ChatType = a.ChatType,
                    ContactName = a.ReferenceName2,
                    LastMsgContent = a.LastMsgContent,
                    LastMsgDate = a.LastMsgDate,
                    Ref1 = a.ReferenceId1,
                    Ref2 = a.ReferenceId2,
                    SenderType = UserType.Admins,
                    ReceiverType = receiverType,
                }).OrderByDescending(a => a.LastMsgDate).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAdminInbox), ex);
                return null;
            }
        }

        //---------------------------------------------------------------------------------
        public async Task<List<ChatInboxListDTO>> GetCompanyInbox(ChatType chatType, int companyId)
        {
            try
            {
                UserType receiverType = UserType.Customers;
                //switch (chatType)
                //{
                //    case ChatType.AdminCompany:
                //        receiverType = UserType.Admins;
                //        break;
                //    case ChatType.CompanyCustomer:
                //        receiverType = UserType.Customers;
                //        break;

                //}
                List<ChatInboxListDTO> data = await _context.ChatInbox.Where(a => a.ChatType == chatType && a.ReferenceId1 == companyId.ToString()).Select(a => new ChatInboxListDTO
                {
                    ChatType = a.ChatType,
                    ContactName = a.ReferenceName2,
                    LastMsgContent = a.LastMsgContent,
                    LastMsgDate = a.LastMsgDate,
                    Ref1 = a.ReferenceId1,
                    Ref2 = a.ReferenceId2,
                    SenderType = UserType.Companies,
                    ReceiverType = receiverType,
                }).OrderByDescending(a => a.LastMsgDate).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAdminInbox), ex);
                return null;
            }
        }
        //---------------------------------------------------------------------------------
        // Not Used!!
        private async Task<bool> SendNotification(ChatMessageDTO model, string referenceName)
        {
            try
            {

                var notiGroupId = "";

                UserType notificationFor = UserType.Admins;
                switch (model.ChatType)
                {
                    case ChatType.AdminCompany:

                        if (model.SenderType == UserType.Admins)
                        {
                            // المتلقي هو الشركة العقارية
                            notiGroupId = UserType.Companies.ToString() + model.ReferenceId2;
                            notificationFor = UserType.Companies;
                        }
                        else
                        { notiGroupId = UserType.Admins.ToString(); }
                        break;

                    case ChatType.AdminCustomer:

                        if (model.SenderType == UserType.Admins)
                        {
                            // المتلقي هو الزبون
                            notiGroupId = UserType.Customers.ToString() + model.ReferenceId2;
                            notificationFor = UserType.Customers;
                        }
                        else
                            notiGroupId = UserType.Admins.ToString();
                        break;

                    case ChatType.CompanyCustomer:
                        if (model.SenderType == UserType.Companies)
                        {
                            // المتلقي هو الزبون
                            notiGroupId = model.ReferenceId2;
                            notificationFor = UserType.Customers;
                        }
                        else
                            notiGroupId = UserType.Companies.ToString() + model.ReferenceId1;
                        break;

                    case ChatType.AdvisorCustomer:
                        if (model.SenderType == UserType.Adviser)
                        {
                            // المتلقي هو الزبون
                            notiGroupId = UserType.Customers.ToString() + model.ReferenceId2;
                            notificationFor = UserType.Customers;
                        }
                        else
                            notiGroupId = UserType.Adviser.ToString() + model.ReferenceId1;
                        break;

                }
                await _notificationService.SendNotificationIfNotExistAsync(new NotificationDTO
                {
                    NotificationTypeId = (int)NotificationTypes.ToAll_YouHaveANewMsg,
                    GroupId = notiGroupId,
                    NotificationFor = notificationFor,
                    Url = "/Admin/Chat/Inbox?chatType=" + (int)model.ChatType,
                    NotificationParams = new List<NotificationParamDTO>
                    {
                        new NotificationParamDTO{Param =referenceName}
                    },
                });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendNotification), ex);
                return false;
            }
        }
        //---------------------------------------------------------------------------------
        public async Task<List<ChatInboxListDTO>> GetAllInboxForAdmin()
        {
            try
            {
                var query = _context.ChatInbox.AsQueryable().AsNoTracking();
                List<ChatInboxListDTO> data = await query.Select(a => new ChatInboxListDTO
                {
                    InboxId = a.Id,
                    ChatType = a.ChatType,
                    Ref1Name = a.ReferenceName1,
                    ContactName = a.ReferenceName2,
                    LastMsgContent = a.LastMsgContent,
                    LastMsgDate = a.LastMsgDate,
                    Ref1 = a.ReferenceId1,
                    Ref2 = a.ReferenceId2,
                    SenderType = UserType.Admins,
                }).OrderByDescending(a => a.LastMsgDate).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAdminInbox), ex);
                return null;
            }
        }
        //---------------------------------------------------------------------------------

        public async Task<List<ChatMessage>> GetMassegesForAdmin(int ChatId)
        {
            try
            {
                var masseges = await _context.ChatMessages.Where(a => a.InboxId == ChatId).ToListAsync();
                return masseges;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetMassegesForAdmin), ex);
                return null; ;
            }
        }

        #region Public UI

        //---------------------------------------------------------------------------------
        public async Task<IList<ChatInboxListDTO>> GetCustomerInbox(ChatType chatType, long customerId)
        {
            try
            {
                UserType receiverType = UserType.Companies;
                switch (chatType)
                {
                    case ChatType.CompanyCustomer:
                        receiverType = UserType.Companies;
                        break;
                    case ChatType.AdminCustomer:
                        receiverType = UserType.Admins;
                        break;
                    case ChatType.AdvisorCustomer:
                        receiverType = UserType.Adviser;
                        break;
                    case ChatType.LawyerCustomer:
                        receiverType = UserType.Lawyer;
                        break;
                }
                IList<ChatInboxListDTO> data = await _context.ChatInbox.Where(a => a.ChatType == chatType && a.ReferenceId2 == customerId.ToString()).Select(a => new ChatInboxListDTO
                {
                    InboxId = a.Id,
                    ChatType = a.ChatType,
                    ContactName = a.ReferenceName1,
                    LastMsgContent = a.LastMsgContent,
                    LastMsgDate = a.LastMsgDate,
                    Ref1 = a.ReferenceId1,
                    Ref2 = a.ReferenceId2,
                    SenderType = UserType.Customers,
                    ReceiverType = receiverType,
                }).OrderByDescending(a => a.LastMsgDate).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAdminInbox), ex);
                return null;
            }
        }

        //---------------------------------------------------------------------------------

        public async Task<List<ChatInboxListDTO>> GetCustomerInboxForAdviosor(ChatType chatType, int AdviosorId)
        {
            try
            {
                UserType receiverType = UserType.Customers;
                List<ChatInboxListDTO> data = await _context.ChatInbox.Where(a => a.ChatType == chatType && a.ReferenceId1 == AdviosorId.ToString()).Select(a => new ChatInboxListDTO
                {
                    InboxId = a.Id,
                    ChatType = a.ChatType,
                    ContactName = a.ReferenceName1,
                    LastMsgContent = a.LastMsgContent,
                    LastMsgDate = a.LastMsgDate,
                    Ref1 = a.ReferenceId1,
                    Ref2 = a.ReferenceId2,
                    SenderType = UserType.Adviser,
                    ReceiverType = receiverType,
                }).OrderByDescending(a => a.LastMsgDate).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAdminInbox), ex);
                return null;
            }
        }
        //---------------------------------------------------------------------------------
        public async Task<List<ChatInboxListDTO>> GetCustomerInboxForLawyer(ChatType chatType, int LawyerId)
        {
            try
            {
                UserType receiverType = UserType.Customers;
                List<ChatInboxListDTO> data = await _context.ChatInbox.Where(a => a.ChatType == chatType && a.ReferenceId1 == LawyerId.ToString()).Select(a => new ChatInboxListDTO
                {
                    InboxId = a.Id,
                    ChatType = a.ChatType,
                    ContactName = a.ReferenceName1,
                    LastMsgContent = a.LastMsgContent,
                    LastMsgDate = a.LastMsgDate,
                    Ref1 = a.ReferenceId1,
                    Ref2 = a.ReferenceId2,
                    SenderType = UserType.Adviser,
                    ReceiverType = receiverType,
                }).OrderByDescending(a => a.LastMsgDate).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAdminInbox), ex);
                return null;
            }
        }
        #endregion
    }
}