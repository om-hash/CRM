using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Pal.Core.Domains.Chat;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.Chat;
using Pal.Data.DTOs.Chat;
using Pal.Services.DataServices.Chat;
using Pal.Services.Logger;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pal.Services.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ILoggerService _logger;
        public ChatHub(IChatService chatService, ILoggerService logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        //---------------------------------------------------------------------------
        public override Task OnConnectedAsync()
        {
            //// Update Online Status

            try
            {
                var userType = Context.User.FindFirst(PalClaimType.UserType.ToString())?.Value;
                switch (userType)
                {
                    case nameof(UserType.Admins):
                        Groups.AddToGroupAsync(Context.ConnectionId, userType);
                        break;

                    case nameof(UserType.Companies):
                        var compId = Context.User.FindFirst(PalClaimType.CompanyId.ToString()).Value;
                        Groups.AddToGroupAsync(Context.ConnectionId, userType + compId);
                        break;

                    case nameof(UserType.Customers):
                        var customerId = Context.User.FindFirst(PalClaimType.CustomerId.ToString()).Value;
                        Groups.AddToGroupAsync(Context.ConnectionId, userType + customerId);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("ChatHub." +nameof(OnConnectedAsync), ex);
            }
            return base.OnConnectedAsync();
        }

        //---------------------------------------------------------------------------
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Update Offline Status
            return base.OnDisconnectedAsync(exception);
        }


        //---------------------------------------------------------------------------
        public async Task JoinGroup(ChatType chatType, string ref1, string ref2, UserType senderType)
        {
            try
            {
                var groupId = FormatGroupId(chatType, ref1, ref2, senderType);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("ChatHub." + nameof(JoinGroup), ex);
            }
          
        }





        //---------------------------------------------------------------------------
        public async Task test(string body)
        {
            Console.WriteLine(body);
        }
        //---------------------------------------------------------------------------
        public async Task<ChatMessageDTO> SendMsg(string body)
        {
            try
            {
             
                var message = JsonConvert.DeserializeObject<ChatMessageDTO>(body);
                var result = await _chatService.SendMsg(message);


                // send new msg to receiver chat screen
                var groupId = FormatGroupId(result.ChatType, result.ReferenceId1, result.ReferenceId2, result.ReceiverType);
                _ = Clients.Group(groupId).SendAsync("receiveChatMsg", message);
                return result;
                // send msg notification to receiver
                //switch (result.ReceiverType)
                //{
                //    case UserType.Admins:
                //        groupId = UserType.Admins.ToString();
                //        break;


                //    default:
                //        groupId = result.ReceiverType.ToString() + result.ReceiverId;
                //        break;
                //}
                //_ = Clients.Group(groupId).SendAsync("testtest", result);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendMsg), ex);
                return null;
            }
        }

        //---------------------------------------------------------------------------
        private static string FormatGroupId(ChatType chatType, string ref1, string ref2, UserType userType)
        {
            var groupId = string.Format("{0}-{1}-{2}-{3}", chatType, ref1, ref2, userType);
            return groupId;
        }
    }
}
