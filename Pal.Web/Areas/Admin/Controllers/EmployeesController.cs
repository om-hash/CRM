using FastReport.Utils.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Data.DTOs.Employees;
using Pal.Services.DataServices.Employees;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Logger;
using Pal.Services.PalFunctions;

using Pal.Web.Extensions;
using Syncfusion.EJ2.Base;
using System;
using System.Dynamic;
using System.Threading.Tasks;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _employeesService;
        private readonly ILoggerService _logger;
        private readonly ILookupsService _lookupsService;
       // private readonly SqlCmd _sqlCommand;

        public EmployeesController(IEmployeesService employeesService, ILoggerService loggerService, ILookupsService lookupsService)
        {
            _employeesService = employeesService;
            _logger = loggerService;
            _lookupsService = lookupsService;
            //_sqlCommand = sqlCommand;
        }

        //------------------------------------------------------------------------
        private async Task GetComboBoxes()
        {
            var jobTitles = (await _lookupsService.GetSysJobTitles());
            //ViewBag.editableColumns = await _sqlCommand.GetEditableColumnsWithValsAsync("Employee");
            ViewBag.cbJobTitles = new SelectList(jobTitles, "Id", "Name");
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Employee_List, SuperAdmin")]
        public IActionResult EmployeesList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeesList), ex);
                return NotFound();
            }


        }
        
        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Employee_List, SuperAdmin")]
        public async Task<IActionResult> EmployeesPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var model = await _employeesService.GetAllAsListAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeesList), ex);
                return NotFound();
            }


        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Employee_Add, SuperAdmin")]
        public async Task<IActionResult> EmployeeAdd()
        {
            try
            {
                await GetComboBoxes();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeeAdd), ex);
                return NotFound();
            }

        }

        //--------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee_Add, SuperAdmin")]
        public async Task<IActionResult> EmployeeAdd(EmployeeDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));
                var result = await _employeesService.AddEmployeeAsync(model);
                if (result > 0)
                {
                    JObject jobj = JObject.Parse(model.GCols);
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));
                }
                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeeAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Employee_List, SuperAdmin")]
        public async Task<IActionResult> EmployeeUpdate(int id)
        {
            try
            {
                await GetComboBoxes();
                var employee = await _employeesService.GetEmployeeByIdAsync(id);
                //var props = typeof(EmployeeDTO).GetProperties();
                //dynamic dynObject = new ExpandoObject();
                //foreach (var prop in props)
                //{
                //    ((IDictionary<String, Object>)dynObject)[prop.Name] = prop.GetValue(employee);
                //}
                //var extraCols = _sqlCommand.GetEditableColumns("Employee");
                //foreach (var item in extraCols)
                //{
                //    ((IDictionary<String, Object>)dynObject)[item] = "jjj";
                //}

                return View(employee);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeeUpdate), ex);
                return NotFound();
            }

        }

        //--------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee_Edit, SuperAdmin")]
        public async Task<IActionResult> EmployeeUpdate(EmployeeDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                var result = await _employeesService.UpdateEmployeeAsync(model);
                if (result > 0)
                {
                    JObject jobj = JObject.Parse(model.GCols);
                    //foreach (KeyValuePair<string, JToken> item in jobj)
                    //{
                    //    _sqlCommand.UpdateAsync("Employee", item.Key, item.Value.ToString(), result.ToString());
                    //}
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeeUpdate), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //--------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee_Remove, SuperAdmin")]
        public async Task<IActionResult> EmployeeDelete(int id)
        {
            try
            {
                var result = await _employeesService.DeleteEmployeeAsync(id);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeeDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUsersAsLookupByUserType(int userType, string userId)
        {
            try
            {
                var result = await _lookupsService.GetUsersAsLookup(userType, userId);

                return Json(new ResponseResult(responseType: ResponseType.Success, result));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("EmployeesController" + nameof(EmployeeDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
    }
}
