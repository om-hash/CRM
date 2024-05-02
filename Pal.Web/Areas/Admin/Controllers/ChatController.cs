using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Domains.Account;
using Pal.Core.Domains.Chat;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.Chat;
using System.Linq;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Chat;
using Pal.Services.DataServices.Chat;
using Pal.Services.DataServices.Employees;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [ServiceFilter(typeof(CheckCompanyVerifiedAttribute))]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly ILoggerService _logger;
        private readonly IEmployeesService _employeesService;
        private readonly IWebWorkContext _webWorkContext;
        private readonly ApplicationDbContext _sysdb;
        private readonly UserManager<ApplicationUser> _userManager;

        //------------------------------------------------------------------------
        public ChatController(ILanguageService languageService, ILocalizationService localizationService, 
            IChatService chatService, ILoggerService logger,
              UserManager<ApplicationUser> userManager,
            IEmployeesService employeesService, IWebWorkContext webWorkContext, ApplicationDbContext sysdb)
            : base(languageService, localizationService)
        {
            _chatService = chatService;
            _logger = logger;
            _employeesService = employeesService;
            _userManager = userManager;
            _webWorkContext = webWorkContext;
            _sysdb = sysdb;
        }


        //------------------------------------------------------------------------
        public async Task<IActionResult> Index(ChatType chatType, UserType senderType, string senderId,
            UserType receiverType, string receiverId,
            string ref1, string ref2)
        {
            try
            {
                if (CheckParametters(chatType, senderType, senderId, receiverType, receiverId, ref1, ref2))
                {
                    //var model = await _chatService.GetChatMessages(chatType, ref1, ref2);
                    List<ChatMessage> model = new List<ChatMessage>();
                    //if (chatType == ChatType.All)
                    //{
                    //    if (User.IsInRole(PalRoles.SuperAdmin.ToString()))
                    //    {
                    //        model = await _chatService.GetMassegesForAdmin()
                    //    }
                    //}
                   

                    if(senderType == UserType.Admins)
                    {
                        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                        var role = User.IsInRole("SuperAdmin");

                         model = await _chatService.GetChatMessages(chatType, ref1, ref2, userId, role);
                    }
                    else
                         model = await _chatService.GetChatMessages(chatType, ref1, ref2, null, false);


                    return View(model);
                }
                else
                {
                    return Redirect("/");
                }
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("ChatController" + nameof(Inbox), ex);
                return NotFound();
            }



        }
        ////------------------------------------------------------------------------
        //public async Task<IActionResult> Index2(ChatType chatType, UserType senderType, string senderId,
        //    UserType receiverType, string receiverId,
        //    string ref1, string ref2)
        //{
        //    try
        //    {
        //        if (CheckParametters(chatType, senderType, senderId, receiverType, receiverId, ref1, ref2))
        //        {
        //            var model = await _chatService.GetChatMessages(chatType, ref1, ref2);
        //            return View(model);
        //        }
        //        else
        //        {
        //            return Redirect("/");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = _logger.LogErrorAsync("ChatController" + nameof(Index2), ex);
        //        return NotFound();
        //    }



        //}

        //------------------------------------------------------------------------
        private bool CheckParametters(ChatType chatType, UserType senderType, string senderId,
            UserType receiverType, string receiverId,
            string ref1, string ref2)
        {
            try
            {
                var ss = User.FindFirst(PalClaimType.UserType.ToString()).Value;
                string whoAmI = User.FindFirst(PalClaimType.UserType.ToString()).Value;
                switch (whoAmI)
                {
                    case nameof(UserType.Admins):
                        break;

                    case nameof(UserType.Companies):
                        if (senderType != UserType.Companies)
                            return false;



                        var compId = User.FindFirst(PalClaimType.CompanyId.ToString()).Value;
                        if (chatType != ChatType.CompanyCustomer && compId != ref2)
                            return false;

                        if (chatType != ChatType.AdminCompany && compId != ref1)
                            return false;

                        if (compId != senderId)
                            return false;

                        if (chatType != ChatType.AdminCompany && chatType != ChatType.CompanyCustomer)
                            return false;

                        if (chatType == ChatType.AdminCompany && (receiverType != UserType.Admins))
                            return false;

                        if (chatType == ChatType.CompanyCustomer && receiverType != UserType.Customers)
                            return false;

                        break;
                    case nameof(UserType.RealStateAgents):
                        return false;


                    case nameof(UserType.Customers):
                        if (senderType != UserType.Customers)
                            return false;

                        var customerId = User.FindFirst(PalClaimType.CustomerId.ToString()).Value;
                        if (customerId != ref2 || customerId != senderId)
                            return false;

                        if (chatType != ChatType.AdminCustomer && chatType != ChatType.AdvisorCustomer && chatType != ChatType.CompanyCustomer)
                            return false;

                        if (chatType == ChatType.AdminCustomer && (receiverType != UserType.Admins))
                            return false;

                        if (chatType == ChatType.AdvisorCustomer && receiverType != UserType.Adviser)
                            return false;

                        if (chatType == ChatType.CompanyCustomer && receiverType != UserType.Companies)
                            return false;
                        break;

                    case nameof(UserType.Adviser):
                        if (senderType != UserType.Adviser)
                            return false;
                        break;


                    default:
                        return false;
                }

                ViewData["chatType"] = ((int)chatType);
                ViewData["senderType"] = ((int)senderType);
                ViewData["senderId"] = senderId;
                ViewData["receiverType"] = ((int)receiverType);
                ViewData["receiverId"] = receiverId;
                ViewData["ref1"] = ref1;
                ViewData["ref2"] = ref2;
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("ChatController" + nameof(Inbox), ex);
                return false;
            }


        }


        //------------------------------------------------------------------------
        [Authorize(Roles = "Chat, SuperAdmin")]

        public async Task<IActionResult> Inbox(ChatType chatType)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = User.FindFirst(ClaimTypes.Role).Value;
                var empId = await _employeesService.GetEmployeeIdByUerIdAsync(userId);
                if(empId == 0 && role != "SuperAdmin")
                    return View(new List<ChatInboxListDTO>());
                List<ChatInboxListDTO> model = await _chatService.GetAdminInbox(chatType, empId, role);

                //List<ChatInboxListDTO> model = await _chatService.GetAdminInbox(chatType);

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Inbox), ex);
                return View();
            }
        }

        //------------------------------------------------------------------------

        public async Task<IActionResult> CompanyInbox(ChatType chatType, int companyId)
        {
            try
            {
                List<ChatInboxListDTO> model = await _chatService.GetCompanyInbox(chatType, companyId);

                return View("Inbox", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Inbox), ex);
                return View();
            }
        }
        //------------------------------------------------------------------------
        public async Task<IActionResult> AdvisorInbox(ChatType chatType, int advisorId)
        {
            var u = User;
            try
            {
                List<ChatInboxListDTO> model = await _chatService.GetCustomerInboxForAdviosor(chatType, advisorId);

                return View("Inbox", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Inbox), ex);
                return View();
            }
        }
        //------------------------------------------------------------------------
        public async Task<IActionResult> LawyerInbox(ChatType chatType, int LawyerId)
        {
            try
            {
                List<ChatInboxListDTO> model = await _chatService.GetCustomerInboxForAdviosor(chatType, LawyerId);

                return View("Inbox", model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Inbox), ex);
                return View();
            }
        }
        //--------------------------------------------------
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> AllInboxes()
        {
            try
            {
                List<ChatInboxListDTO> model = await _chatService.GetAllInboxForAdmin();

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Inbox), ex);
                return View();
            }
        }
        //--------------------------------------------------
        public async Task<IActionResult> GetInboxMassages(int id)
        {
            try
            {
               var model = await _chatService.GetMassegesForAdmin(id);
                
                

                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Inbox), ex);
                return View();
            }
        }


        //------------------------------------------------------------------------
        public IActionResult ChatReport(int id, UserType senderType, string senderName, string receiverName)
        {
            WebReport chatReport = new WebReport();
            string path = Directory.GetCurrentDirectory() + "\\Reports\\ChatReport.frx";
            //var entity = await _chatService.GetMassegesForAdmin(id);
            //List<ChatMessageDTO> model = entity.Select(m => new ChatMessageDTO
            //{
            //    Id = m.Id,
            //    Content = m.Content,
            //    MsgDateLocal = m.MsgDateLocal,
            //    SenderId = ((int)m.SenderType).ToString(),
            //}).ToList();
            chatReport.Report.Load(path);
            int sender = ((int)senderType);
            chatReport.Report.SetParameterValue("senderType", sender);
            chatReport.Report.SetParameterValue("SenderName", senderName);
            chatReport.Report.SetParameterValue("RecieverName", receiverName);
            chatReport.Report.SetParameterValue("chatId", id);
            //chatReport.Report.RegisterData(model, "Msg");
            chatReport.Report.Prepare();

            Stream stream = new MemoryStream();

            chatReport.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return new FileStreamResult(stream, "application/pdf");
            //return File(stream, "application/pdf", "test" + ".pdf");

        }
    }
}
