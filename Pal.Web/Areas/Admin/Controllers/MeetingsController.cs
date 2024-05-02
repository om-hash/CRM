using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Enums;
using Pal.Data.Contexts;
using Pal.Data.DTOs.CRM.Meeting;
using Pal.Services.CRM.Meetings;
using Pal.Services.DataServices.Employees;
using Pal.Services.DataServices.Lookups;
using Pal.Services.ExcelManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.PalFunctions;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class MeetingsController : BaseController
    {
        private readonly IMeetingService _meetingService;
        private readonly IEmployeesService _employeesService;
        private readonly ApplicationDbContext _context;
        private readonly ILookupsService _lookupsService;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IExcel _Excel;

        public MeetingsController(IEmployeesService employeesService, ApplicationDbContext context, 
           ILanguageService languageService, ILocalizationService localizationService, 
           IMapper mapper, IMeetingService meetingService, ILookupsService lookupsService, 
           ILoggerService logger, IExcel Excel) : base(languageService, localizationService)
        {
            _context = context;
            _employeesService = employeesService;
            _meetingService = meetingService;
            _lookupsService = lookupsService;
            _logger = logger;
            _mapper = mapper;
            _Excel= Excel;  
        }
        private async Task GetComboBoxes()
        {
            ViewBag.Employee = (await _lookupsService.GetEmployeeAsLookup()).ToSelectList();
            ViewBag.Customers = (await _lookupsService.GetCustomerAsLookup()).ToSelectList();

            ViewBag.Customers1 = await _lookupsService.GetCustomerAsLookup();
            ViewBag.Leads1 = await _lookupsService.GetLeadsAsLookup();
            ViewBag.Companies1 = await _lookupsService.GetCompaniesAsLookup();
        }
        //---------------------------------------------------
        [Authorize(Roles = "Meeting_List, SuperAdmin")]
        public IActionResult MeetingsList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("MeetingController" + nameof(MeetingsList), ex);
                return NotFound();
            }
        }
        
        //---------------------------------------------------
        [Authorize(Roles = "Meeting_List, SuperAdmin")]
        public async Task<IActionResult> MeetingsPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var model = await _meetingService.GetAllAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("MeetingController" + nameof(MeetingsPaginatedList), ex);
                return NotFound();
            }
        }
        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Meeting_Add, SuperAdmin")]
        public async Task<IActionResult> MeetingAdd()
        {
            try
            {
                var model = new MeetingDTO { };
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("MeetingController" + nameof(MeetingAdd), ex);
                return NotFound();
            }

        }
        //--------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Meeting_Add, SuperAdmin")]
        public async Task<IActionResult> MettingAdd(MeetingDTO model)
        {
            model.CustomerName = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                var result = await _meetingService.AddAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("MeetingController" + nameof(MettingAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //------------------------------------------------------------------------

        [Authorize(Roles = "Meeting_Add, SuperAdmin")]
        //[Route("UploadList")]
        //[HttpPost]
        public IActionResult UploadListAsync(IFormFile ExcelFile)
        {
            try
            {
                string filePath = _Excel.Documentupload(ExcelFile);
                DataTable excelDT = _Excel.excelToDt(filePath);

                //var cmdText =
                //$@"INSERT INTO {excelDT.TableName} 
                //([Title] ,[Location] ,[From] ,[To] ,[EmployeeId] ,[CustomerId] ,
                //[Description] ,[CreationDate] ,[CreatedBy] ,[IsDeleted])
                //OUTPUT INSERTED.Id
                //VALUES
                //(NULLIF(@Title, NULL) ,NULLIF(@Location, NULL) ,NULLIF(@From, NULL) ,NULLIF(@To, NULL) ,NULLIF(@EmployeeId, NULL) ,NULLIF(@CustomerId, NULL) ,
                //NULLIF(@Description, NULL) ,NULLIF(@CreationDate, NULL) ,NULLIF(@CreatedBy, NULL) ,0)";

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var refId = _context.Users.FirstOrDefault(i => i.Id == userId).ReferenceId;
                //var empId = _employeesService.GetEmployeeIdByUerIdAsync(userId).Result.ToString();

                var cmdText =
                $@"INSERT INTO {excelDT.TableName} 
                ([Title] ,[Location] ,[From] ,[To] ,[Description] ,[IsDeleted]) OUTPUT INSERTED.Id VALUES
                (NULLIF(@Title, NULL) ,NULLIF(@Location, NULL) ,NULLIF(@From, NULL) ,
                NULLIF(@To, NULL), NULLIF(@Description, NULL), 0)";

                
                _Excel.ImportDocument(excelDT, cmdText);
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(MeetingAdd), ex);
                return NotFound();
            }

        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Meeting_List, SuperAdmin")]
        public async Task<IActionResult> MeetingUpdate(int id)
        {
            try
            {
                var project = await _meetingService.GetMeetingByIdAsync(id);
                await GetComboBoxes();
                return View(project);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("MeetingController" + nameof(MeetingUpdate), ex);
                return NotFound();
            }

        }
        //------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Meeting_Edit, SuperAdmin")]
        public async Task<IActionResult> MeetingUpdate(MeetingDTO model)
        {
            try
            {
                var result = await _meetingService.UpdateAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("MeetingController" + nameof(MeetingUpdate), ex);
                return NotFound();
            }
        }
        //------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Meeting_Remove, SuperAdmin")]
        public async Task<IActionResult> MeetingDelete(int id)
        {
            try
            {
                var result = await _meetingService.DeleteAsync(id);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("MeetingController" + nameof(MeetingDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //------------------------------------------------------------------------

    }
}
