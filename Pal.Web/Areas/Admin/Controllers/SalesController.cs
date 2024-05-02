using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Pal.Core.Domains.Account;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Email;
using Pal.Core.Enums.Roles;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Sales;
using Pal.Services;
using Pal.Services.DataServices.Sales;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Email;
using Pal.Services.ExcelManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.PalFunctions;
using Pal.Services.WebWorkContext;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Linq;
using Syncfusion.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SalesController : BaseController
    {
        private readonly ISalesService _SalesService;
        private readonly ILoggerService _logger;
        private readonly ILookupsService _lookupsService;
        private readonly ApplicationDbContext _sysdb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IWebWorkContext _webWorkContext;
        private readonly IExcel _Excel;


        //----------------------------------------------------------------------------------------
        public SalesController(ILookupsService lookupsService, ISalesService SalesService,
            ILanguageService languageService, ILocalizationService localizationService,
            INotificationService _notificationService, ILoggerService logger,
            ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            IEmailService emailService, IWebWorkContext webWorkContext, IExcel Excel)
            : base(languageService, localizationService)
        {
            _SalesService = SalesService;
            _logger = logger;
            _lookupsService = lookupsService;
            _sysdb = db;
            _userManager = userManager;
            _emailService = emailService;
            _webWorkContext = webWorkContext;
            _Excel = Excel;
        }


        #region Admin Transactions
        //----------------------------------------------------------------------------------------
        private async Task GetComboBoxes()
        {
            ViewBag.SalesCateogries = (await _lookupsService.GetSyssSalesCategories()).ToSelectList();




        }
        //----------------------------------------------------------------------------------------

        [Authorize(Roles = "Sales_List, SuperAdmin")]
        public async Task<IActionResult> SalesList()
        {
            try
            {

                ViewBag.SalesCateogries = (await _lookupsService.GetSyssSalesCategories()).ToSelectList();
                ViewBag.City = (await _lookupsService.GetSysCity()).ToSelectList();
                ViewBag.Countries = (await _lookupsService.GetSysCountries()).ToSelectList();

                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesList), ex);
                return NotFound();
            }

        }

        //----------------------------------------------------------------------------------------
        [Authorize(Roles = "Sales_List, SuperAdmin")]
        public async Task<IActionResult> SalesEditableList()
        {
            try
            {
                ViewBag.SalesCateogries = (await _lookupsService.GetSyssSalesCategories()).ToSelectList();
                ViewBag.City = (await _lookupsService.GetSysCity()).ToSelectList();
                ViewBag.Countries = (await _lookupsService.GetSysCountries()).ToSelectList();
                //var model = await _SalesService.GetAllAsListAsync(new DataManagerRequest(), false);

                return View();

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesList), ex);
                return NotFound();
            }

        }

        //----------------------------------------------------------------------------------------
        [Authorize(Roles = "Sales_List, SuperAdmin")]

        public async Task<IActionResult> SalesEditableListData([FromBody] DataManagerRequest dm)
        {
            try
            {

                var model = await _SalesService.GetAllAsListToEditAsync(new DataManagerRequest());

                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesList), ex);
                return NotFound();
            }

        }
        //----------------------------------------------------------------------------------------

        [Authorize(Roles = "Sales_List, SuperAdmin")]
        public async Task<IActionResult> SalesPaginatedList([FromBody] MyDataManagerRequest dm)
        {
            try
            {
                var model = await _SalesService.GetAllAsListAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesList), ex);
                return NotFound();
            }

        }

        //----------------------------------------------------------------------------------------

        [Authorize(Roles = "Sales_Add, SuperAdmin")]
        public async Task<IActionResult> SalesAdd()
        {
            try
            {
                await GetComboBoxes();
                var model = new CreateNewSalesDTO
                {
                    CountryId = 1,
                    CityId = 1,
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesAdd), ex);
                return NotFound();
            }

        }


        //----------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalesAdd(CreateNewSalesDTO createNewSalesDTO)
        {
            using var transaction = _sysdb.Database.BeginTransaction();
            try
            {
                if (createNewSalesDTO == null)
                    return View();

                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.ModelNotValid, Localize("account.resgister.NotValid").ToString()));




                if (IsEmailExist(createNewSalesDTO.Email))
                {
                    ModelState.AddModelError(string.Empty, Localize("account.resgister.emailexist").ToString());
                    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.emailexist").ToString()));
                }

                if (IsUserNameExist(createNewSalesDTO.UserName.Trim()))
                {
                    ModelState.AddModelError(string.Empty, Localize("account.resgister.usernamexist").ToString());
                    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.usernamexist").ToString()));
                }

                ApplicationUser user = new()
                {
                    Email = createNewSalesDTO.Email,
                    UserName = createNewSalesDTO.UserName.Replace(" ", ""),
                    FullName = createNewSalesDTO.OwnerName,
                    PhoneNumber = createNewSalesDTO.Phone,
                    WhatsappNumber = createNewSalesDTO.WhatsApp,
                    UserType = UserType.Sales,
                };

                if (createNewSalesDTO.IsEmailAutoActivation)
                    user.EmailConfirmed = true;
                if (createNewSalesDTO.IsPhoneNumberAutoActivation)
                    user.PhoneNumberConfirmed = true;

                var result = await _userManager.CreateAsync(user, createNewSalesDTO.Password);
                if (result.Succeeded)
                {
                    var addComp = await _SalesService.CreateNewSales(createNewSalesDTO);
                    if (!addComp.IsSuccess)
                        return Json(addComp);


                    user.ReferenceId = addComp.Data.ToString();

                    await _userManager.UpdateAsync(user);

                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
                        new Claim(type: PalClaimType.SalesId.ToString(), value: user.ReferenceId.ToString()),
                            new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber ?? "")
                    };
                    await _userManager.AddClaimsAsync(user, claims);

                    #region Roles
                    //#region Project
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_List.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Add.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Edit.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Remove.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Repeat.ToString());
                    //#endregion
                    //#region RealEstate
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_List.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Add.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Edit.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Remove.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Repeat.ToString());
                    //#endregion
                    #region Chat
                    //await _userManager.AddToRoleAsync(user, PalRoles.Chat.ToString());
                    #endregion
                    //#region MyRegion
                    //await _userManager.AddToRoleAsync(user, PalRoles.Sales_Edit.ToString());
                    //#endregion



                    #endregion


                    if (!createNewSalesDTO.IsEmailAutoActivation)
                        await SendEmailConfirmation(user);

                    await transaction.CommitAsync();
                    return Json(new MyResponseResult { IsSuccess = true, Data = addComp.Data });
                }
                else //if there are errors in signin
                {
                    var errors = new List<object>();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", Localize($"account.{error.Code}").Value);
                        errors.Add((Localize($"account.{error.Code}").Value));
                    }

                    return Json(new MyResponseResult { IsSuccess = false, Errors = new List<string> { errors[0].ToString() } }); ;
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesAdd), ex);
                return Json(MyResponseResult.Error(ex, System.Net.HttpStatusCode.InternalServerError));
            }


        }


        //---------------------------------------------------------------------------
        [Authorize(Roles = "Sales_Add, SuperAdmin")]
        //[Route("UploadList")]
        //[HttpPost]
        public IActionResult UploadListAsync(IFormFile ExcelFile)
        {
            try
            {
                string filePath = _Excel.Documentupload(ExcelFile);

                DataTable excelDT = _Excel.excelToDt(filePath);
                DataTable separetedCols = new DataTable();

                separetedCols = excelDT.Copy();
                separetedCols.TableName = "SalesTranslate";

                IEnumerable<DataColumn> cols = separetedCols.Columns.Cast<DataColumn>();

                cols.ToList().ForEach(
                (col) =>
                {
                    if (col.ColumnName != "SalesName") { separetedCols.Columns.Remove(col); }
                });

                separetedCols.Columns.Add("Id");
                excelDT.Columns.Remove("SalesName");

                var cmdText =
                $@"INSERT INTO {excelDT.TableName} 
                ([SalesCategoryId] ,[FullAddress] ,[Phone] ,[WhatsApp] 
                ,[Email] ,[Website] ,[ManagedBy] ,[ManagerName] ,[ManagerEmail] ,[ApproveState] 
                ,[IsActive] ,[ApprovedBy] ,[ApprovedDate] ,[TaxNumber], [IsDeleted])
                OUTPUT INSERTED.Id
                VALUES (NULLIF(@SalesCategoryId, NULL) ,NULLIF(@FullAddress, NULL) ,NULLIF(@Phone, NULL) ,
			    NULLIF(@WhatsApp, 'hhhhhh') ,NULLIF(@Email, NULL) ,NULLIF(@Website, NULL) ,NULLIF(@ManagedBy, NULL) ,
                NULLIF(@ManagerName, NULL) ,NULLIF(@ManagerEmail, NULL) ,NULLIF(@ApproveState, NULL) ,1 ,
                NULLIF(@ApprovedBy, NULL) , NULLIF(@ApprovedDate, NULL) ,NULLIF(@TaxNumber, NULL), 0)";
                separetedCols = _Excel.ImportDocument(excelDT, cmdText, separetedCols);

                cmdText =
                $@"INSERT INTO SalesTranslate 
                ([SalesName], [CompId], [LanguageId]) OUTPUT INSERTED.Id
                VALUES (NULLIF(@SalesName, NULL) ,NULLIF(@Id, NULL), 1)";
                _Excel.ImportDocument(separetedCols, cmdText);

                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(SalesAdd), ex);
                return NotFound();
            }

        }
        //----------------------------------------------------------------------------------------
        [Authorize(Roles = "Sales_List, SuperAdmin")]
        public async Task<IActionResult> SalesUpdate(int id)
        {
            try
            {
                await GetComboBoxes();
                var model = await _SalesService.GetByIdAsync(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesUpdate), ex);
                return NotFound();
            }

        }

        //----------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sales_Edit, SuperAdmin")]
        public async Task<IActionResult> ComapnyUpdate(CreateNewSalesDTO createNewSalesDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.ModelNotValid, "ModelNotValid"));

                var resopns = await _SalesService.UpdateSales(createNewSalesDTO);
                return Json(resopns);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(ComapnyUpdate), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }

        //----------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sales_Delete, SuperAdmin")]
        public async Task<IActionResult> SalesDelete(int id)
        {
            try
            {
                var resopns = await _SalesService.DeleteSales(id);
                return Json(resopns);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }
        [Authorize(Roles = "Sales_Approvement, SuperAdmin")]
        public async Task<IActionResult> SalesApprove(int id)
        {
            try
            {
                await _SalesService.SalesApprove(id);
                return RedirectToAction("SalesUpdate", new { id = id });
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesApprove), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        [Authorize(Roles = "Sales_Approvement, SuperAdmin")]
        public async Task<IActionResult> SalesDisApprove(int id)
        {
            try
            {
                await _SalesService.SalesDisApprove(id);
                return RedirectToAction("SalesUpdate", new { id = id });
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(SalesDisApprove), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }

        //===========//===========//===========//===========//===========
        public async Task<IActionResult> DeletePermenantly(int id)
        {
            var result = await _SalesService.DeletePermenantly(id);
            return Json(result);
        }

        //===========//===========//===========//===========//===========
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _SalesService.Restore(id);
            return Json(result);
        }

        #endregion

        #region Sales Transactions
        [Authorize(Roles = "Sales_Edit, SuperAdmin")]
        public async Task<IActionResult> MySales()
        {
            try
            {
                if (!AmISales())
                    return RedirectToAction("Index", "Dashboard");

                await GetComboBoxes();
                var SalesId = GetSalesId();
                var comp = await _SalesService.GetByIdAsync(SalesId);
                return View(comp);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(MySales), ex);
                return Redirect("/Error");
            }
        }


        //----------------------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMySales(CreateNewSalesDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.ModelNotValid, "ModelNotValid"));

                var resopns = await _SalesService.UpdateMySales(model);
                if (resopns == true)
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be Update!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateMySales), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region private
        //*************************************************************************************** 
        private async Task<bool> SendEmailConfirmation(ApplicationUser user)
        {
            try
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, uid = user.Id }, Request.Scheme);
                await _emailService.SendEmail("noreply@insaattan.com", "Insaattan", user.Email, user.UserName, confirmationLink, EmailType.ConfirmAccount);

                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SendEmailConfirmation), ex, Importance.VeryHigh);
                return false;
            }
        }

        //*************************************************************************************** IsEmailExist
        private bool IsEmailExist(string Email)
        {
            try
            {
                return _sysdb.Users.Any(x => x.Email == Email && x.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(IsEmailExist), ex, Importance.VeryHigh);
                return false;
            }

        }

        //*************************************************************************************** IsUserNameExist
        private bool IsUserNameExist(string userName)
        {
            try
            {
                return _sysdb.Users.Any(x => x.UserName == userName && x.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(IsUserNameExist), ex, Importance.VeryHigh);
                return false;
            }

        }
        //*************************************************************************************** 
        public async Task<IActionResult> CheckIsFirstTimeLogIn()
        {
            try
            {
                var comapnyId = int.Parse(_webWorkContext.GetMySalesId());
                if (comapnyId > 0)
                {
                    var model = await _SalesService.CheckIfFirstTime(comapnyId);
                    if (model)
                        return Json(new ResponseResult(ResponseType.Success));
                    else
                        return Json(new ResponseResult(ResponseType.Error));

                }
                else
                    return Json(new ResponseResult(ResponseType.Error));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(CheckIsFirstTimeLogIn), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }
        //*************************************************************************************** 
        public async Task<IActionResult> AcceptTearmAndConditions()
        {
            try
            {
                var comapnyId = int.Parse(_webWorkContext.GetMySalesId());
                if (comapnyId > 0)
                {
                    var model = await _SalesService.AcceptTearmAndConditions(comapnyId);
                    if (model)
                        return Json(new ResponseResult(ResponseType.Success));
                    else
                        return Json(new ResponseResult(ResponseType.Error));

                }
                else
                    return Json(new ResponseResult(ResponseType.Error));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("SalesController" + nameof(AcceptTearmAndConditions), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }
        //*************************************************************************************** 

        #endregion
    }
}
