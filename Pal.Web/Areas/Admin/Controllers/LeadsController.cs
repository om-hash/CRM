using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Pal.Core.Enums;
using Pal.Core.Enums.Customer;
using Pal.Data.DTOs.Customer;
using Pal.Services.DataServices.Customers;
using Pal.Services.DataServices.Lookups;
using Pal.Services.ExcelManager;
using Pal.Services.FileManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.PalFunctions;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Inputs;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]

    public class LeadsController : BaseController
    {
        private readonly IFileManagerService _fileManagerService;
        private readonly ICustomersService _customersService;
        private readonly ILookupsService _lookupsService;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IExcel _Excel;

        public LeadsController(
            IFileManagerService fileManagerService,
            ILanguageService languageService, ILocalizationService localizationService, 
            IMapper mapper, ICustomersService customersService, 
            ILookupsService lookupsService, INotificationService _notificationService, 
            ILoggerService logger, IExcel Excel) : base(languageService, localizationService)
        {
            _fileManagerService = fileManagerService;
            _customersService = customersService;
            _lookupsService = lookupsService;
            _logger = logger;
            _mapper = mapper;
            _Excel = Excel;
        }


        //------------------------------------------------------------------------
        private async Task GetComboBoxes()
        {
            ViewBag.cbNationalities = (await _lookupsService.GetSysNationalities()).ToSelectList();
            ViewBag.cbCountry = (await _lookupsService.GetSysCountries()).ToSelectList();
            //ViewBag.cbCity =  _lookupsService.GetSysCities().ToSelectList();
            ViewBag.cbCustomerStatus = new SelectList(Enum.GetNames(typeof(CustomerStatus)).ToList());
            ViewBag.Employees = (await _lookupsService.GetEmployeeAsLookup()).ToSelectList();
            ViewData["LeadSources"] = await _lookupsService.GetLeadSources();
            ViewData["LeadStatus"] = await _lookupsService.GetLeadStatus();
            ViewData["LeadRating"] = await _lookupsService.GetLeadRating();
            //ViewBag.Projects = (await _lookupsService.GetProjectsAsLookup()).Select(a => new
            //{ 
            //    text = a.Name,
            //    id = a.Id,
            //}).ToList(); 
        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Lead_List, SuperAdmin")]
        public IActionResult LeadsList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LeadsController" + nameof(LeadsList), ex);
                return NotFound();
            }

        }
        
        //---------------------------------------------------------------------------
        [Authorize(Roles = "Lead_List, SuperAdmin")]
        public async Task<IActionResult> LeadsPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var model = await _customersService.GetLeadsAsListAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("LeadsController" + nameof(LeadsList), ex);
                return NotFound();
            }

        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Lead_Add, SuperAdmin")]
        [Route("UploadList")]
        [HttpPost]
        public IActionResult UploadListAsync(IFormFile ExcelFile)
        {
            try
            {
                string filePath = _Excel.Documentupload(ExcelFile);
                DataTable excelDT = _Excel.excelToDt(filePath);

                var cmdText =
                $@"INSERT INTO {excelDT.TableName} 
                ()
                VALUES ()";

                _Excel.ImportDocument(excelDT, cmdText);
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(LeadAdd), ex);
                return NotFound();
            }

        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Lead_Add, SuperAdmin")]
        public async Task<IActionResult> LeadAdd()
        {
            try
            {
                await GetComboBoxes();
                CustomerDTO model = new()
                {
                    CountryId = 1,
                    CustomerStatus = CustomerStatus.New,
                    NationalityId = 1,
                    FullName = "",
                    WhatsappNumber = "",
                    IsLead = true,
                    CityId = 1,
                    LeadRatingId = 1,
                    LeadSourceId = 1,
                    LeadStatusId = 1
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(LeadAdd), ex);
                return NotFound();
            }

        }

        //---------------------------------------------------------------------------

        [HttpPost]
        [Authorize(Roles = "Lead_Add, SuperAdmin")]
        public async Task<IActionResult> LeadAdd(CustomerDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                model.IsLead = true;
                //_fileManagerService.UploadFileAsync(model.MainImageFile, Core.Enums.Attachment.ReferenceType.Customer);
            
                var result = await _customersService.AddAsync(model);

                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(LeadAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }

        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_List, SuperAdmin")]
        public async Task<IActionResult> LeadUpdate(long id)
        {
            try
            {
                var cust = await _customersService.GetByIdAsync(id);
                await GetComboBoxes();
                return View(cust);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(LeadUpdate), ex);
                return NotFound();
            }

        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LeadUpdate(long id, CustomerDTO model)
        {
            try
            {
                model.IsLead = true;
                var result = await _customersService.UpdateAsync(id, model);
                if (result > 0)
                {
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));
                }
                await GetComboBoxes();
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(LeadUpdate), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LeadDelete(long id)
        {
            try
            {
                var result = await _customersService.DeleteAsync(id);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(LeadDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //--------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Note_Add, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CustomerNoteAdd(string note, string customerId)
        {
            try
            {
                var result = await _customersService.SaveCustomerNote(note, customerId);
                if (result != null)
                    return Json(new ResponseResult(ResponseType.Success, result));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerNoteAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Note_Update, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CustomerNoteUpdate(int id, string content)
        {
            try
            {
                var result = await _customersService.UpdateCustomerNote(id, content);
                if (result != "")
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerNoteAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Note_Delete, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CustomerNoteDelete(int id)
        {
            try
            {
                var result = await _customersService.DeleteCustomerNote(id);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be Remove!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerNoteAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //---------------------------------------------------------------------------
        [HttpPost]
        [Authorize(Roles = "ConvertLeadToCustomer, SuperAdmin")]
        public async Task<IActionResult> ConvertToCustomer(long id)
        {
            try
            {
                if (await _customersService.ConvertLeadToCustomer(id))
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be Converted!"));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(ConvertToCustomer), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //---------------------------------------------------------------------------
    }
}
