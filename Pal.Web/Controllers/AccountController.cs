using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pal.Core.Domains.Account;
using Pal.Core.Domains.Customers;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Email;
using Pal.Core.Enums.Roles;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Company;
using Pal.Services.DataServices.Customers;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Email;
using Pal.Services.FileManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.PalFunctions;
using Pal.Services.Sms;
using Pal.Web.Extensions;
using Pal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Pal.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApplicationDbContext _sysdb;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IFileManagerService _fileManager;
        private readonly ILookupsService _lookupsService;
        private readonly ICustomersService _customersService;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        private readonly ILoggerService _loggerService;

        public AccountController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IFileManagerService fileManagerService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILookupsService lookupsService,
            ICustomersService customersService,
            ISmsService smsService,
            ILoggerService loggerService, ApplicationDbContext context,
            IEmailService emailService) : base(languageService, localizationService)
        {
            _sysdb = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _fileManager = fileManagerService;
            _lookupsService = lookupsService;
            _customersService = customersService;
            _smsService = smsService;
            _loggerService = loggerService;
            _emailService = emailService;
            _context = context;
        }
        private async Task GetComboBoxes()
        {
            ViewBag.CompanyCateogries = (await _lookupsService.GetSysCompanyCategories()).ToSelectList();
            ViewBag.SalesCateogries = (await _lookupsService.GetSyssSalesCategories()).ToSelectList();
        }

        //------------------------------------------------------------
        private async Task GetLookups()
        {

            try
            {
                ViewBag.cbCountry = await _lookupsService.GetSysCountries();
                ViewBag.cbCity = await _lookupsService.GetSysCity();

                ViewBag.cbNationality = await _lookupsService.GetSysNationalities();
                await Task.Run(() =>
                 {
                     ViewData["WeekDays"] = PalFunctions.GetWeekDaysByCulture(Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name)
                                              .Select(a => new { Id = a.Id, Name = a.DayName });
                     ViewData["WorkHours"] = _lookupsService.GetSysWorkHours().ToSelectList();
                 });
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(GetLookups), ex);
            }


        }


        #region Registeration
        //------------------------------------------------------------
        public async Task<IActionResult> CustomerRegister()
        {
            try
            {
                var model = new CustomerRegisterViewModel
                {
                    //Address = "Address",
                    CountryId = 1,
                    //PhoneNumber = "+905374611470",
                    //Email = "abdullah.666doghan@outlook.com"/*"Customer" + DateTime.Now.Millisecond.ToString() + "@customer.com"*/,
                    //FullName = "FullName",
                    //NationalityId = 1,
                    //UserName = "customer" + DateTime.Now.Millisecond.ToString(),
                    //WhatsappNumber = "4456546",
                };

                await GetLookups();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(CustomerRegister), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerRegister(CustomerRegisterViewModel model)
        {
            try
            {
                if (model == null)
                    return View();

                //if (!ModelState.IsValid)
                //{
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.ModelNotValid, Localize("account.resgister.NotValid").ToString()));
                //}


                //if (IsEmailExistForServiceside(model.Email))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.emailexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.emailexist").ToString()));
                //}

                //if (IsUserNameExistForServiceside(model.UserName))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.usernamexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.usernamexist").ToString()));
                //}


                ApplicationUser user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    WhatsappNumber = model.WhatsappNumber,
                    UserType = UserType.Customers,
                    RegisterdByAdmin = false,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    user.ReferenceId = (await _customersService.AddAsync(new Data.DTOs.Customer.CustomerDTO
                    {
                        CountryId = model.CountryId,
                        CustomerStatus = Core.Enums.Customer.CustomerStatus.New,
                        FullName = model.FullName,
                        NationalityId = model.NationalityId,
                        PhoneNumber = model.PhoneNumber,
                        WhatsappNumber = model.WhatsappNumber,
                        UserId = user.Id,
                        Email = user.Email,
                        IsLead = true,
                        FullAddress = model.Address,
                    })).ToString();

                    var claims = new List<Claim>
                {
                    new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
                    new Claim(type: PalClaimType.CustomerId.ToString(), value: user.ReferenceId.ToString()),
                   new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber.ToString())
                };
                    await _userManager.AddClaimsAsync(user, claims);
                    await SendEmailConfirmation(user);


                    TempData["email"] = user.Email;
                    TempData["userFullName"] = user.FullName;
                    //return RedirectToAction(nameof(WelcomePage));
                    //var signinResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                        return Json(new ResponseResult(ResponseType.Success, user.Id.ToString()));
                    //return RedirectToAction(nameof(PhoneNumberVerification));

                    else
                        return Json(new ResponseResult(ResponseType.Error));

                }
                else //if there are errors in register
                {
                    var errors = new List<object>();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", Localize($"account.{error.Code}").Value);
                        errors.Add((Localize($"account.{error.Code}").Value));
                    }
                    await GetLookups();
                    return Json(new ResponseResult(ResponseType.Error, errors[0].ToString()));
                }
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(CustomerRegister), ex);
                return NotFound();
            }



        }

        //------------------------------------------------------------
        public async Task<IActionResult> RegisterAdvisor()
        {

            try
            {
                var model = new AdvisorRegisterViewModel
                {
                    CountryId = 1,
                    //UserPhoneNumber = "153216546",
                    //Email = "Advisor" + DateTime.Now.Millisecond.ToString() + "@Advisor.com",
                    //FullName = "FullName",
                    //UserName = "Advisor" + DateTime.Now.Millisecond.ToString(),
                    //WhatsappNumber = "4456546",
                    //WorkStart = 0,
                    //WorkEnd = 3,

                };
                await GetLookups();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(RegisterAdvisor), ex);
                return NotFound();
            }


        }

        //------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdvisor(AdvisorRegisterViewModel model)
        {
            try
            {
                if (model == null)
                    return View();

                //if (!ModelState.IsValid)
                //{
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.ModelNotValid, Localize("account.resgister.NotValid").ToString()));
                //}


                //if (IsEmailExistForServiceside(model.Email))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.emailexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.emailexist").ToString()));
                //}

                //if (IsUserNameExistForServiceside(model.UserName))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.usernamexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.usernamexist").ToString()));
                //}


                ApplicationUser user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FullName = model.FullName,
                    PhoneNumber = model.UserPhoneNumber,
                    WhatsappNumber = model.WhatsappNumber,
                    UserType = UserType.Adviser,
                    RegisterdByAdmin = false,
                };


                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //user.ReferenceId = (await _advisorService.AddAsync(new Data.DTOs.Advisors.AdvisorCreateDTO
                    //{
                    //    CountryId = model.CountryId,
                    //    CityId = model.CityId,
                    //    SpeakingLanguages = model.SpeakingLanguages,
                    //    WhatsApp = model.WhatsappNumber,
                    //    Phone = model.UserPhoneNumber,
                    //    Email = model.Email,
                    //    MainImgFile = model.MainImg,
                    //    WorkStart = model.WorkStart,
                    //    WorkEnd = model.WorkEnd,
                    //    Days = model.Days,
                    //    CertificateFileObject = model.CertificateFile,
                    //    FullAddress = model.FullAddress,
                    //    Translates = new List<Data.DTOs.Advisors.AdvisorTranslateDTO>
                    //{
                    //    new Data.DTOs.Advisors.AdvisorTranslateDTO{ LanguageId = 1, FullName = model.FullName },
                    //    new Data.DTOs.Advisors.AdvisorTranslateDTO{ LanguageId = 2, FullName = model.FullName },
                    //},

                    //})).ToString();

                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
                        new Claim(type: PalClaimType.AdvisorId.ToString(), value: user.ReferenceId.ToString()),
                        new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber.ToString())
                    };
                    await _userManager.AddClaimsAsync(user, claims);

                    #region Roles
                    #region Project
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_List.ToString());
                    #endregion
                    #region RealEstate
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_List.ToString());
                    #endregion
                    #region Chat
                    //await _usermanager.addtoroleasync(user, palroles.chat.tostring());
                    #endregion
                    #region MyTask
                    await _userManager.AddToRoleAsync(user, PalRoles.Task_MyTasks_Scheduler.ToString());
                    await _userManager.AddToRoleAsync(user, PalRoles.Task_Details.ToString());
                    #endregion



                    #endregion
                    await SendEmailConfirmation(user);
                    TempData["email"] = user.Email;
                    TempData["userFullName"] = user.FullName;

                    return Json(new ResponseResult(ResponseType.Success, user.Id.ToString()));

                }
                else //if there are errors in register
                {
                    var errors = new List<object>();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", Localize($"account.{error.Code}").Value);
                        errors.Add((Localize($"account.{error.Code}").Value));
                    }
                    await GetLookups();
                    return Json(new ResponseResult(ResponseType.Error, errors[0].ToString()));
                }
            }
            catch (Exception ex)
            {
               
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(CustomerRegister), ex);
                return NotFound();
            }


        }

        //------------------------------------------------------------
        public async Task<IActionResult> RegisterCompany()
        {
            try
            {
                await GetLookups();
                await GetComboBoxes();
                var model = new CompanyRegisterViewModel
                {
                    CountryId = 1,
                    CityIdd = 1,
               
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(RegisterCompany), ex);
                return NotFound();
            }

        } 
        public async Task<IActionResult> RegisterSales()
        {
            try
            {
                await GetLookups();
                await GetComboBoxes();
                var model = new SalesRegisterViewModel
                {
                    CountryId = 1,
                    CityIdd = 1,
               
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(RegisterSales), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RegisterCompany(CompanyRegisterViewModel model)
        {
            try
            {
                if (model == null)
                    return View();

                //if (!ModelState.IsValid)
                //{
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.ModelNotValid, Localize("account.resgister.NotValid").ToString()));
                //}

                //if (IsEmailExistForServiceside(model.Email))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.emailexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.emailexist").ToString()));
                //}

                //if (IsUserNameExistForServiceside(model.UserName))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.usernamexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.usernamexist").ToString()));
                //}

                ApplicationUser user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FullName = model.FullName,
                    PhoneNumber = model.UserPhoneNumber,
                    WhatsappNumber = model.WhatsappNumber,
                    UserType = UserType.Companies,
                    RegisterdByAdmin = false,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //user.ReferenceId = (await _companyService.CreateNewCompany(new CreateNewCompanyDTO
                    //{
                    //    CityId = model.CityIdd,
                    //    CountryId = model.CountryId,
                    //    Email = model.ContactEmail,
                    //    ManagedBy = user.Id,
                    //    Phone = model.CompanyPhoneNumber,
                    //    Website = model.Website,
                    //    MainImageFile = model.Logo,
                    //    WhatsApp = user.PhoneNumber,
                    //    CompanyTranslates = new List<CreateNewCompanyTranslateDTO>
                    //{
                    //    new CreateNewCompanyTranslateDTO{CompanyName = model.CompanyName,LanguageId = 1,},
                    //    new CreateNewCompanyTranslateDTO{CompanyName = model.CompanyName,LanguageId = 2,}
                    //},
                    //    CompanyLogoFile = model.Logo,
                    //    FullAddress = model.CompanyFullAddress,
                    //    ManagerEmail = model.ManagerEmail,
                    //    ManagerName = model.ManagerName,
                    //    TaxNumber = model.TaxNumber,
                    //    ManagerSignatureImageFile = model.ManagerSignatureImageFile,
                    //    TaxDocumentImageFile = model.TaxDocumentImageFile,
                    //    AttachmentsFiles = model.Attachments,
                    //})).ToString();

                    await _userManager.UpdateAsync(user);

                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
                        new Claim(type: PalClaimType.CompanyId.ToString(), value: user.ReferenceId.ToString()),
                            new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber.ToString())
                    };
                    await _userManager.AddClaimsAsync(user, claims);

                    #region Roles
                    #region Project
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_List.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Add.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Edit.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Remove.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_Repeat.ToString());
                    #endregion
                    #region RealEstate
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_List.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Add.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Edit.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Remove.ToString());
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_Repeat.ToString());
                    #endregion
                    #region Chat
                    //await _userManager.AddToRoleAsync(user, PalRoles.Chat.ToString());
                    #endregion
                    #region MyRegion
                    //await _userManager.AddToRoleAsync(user, PalRoles.Companies_Edit.ToString());
                    #endregion

                    #endregion
                    //await SendSmsVerification(user.PhoneNumber);
                    await SendEmailConfirmation(user);

                    TempData["email"] = user.Email;
                    TempData["userFullName"] = user.FullName;

                    return Json(new ResponseResult(ResponseType.Success, user.Id.ToString()));
                    //return RedirectToAction(nameof(PhoneNumberVerification));

                    //var signinResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    //if (signinResult.Succeeded)
                    //    return RedirectToAction(nameof(PhoneNumberVerification));

                    //else
                    //    //TODO there is a big fuckin error in my system!!!!!!
                    //    // this code here should never run
                    //    return RedirectToAction(nameof(Login));

                }
                else //if there are errors in signin
                {
                    var errors = new List<object>();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", Localize($"account.{error.Code}").Value);
                        errors.Add((Localize($"account.{error.Code}").Value));
                    }
                    await GetLookups();
                    return Json(new ResponseResult(ResponseType.Error, errors[0].ToString()));
                }
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(RegisterCompany), ex);
                return NotFound();
            }

        }


        //------------------------------------------------------------
        /// <summary>
        /// This page opens after registering
        /// </summary>
        public IActionResult WelcomePage()
        {
            return View();
        }


        //*************************************************************************************** 
        [HttpGet]
        public async Task<IActionResult> PhoneNumberVerification(string uid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(uid);
                if (user == null)
                    return Redirect("/");

                if (await _userManager.IsPhoneNumberConfirmedAsync(user))
                    return Redirect("/");


                var now = DateTime.Now;
                var checkCode = await _sysdb.PhoneVerificationTokens.AnyAsync(a => a.PhoneNumber == user.PhoneNumber &&
                                     EF.Functions.DateDiffMinute(a.SendDate, now) < 10);
                if (!checkCode)
                   SendSmsVerification(user.PhoneNumber, PhoneVerificationTokenType.Register);


                ViewBag.userPhoneNumber = user.PhoneNumber;
                ViewBag.userId = user.Id;

                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(PhoneNumberVerification), ex, Importance.VeryHigh);
                return BadRequest();
            }

        }



        //*************************************************************************************** 
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendPhoneVerificationSms(string userId)
        {
            try
            {
                var user = await _sysdb.Users.FirstOrDefaultAsync(a => a.Id == userId);
                if (await _userManager.IsPhoneNumberConfirmedAsync(user))
                    return Json(new ResponseResult(ResponseType.Success));


                if (SendSmsVerification(user.PhoneNumber, PhoneVerificationTokenType.Login))
                    return Json(new ResponseResult(ResponseType.Success));


                return Json(new ResponseResult(ResponseType.Error, "WrongNumber"));
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ResendPhoneVerificationSms), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //*************************************************************************************** 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumber">use this format!!!! +905344602870</param>
        /// <returns></returns>
        private bool SendSmsVerification(string phoneNumber, PhoneVerificationTokenType phoneVerificationTokenType)
        {
            try
            {
                return true;
                //#region Test Debug
                //if (phoneNumber == "+905374611470")
                //{
                //    await _sysdb.PhoneVerificationTokens.AddAsync(new AspNetPhoneVerificationToken
                //    {
                //        PhoneNumber = phoneNumber,
                //        Code = "123456",
                //        VerificationTokenType = phoneVerificationTokenType,
                //        SendDate = DateTime.Now,
                //    });
                //    await _sysdb.SaveChangesAsync();

                //    return true;
                //}
                //#endregion


                //var code = PalFunctions.GenerateRandom(6, true);
                //var result = await _smsService.SendSms(templateId: 1, phoneNumber, langId: 1, parameters: code);
                //if (result.ResponseType == ResponseType.Success)
                //{
                //    await _sysdb.PhoneVerificationTokens.AddAsync(new AspNetPhoneVerificationToken
                //    {
                //        PhoneNumber = phoneNumber,
                //        Code = code,
                //        SendDate = DateTime.Now,
                //        VerificationTokenType = phoneVerificationTokenType,
                //    });
                //    await _sysdb.SaveChangesAsync();

                //}

                //return result.ResponseType == ResponseType.Success;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(SendSmsVerification), ex, Importance.VeryHigh);
                return false;
            }
        }

        //*************************************************************************************** 
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckRegisterSmsVerification(string userId, string code)
        {
            try
            {
                var user = await _sysdb.Users.FirstOrDefaultAsync(a => a.Id == userId);
                var now = DateTime.Now;
                var checkCode = await _sysdb.PhoneVerificationTokens.AnyAsync(a => a.PhoneNumber == user.PhoneNumber &&
                                    a.Code == code &&
                                    a.VerificationTokenType == PhoneVerificationTokenType.Register &&
                                     EF.Functions.DateDiffMinute(a.SendDate, now) < 10);

                if (checkCode)
                {
                    user.PhoneNumberConfirmed = true;
                    _sysdb.Users.Update(user);
                    await _sysdb.SaveChangesAsync();
                    return Json(new ResponseResult(ResponseType.Success));
                }
                else
                    return Json(new ResponseResult(ResponseType.Error, "WrongCode"));

            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(CheckRegisterSmsVerification), ex);
                return Json(new ResponseResult(ResponseType.Error, ex));
            }
        }

        //*************************************************************************************** 
        private async Task<bool> SendEmailConfirmation(ApplicationUser user)
        {
            try
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, uid = user.Id }, Request.Scheme);
                await _emailService.SendEmail("noreply@insaattan.com", "Insaattan", user.Email, user.UserName, confirmationLink, EmailType.ConfirmAccount);

                return true;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(SendEmailConfirmation), ex, Importance.VeryHigh);
                return false;
            }
        }
        //*************************************************************************************** ConfirmEmail
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
                    return NotFound();


                var user = await _userManager.FindByIdAsync(uid);
                if (user == null)
                    return NotFound();

                var res = await _userManager.ConfirmEmailAsync(user, token);
                if (res.Succeeded)
                {
                    return View();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ConfirmEmail), ex, Importance.VeryHigh);
                return NotFound();
            }

        }

        //*************************************************************************************** IsEmailExist




        //*************************************************************************************** 
        [HttpPost]
        public async Task<IActionResult> ReSendEmailConfirmation(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return Json(new ResponseResult(ResponseType.Error, "EmailNotFound"));


                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return Json(new ResponseResult(ResponseType.Error, "UserNotFound"));


                if (await SendEmailConfirmation(user))
                    return Json(new ResponseResult(ResponseType.Success));
                else
                    return Json(new ResponseResult(ResponseType.Error, "CouldNotSendEmail"));
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ReSendEmailConfirmation), ex, Importance.VeryHigh);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }

        //------------------------------------------------------------
        public async Task<IActionResult> RegisterLawyer()
        {

            try
            {
                var model = new LawyerRegisterViewModel
                {
                    CountryId = 1,
                    //UserPhoneNumber = "153216546",
                    //Email = "Advisor" + DateTime.Now.Millisecond.ToString() + "@Advisor.com",
                    //FullName = "FullName",
                    //UserName = "Advisor" + DateTime.Now.Millisecond.ToString(),
                    //WhatsappNumber = "4456546",
                    //WorkStart = 0,
                    //WorkEnd = 3,

                };
                await GetLookups();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(RegisterLawyer), ex);
                return NotFound();
            }


        }

        //------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterLawyer(LawyerRegisterViewModel model)
        {
            try
            {
                if (model == null)
                    return View();

                //if (!ModelState.IsValid)
                //{
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.ModelNotValid, Localize("account.resgister.NotValid").ToString()));
                //}


                //if (IsEmailExistForServiceside(model.Email))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.emailexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.emailexist").ToString()));
                //}

                //if (IsUserNameExistForServiceside(model.UserName))
                //{
                //    ModelState.AddModelError(string.Empty, Localize("account.resgister.usernamexist").ToString());
                //    await GetLookups();
                //    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.usernamexist").ToString()));
                //}


                ApplicationUser user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FullName = model.FullName,
                    PhoneNumber = model.UserPhoneNumber,
                    WhatsappNumber = model.WhatsappNumber,
                    UserType = UserType.Lawyer,
                    RegisterdByAdmin = false,
                };


                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //user.ReferenceId = (await _lawyerService.AddAsync(new Data.DTOs.Lawyer.LawyerCreateDTO
                    //{
                    //    CountryId = model.CountryId,
                    //    CityId = model.CityId,
                    //    SpeakingLanguages = model.SpeakingLanguages,
                    //    WhatsApp = model.WhatsappNumber,
                    //    Phone = model.UserPhoneNumber,
                    //    Email = model.Email,
                    //    MainImgFile = model.MainImg,
                    //    WorkStart = model.WorkStart,
                    //    WorkEnd = model.WorkEnd,
                    //    Days = model.Days,
                    //    CertificateFileObject = model.CertificateFile,
                    //    FullAddress = model.FullAddress,
                    //    Translates = new List<Data.DTOs.Lawyer.LawyerTranslateDTO>
                    //{
                    //    new Data.DTOs.Lawyer.LawyerTranslateDTO{ LanguageId = 1, FullName = model.FullName },
                    //    new Data.DTOs.Lawyer.LawyerTranslateDTO{ LanguageId = 2, FullName = model.FullName },
                    //},

                    //})).ToString();

                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
                        new Claim(type: PalClaimType.LawyerId.ToString(), value: user.ReferenceId.ToString()),
                        new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber.ToString())
                    };
                    await _userManager.AddClaimsAsync(user, claims);

                    #region Roles
                    #region Project
                    //await _userManager.AddToRoleAsync(user, PalRoles.Project_List.ToString());
                    #endregion
                    #region RealEstate
                    //await _userManager.AddToRoleAsync(user, PalRoles.RealEstate_List.ToString());
                    #endregion
                    #region Chat
                    //await _usermanager.addtoroleasync(user, palroles.chat.tostring());
                    #endregion



                    #endregion
                    await SendEmailConfirmation(user);
                    TempData["email"] = user.Email;
                    TempData["userFullName"] = user.FullName;

                    return Json(new ResponseResult(ResponseType.Success, user.Id.ToString()));

                }
                else //if there are errors in register
                {
                    var errors = new List<object>();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", Localize($"account.{error.Code}").Value);
                        errors.Add((Localize($"account.{error.Code}").Value));
                    }
                    await GetLookups();
                    return Json(new ResponseResult(ResponseType.Error, errors[0].ToString()));
                }
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(RegisterLawyer), ex);
                return NotFound();
            }


        }

        //------------------------------------------------------------
        #endregion

        #region Login
        //------------------------------------------------------------
        public IActionResult Login()
        {
            return View();
        }

        //------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            //PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            //ApplicationUser admin = new ApplicationUser()
            //{
            //    UserName = model.UserNameOrEmail
            //};
            //admin.PasswordHash = ph.HashPassword(admin, "123456");

            try
            {
                if (model == null)
                    return NotFound();

                if (!ModelState.IsValid)
                {
                    AddErrorsToModelState();
                    return View(model);
                }

                var user = await _userManager.FindByNameAsync(model.UserNameOrEmail);

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
                    if (user == null)
                    {
                        ModelState.AddModelError("0", Localize("account.login.error.usernotfound").ToString());
                        return View(model);
                    }
                }
                //var user = _context.Users.FirstOrDefault(a => a.UserName == model.UserNameOrEmail);
                if (user.IsDeleted)
                {
                    ModelState.AddModelError("0", Localize("account.login.error.usernotactive").ToString());
                    return View(model);
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("0", Localize("account.login.error.emailnotconfirmed").ToString());
                    return View(model);
                }

                //if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                //    return RedirectToAction(nameof(PhoneNumberVerification), new { uid = user.Id });

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {

                    user.LastLogInDate = DateTime.Now;
                    _sysdb.Users.Update(user);
                    await _sysdb.SaveChangesAsync();
                    switch (user.UserType)
                    {
                        case UserType.Admins:
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                        case UserType.Companies:
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                        case UserType.Adviser:
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                        case UserType.Lawyer:
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                        default:
                            return Redirect("/");

                    }


                }

                ModelState.AddModelError("", Localize("account.login.error.invalidcridential").ToString());
                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(Login), ex);
                return NotFound();
            }
        }

        //------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Redirect("/");
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(Logout), ex);
                return NotFound();
            }

        }

        #endregion

        #region Reset Password
        //*************************************************************************************** ResetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }

        //*************************************************************************************** ResetPassword

        public async Task<IActionResult> ForgetPasswordPost(string phoneNumber, string countryCode)
        {
            var FullPhone = "+" + countryCode.Trim() + phoneNumber;

            if (phoneNumber == null)
            {
                ModelState.AddModelError(string.Empty, Localize("publicUI.account.resetPassword.phoneNumberNotFound").ToString());
                return View();
            }
            phoneNumber = phoneNumber.Replace(" ", String.Empty);

            //var x = _sysdb.Users.Any(x => x.PhoneNumber == phoneNumber);
            //var y = _sysdb.Users.Any(x => x.PhoneNumber == "+" + countryCode.Trim() + phoneNumber);

            if (!_sysdb.Users.Any(x => x.PhoneNumber == phoneNumber) && !_sysdb.Users.Any(x => x.PhoneNumber == FullPhone))
                return Redirect("/");

            var now = DateTime.Now;
            var checkCode = await _sysdb.PhoneVerificationTokens.AnyAsync(a => a.PhoneNumber == FullPhone &&
                                 EF.Functions.DateDiffMinute(a.SendDate, now) < 10);
            if (!checkCode)
                if (SendSmsVerification(FullPhone, PhoneVerificationTokenType.ResetPassword))
                {
                    var userId = await _sysdb.Users.Where(x => x.PhoneNumber == phoneNumber || x.PhoneNumber == FullPhone).Select(z => z.Id).FirstOrDefaultAsync();
                    return RedirectToAction(nameof(ResetPassword), new { uid = userId });
                }
                else
                    return View(nameof(ForgetPassword));
            return View();
        }

        //*************************************************************************************** ResetPassword
        [HttpGet]
        public IActionResult ForgetPasswordByEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPasswordByEmail(string email)
        {
            if (email == null)
            {
                //ModelState.AddModelError(string.Empty, Localize("publicUI.account.resetPassword.phoneNumberNotFound").ToString());
                return View();
            }

            ApplicationUser user = new ApplicationUser();
            if (!_sysdb.Users.Any(x => x.Email == email)) 
            { return Redirect("/"); }
            else 
            { user = await _sysdb.Users.FirstOrDefaultAsync(x => x.Email == email); }

            string checkCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (checkCode != "")
                if (SendEmailConfirmation(user).Result)
                {
                    var userId = user.Id;
                    return RedirectToAction(nameof(ResetPassword), new { uid = userId });
                }
                else
                    return View(nameof(ForgetPassword));
            return View();
        }

        //*************************************************************************************** ResetPassword

        public async Task<IActionResult> ResetPassword(string uid)
        {
            try
            {
                if (string.IsNullOrEmpty(uid))
                    return BadRequest();
                var user = await _userManager.FindByIdAsync(uid);
                if (user == null)
                    return NotFound();

                ViewBag.UserId = user.Id;

                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ResetPassword), ex, Importance.VeryHigh);
                return BadRequest();
            }

        }


        //*************************************************************************************** ResetPassword
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)    
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, Localize("publicUI.account.resetPassword.codeNotValid").ToString());
                    return View(model);
                }


                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return BadRequest("User Not Found");

                // check the code
                var now = DateTime.Now;
                var checkCode = await _sysdb.PhoneVerificationTokens.AnyAsync(a => a.PhoneNumber == user.PhoneNumber &&
                                      a.Code == model.VerificationCode &&
                                      a.VerificationTokenType == PhoneVerificationTokenType.ResetPassword &&
                                       EF.Functions.DateDiffMinute(a.SendDate, now) < 10);

                if (checkCode)
                {
                    await _userManager.RemovePasswordAsync(user);
                    var result = await _userManager.AddPasswordAsync(user, model.NewPassword);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Login));
                }

                else
                {
                    ModelState.AddModelError(string.Empty, Localize("publicUI.account.resetPassword.codeNotValid").ToString());
                    ViewBag.UserId = model.UserId;
                    return View(model);
                }

                return NotFound();

            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ResetPassword), ex, Importance.VeryHigh);
                return BadRequest();
            }

        }


        #endregion

        #region Chang Email
        public IActionResult ChangeCustomerEmail(string id)
        {
            try
            {
                ViewBag.UserId = id;
                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(ChangeCustomerEmail), ex);
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangeCustomerEmail(string Id, string Email)
        {
            try
            {
                var user = await _sysdb.Users.FirstOrDefaultAsync(a => a.Id == Id);
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, Email);
                var result = await _userManager.ChangeEmailAsync(user, Email, token);
                user.EmailConfirmed = false;
                _sysdb.Users.Update(user);
                await _sysdb.SaveChangesAsync();
                if (result.Succeeded)
                {
                    await SendEmailConfirmation(user);
                    var customer = await _context.Customers.FirstOrDefaultAsync(a => a.UserId == Id);
                    var oldEmail = customer.Email;
                    customer.Email = Email;
                    _context.Update(customer);
                    var note = new CustomerNotes { Content = "Email has been changed from " + oldEmail + " to new email " + Email, Date = DateTime.Now, CustomerId = customer.Id, };
                    _context.CustomerNotes.Add(note);
                    await _context.SaveChangesAsync();
                    return Json(new ResponseResult(ResponseType.Success));
                }
                else
                {
                    return Json(new ResponseResult(ResponseType.Error));
                }
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ChangeCustomerEmail), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Chang Phone
        public IActionResult ChangeCustomerPhoneNumber(string id)
        {
            try
            {
                ViewBag.UserId = id;
                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(ChangeCustomerPhoneNumber), ex);
                return NotFound();
            }
        }
        //--------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SendPhoneVerificationSms(string phoneNumber)
        {
            try
            {
                var now = DateTime.Now;
                var checkCode = await _sysdb.PhoneVerificationTokens.AnyAsync(a => a.PhoneNumber == phoneNumber &&
                          
                            a.VerificationTokenType == PhoneVerificationTokenType.changePhoneNumber &&
                             EF.Functions.DateDiffMinute(a.SendDate, now) < 10);
                if (checkCode)
                    return Json(new ResponseResult(ResponseType.Success));
                //return Json(new ResponseResult(ResponseType.Error, Localize("publicUI.account.resetPhone.codeSendBrfor").ToString()));


                if (SendSmsVerification(phoneNumber, PhoneVerificationTokenType.changePhoneNumber))
                    return Json(new ResponseResult(ResponseType.Success));

                return Json(new ResponseResult(ResponseType.Error, "WrongNumber"));
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(SendPhoneVerificationSms), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //--------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ChangeCustomerPhoneNumber(ResetPhoneNumberVM model)
        {
            try
            {
                var user = await _sysdb.Users.FirstOrDefaultAsync(a => a.Id == model.Id);

                var now = DateTime.Now;
                var checkCode = await _sysdb.PhoneVerificationTokens.AnyAsync(a => a.PhoneNumber == model.phoneNumber &&
                                      a.Code == model.VerificationCode &&
                                      a.VerificationTokenType == PhoneVerificationTokenType.changePhoneNumber &&
                                       EF.Functions.DateDiffMinute(a.SendDate, now) < 10);

                if (!checkCode)
                    return Json(new ResponseResult(ResponseType.Error, Localize("publicInterface.resetPhone.codeNotMatch").ToString()));

                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.phoneNumber);
                var result = await _userManager.ChangePhoneNumberAsync(user, model.phoneNumber, token);
                _sysdb.Users.Update(user);
                await _sysdb.SaveChangesAsync();
                if (result.Succeeded)
                {

                    var customer = await _context.Customers.FirstOrDefaultAsync(a => a.UserId == model.Id);
                    var oldPhoneNumber = customer.PhoneNumber;
                    customer.PhoneNumber = model.phoneNumber;
                    _context.Update(customer);
                    var note = new CustomerNotes { Content = "Phone number has been changed from " + oldPhoneNumber + " to new number "+ model.phoneNumber, Date = DateTime.Now, CustomerId = customer.Id, };
                    _context.CustomerNotes.Add(note);
                    await _context.SaveChangesAsync();
                    return Json(new ResponseResult(ResponseType.Success));
                }
                else
                {

                    return Json(new ResponseResult(ResponseType.Error));
                }
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ChangeCustomerPhoneNumber), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Chang Password
        public IActionResult ChangeCustomerPassword(string id)
        {
            try
            {
                ViewBag.UserId = id;
                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(ChangeCustomerPassword), ex);
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangeCustomerPassword(ChangePasswordVM model)
        {
            try
            {

                var user = await _sysdb.Users.FirstOrDefaultAsync(a => a.Id == model.Id);
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Json(new ResponseResult(ResponseType.Success));
                }
                else
                {
                    return Json(new ResponseResult(ResponseType.Error, Localize(result.Errors.First().Code.ToString()).ToString()));
                }
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(ChangeCustomerPassword), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        #endregion

        #region Validation
        [HttpPost]
        public IActionResult IsEmailExist(string Email)
        {
            try
            {
                var IsExist = _sysdb.Users.Any(x => x.Email == Email && x.IsDeleted == false);
                if (!IsExist)
                    return Json(new ResponseResult(ResponseType.Error));
                else
                    return Json(new ResponseResult(ResponseType.Success));


            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(IsEmailExist), ex, Importance.VeryHigh);
                return null;
            }

        }

        //*************************************************************************************** IsUserNameExist
        [HttpPost]
        public IActionResult IsUserNameExist(string userName)
        {
            try
            {
                var IsExist = _sysdb.Users.Any(x => x.UserName == userName && x.IsDeleted == false);
                if (!IsExist)
                    return Json(new ResponseResult(ResponseType.Error));
                else
                    return Json(new ResponseResult(ResponseType.Success));


            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(IsUserNameExist), ex, Importance.VeryHigh);
                return null;
            }

        }

        #endregion

    }
}
