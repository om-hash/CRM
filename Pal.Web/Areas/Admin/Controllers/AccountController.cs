using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Pal.Core.Domains.Account;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Email;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Account;
using Pal.Data.DTOs.Employees;
using Pal.Services;
using Pal.Services.DataServices.Employees;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Email;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Web.Controllers;
using Pal.Web.Extensions;

using System.Security.Claims;

namespace Pal.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Area("Admin")]
    public class AccountController : BaseController
    {

        private readonly ILoggerService _loggerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _sysdb;
        private readonly IEmployeesService _employeeService;
        private readonly ILookupsService _lookupsService;
        private readonly IEmailService _emailService;
        public AccountController(ILoggerService loggerService, ApplicationDbContext db,
              UserManager<ApplicationUser> userManager, IEmployeesService employeesService, ILanguageService languageService,
          ILocalizationService localizationService, ILookupsService lookupsService,
            IEmailService emailService) : base(languageService, localizationService)
        {
            _loggerService = loggerService;
            _sysdb = db;
            _userManager = userManager;
            _employeeService = employeesService;
            _lookupsService = lookupsService;
            _emailService = emailService;
        }
        //-------------------------------------------------------------------
        #region UserService
        private async Task GetComboBoxes()
        {
            var jobTitles = (await _lookupsService.GetSysJobTitles());
            ViewBag.cbJobTitles = new SelectList(jobTitles, "Id", "Name");
        }

        [Authorize(Roles = "SuperAdmin, Employee_List")]
        public IActionResult UsersList()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(UsersList), ex);
                return NotFound();
            }
        }

        public async Task<IActionResult> UsersPaginatedList([FromBody] MyDataManagerRequest dm)
        {
            try
            {
                var query = _sysdb.Users.AsQueryable().AsNoTracking()
                    .Where(x => x.UserType == UserType.Admins)
                    .Where(x => x.IsDeleted == dm.ShowDeleted)
                    .Select(x => new UsersDTOList
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        UserType = x.UserType,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        UserName = x.UserName,
                        WhatsappNumber = x.WhatsappNumber,
                        IsDeleted = x.IsDeleted,
                    });


                var data = await SyncGridOperations<UsersDTOList>.PagingAndFilterAsync(query, dm);


                return dm.RequiresCounts ? Json(new { result = data.Data, count = data.TotalCount }) : Json(data);

            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(UsersList), ex);
                return NotFound();
            }
        }

        //------------------------------------------------------------.
        [Authorize(Roles = "SuperAdmin, Employee_Add")]
        public async Task<IActionResult> UserAdd()
        {
            try
            {
                await GetComboBoxes();
                return View();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(UserAdd), ex);
                return NotFound();
            }
        }
        //------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Employee_Add")]
        public async Task<IActionResult> UserAdd(UserDTO model)
        {
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                WhatsappNumber = model.WhatsappNumber,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Admins,
                Email = model.Email,
            };
            if (model.IsEmailAutoActivation)
                user.EmailConfirmed = true;
            if (model.IsPhoneNumberAutoActivation)
                user.PhoneNumberConfirmed = true;

            try
            {
                if (IsUserNameExist(model.UserName))
                {
                    ModelState.AddModelError(string.Empty, Localize("account.resgister.usernamexist").ToString());
                    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.usernamexist").ToString()));
                }

                if (IsEmailExist(model.Email))
                {
                    ModelState.AddModelError(string.Empty, Localize("account.resgister.emailexist").ToString());
                    return Json(new ResponseResult(ResponseType.Error, Localize("account.resgister.emailexist").ToString()));
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.Roles != null)
                    {
                        var x = model.Roles.Select(x => x.RoleCode).ToList();
                        // ------------------------------------------------------------------------------ xxxx;
                        await _userManager.AddToRolesAsync(user, model.Roles.Select(x => x.RoleCode).ToList());
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
                        new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber.ToString())
                    };
                    await _userManager.AddClaimsAsync(user, claims);

                    if (!model.IsEmailAutoActivation)
                        await SendEmailConfirmation(user);

                    if (model.IsAddEmployee)
                    {
                        var empResult = await _employeeService.AddEmployeeAsync(new EmployeeDTO
                        {
                            UserId = user.Id,
                            FullName = model.FullName,
                            JobTitleId = model.JobTitleId,
                            UserType = UserType.Admins,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,

                        });
                        if (empResult == 0)
                            throw new Exception("Can not save user as employee!");
                        //return Json(new ResponseResult(ResponseType.Error, "Saved as user but not saved as employee!"));
                        user.ReferenceId = empResult.ToString();
                        result = await _userManager.UpdateAsync(user);

                    }
                    if (result.Succeeded)
                        return Json(new ResponseResult(ResponseType.Success, user.Id.ToString()));

                    else
                        throw new Exception("Can not update user to add employeeId as referenceId!");
                }

                else // if there are errors in register
                {
                    var errors = new List<object>();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", Localize($"account.{error.Code}").Value);
                        errors.Add((Localize($"account.{error.Code}").Value));
                    }
                    return Json(new ResponseResult(ResponseType.Error, errors[0].ToString()));
                }

            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                if (user.ReferenceId != null)
                    await _employeeService.DeleteEmployeeAsync(int.Parse(user.ReferenceId));
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(UserAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //------------------------------------------------------------
        [Authorize(Roles = "SuperAdmin, Employee_Edit")]
        public async Task<IActionResult> UserUpdate(string id)
        {
            try
            {
                await GetComboBoxes();
                var user = await _sysdb.Users.Where(x => x.Id == id).Select(x => new UserDTO
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    UserType = x.UserType,
                    WhatsappNumber = x.WhatsappNumber,
                    Password = "",
                }).FirstOrDefaultAsync();

                user.JobTitleId = _sysdb.Employees.FirstOrDefault(j => j.UserId == user.Id).JobTitleId;

                var roles = new List<RoleDTO>();
                var rolesIds = await _sysdb.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToListAsync();
                if (rolesIds != null)
                {
                    foreach (var roleId in rolesIds)
                    {
                        roles.Add(new RoleDTO { RoleCode = _sysdb.Roles.Where(x => x.Id == roleId).Select(a => a.Name).FirstOrDefault() });
                    }
                }
                user.Roles = roles;

                return View(user);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(UserUpdate), ex);
                return NotFound();
            }
        }
        //------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Employee_Edit")]
        public async Task<IActionResult> UserUpdate(UserDTO model)
        {
            try
            {
                var user = await _sysdb.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                user.UserName = model.UserName;
                user.WhatsappNumber = model.WhatsappNumber;
                user.PhoneNumber = model.PhoneNumber;
                user.UserType = UserType.Admins;
                user.Email = model.Email;
                user.FullName = model.FullName;
                if (model.NewPassword != null)
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.NewPassword);
                }
                _sysdb.Users.Update(user);

                var emp = _sysdb.Employees.FirstOrDefault(j => j.UserId == user.Id);
                emp.JobTitleId = model.JobTitleId;
                _sysdb.Employees.Update(emp);

                //update Rolse
                var oldRols = await _sysdb.UserRoles.Where(x => x.UserId == model.Id).ToListAsync();
                _sysdb.UserRoles.RemoveRange(oldRols);
                await _sysdb.SaveChangesAsync();
                await _userManager.AddToRolesAsync(user, model.Roles?.Select(x => x.RoleCode).ToList());
                //end of update Rolse

                await _sysdb.SaveChangesAsync();
                return Json(new ResponseResult(ResponseType.Success));
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(UserUpdate), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Employee_Remove")]
        public async Task<IActionResult> UserDelete(string id)
        {
            try
            {
                var user = await _sysdb.Users.FirstOrDefaultAsync(x => x.Id == id);
                //_sysdb.Users.Remove(user);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Can not be deleted"));


            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("AccountController" + nameof(UserDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //------------------------------------------------------------
        private bool IsEmailExist(string Email)
        {
            try
            {
                return _sysdb.Users.Any(x => x.Email == Email && x.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(IsEmailExist), ex, Importance.VeryHigh);
                return false;
            }

        }
        //-----------------------------------------------------
        private bool IsUserNameExist(string userName)
        {
            try
            {
                return _sysdb.Users.Any(x => x.UserName == userName && x.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(IsUserNameExist), ex, Importance.VeryHigh);
                return false;
            }

        }
        //----------------------------------------------------- 
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
                _ = _loggerService.LogErrorAsync(nameof(SendEmailConfirmation), ex, Importance.VeryHigh);
                return false;
            }
        }


        #endregion
    }
}
