using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pal.Core.Enums;
using Pal.Core.Enums.Customer;
using Pal.Data.DTOs.Customer;
using Pal.Services.DataServices.Customers;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.PalFunctions;
using Pal.Services.WebWorkContext;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CustomersController : BaseController
    {
        private readonly ICustomersService _customersService;
        private readonly ILookupsService _lookupsService;
        private readonly ILoggerService _logger;
        private readonly IWebWorkContext _webWorkContext;


        public CustomersController(
            ILanguageService languageService, ILocalizationService localizationService, ICustomersService customersService, ILookupsService lookupsService, INotificationService _notificationService, ILoggerService logger, IWebWorkContext webWorkContext) : base(languageService, localizationService)
        {
            _customersService = customersService;
            _lookupsService = lookupsService;
            _logger = logger;
            _webWorkContext = webWorkContext;
        }


        //------------------------------------------------------------------------
        private async Task GetComboBoxes()
        {
            ViewBag.cbNationalities = (await _lookupsService.GetSysNationalities()).ToSelectList();
            ViewBag.cbCountry = (await _lookupsService.GetSysCountries()).ToSelectList();
            ViewBag.cbCustomerStatus = new SelectList(Enum.GetNames(typeof(CustomerStatus)).ToList()); //Enum.GetNames(typeof(CustomerStatus)).ToList();
            ViewBag.Employees = (await _lookupsService.GetEmployeeAsLookup()).ToSelectList();
            //ViewBag.Projects = (await _lookupsService.GetProjectsAsLookup()).Select(a => new
            //{
            //    text = a.Name,
            //    id = a.Id,
            //}).ToList(); ;
            ViewData["LeadSources"] = await _lookupsService.GetLeadSources();
            ViewData["LeadStatus"] = await _lookupsService.GetLeadStatus();
            ViewData["LeadRating"] = await _lookupsService.GetLeadRating();
        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_List, SuperAdmin")]
        public async Task<IActionResult> CustomersList()
        {
            try
            {
                await GetComboBoxes();
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomersList), ex);
                return NotFound();
            }

        }
        
        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_List, SuperAdmin")]
        public async Task<IActionResult> CustomersPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var model = await _customersService.GetCustomersAsListAsync(dm);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomersList), ex);
                return NotFound();
            }

        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Add, SuperAdmin")]
        public async Task<IActionResult> CustomerAdd()
        {
            try
            {
                await GetComboBoxes();
                var model = new CustomerDTO
                {
                    CountryId = 1,
                    CustomerStatus = Core.Enums.Customer.CustomerStatus.New,
                    NationalityId = 1,
                    PhoneNumber = "1213",
                    FullName = "dsdsds",
                    WhatsappNumber = "fdfsfs",
                    Username = "Customer",
                    Password = "Customer123!@#"
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerAdd), ex);
                return NotFound();
            }

        }

        //---------------------------------------------------------------------------
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer_Add, SuperAdmin")]
        public async Task<IActionResult> CustomerAdd(CustomerDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));


                var result = await _customersService.AddAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerAdd), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }


        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_List, SuperAdmin")]
        public async Task<IActionResult> CustomerUpdate(long id)
        {
            try
            {
                var project = await _customersService.GetByIdAsync(id);
                await GetComboBoxes();
                return View(project);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerUpdate), ex);
                return NotFound();
            }

        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Edit, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerUpdate(long id, CustomerDTO model)
        {
            try
            {
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
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerUpdate), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_Remove, SuperAdmin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerDelete(long id)
        {
            try
            {
                var result = await _customersService.DeleteAsync(id);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_List, SuperAdmin")]
        public async Task<IActionResult> TimeLine(long id)
        {
            try
            {
                var timeLine = await _customersService.GetCustomerTimeLine(Convert.ToInt32(id));
                return View(timeLine);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(TimeLine), ex);
                return NotFound();
            }

        }
        //---------------------------------------------------------------------------
        [Authorize(Roles = "Customer_List, SuperAdmin")]
        public async Task<IActionResult> CustomerDetails(long id, int? isAjax)
        {
            try
            {
                var project = await _customersService.CustomerDetails(id);
                if (isAjax == 1)
                    return PartialView(project);

                return View(project);
            }

            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(CustomerDetails), ex);
                return NotFound();
            }

        }
        //---------------------------------------------------------------------------
        [HttpPost]
        [Authorize(Roles = "ConvertCustomerToLead, SuperAdmin")]
        public async Task<IActionResult> ConvertToLead(long id)
        {
            try
            {
                if (await _customersService.ConvertCustomerToLead(id))
                    return Json(new ResponseResult(ResponseType.Success));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be Converted!"));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(ConvertToLead), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //---------------------------------------------------------------------------

        public async Task<IActionResult> CustomerInformationReport(int id)
        {
         
            var IsRtl = _languageService.IsLayoutRtl();
            string path = "";
            var cutomerInfo = await _customersService.CustomerDetails(id);
            var model = new List<CustomerDetailsDTO>();
            model.Add(cutomerInfo);
            WebReport customerReport = new WebReport();

            switch (IsRtl)
            {
                case true:
                    path = Directory.GetCurrentDirectory() + "\\Reports\\CutomerInformationReport_rtl.frx";
                    customerReport.Report.Load(path);
                    break;
                case false:
                    path = Directory.GetCurrentDirectory() + "\\Reports\\CutomerInformationReport_ltr.frx";
                    customerReport.Report.Load(path);
                    break;
                
            }
            customerReport.Report.SetParameterValue("TitleText", Localize("report.CustomerInformation.TitleText").ToString());
            customerReport.Report.SetParameterValue("TcText", Localize("report.CustomerInformation.TcText").ToString());
            customerReport.Report.SetParameterValue("NameText", Localize("report.CustomerInformation.NameText").ToString());
            customerReport.Report.SetParameterValue("PassportText", Localize("report.CustomerInformation.PassportText").ToString());
            customerReport.Report.SetParameterValue("NationalityText", Localize("report.CustomerInformation.NationalityText").ToString());
            customerReport.Report.SetParameterValue("OccupationText", Localize("report.CustomerInformation.OccupationText").ToString());
            customerReport.Report.SetParameterValue("EmailText", Localize("report.CustomerInformation.EmailText").ToString());
            customerReport.Report.SetParameterValue("PhoneText", Localize("report.CustomerInformation.PhoneText").ToString());
            customerReport.Report.SetParameterValue("WhatsAppText", Localize("report.CustomerInformation.WhatsAppText").ToString());
            customerReport.Report.SetParameterValue("HomePhoneText", Localize("report.CustomerInformation.HomePhoneText").ToString());
            customerReport.Report.SetParameterValue("PoBoxText", Localize("report.CustomerInformation.PoBoxText").ToString());
            customerReport.Report.SetParameterValue("CityText", Localize("report.CustomerInformation.CityText").ToString());
            customerReport.Report.SetParameterValue("OfficePhoneText", Localize("report.CustomerInformation.OfficePhoneText").ToString());
            customerReport.Report.SetParameterValue("DateOfBirthText", Localize("report.CustomerInformation.DateOfBirthText").ToString());
            customerReport.Report.SetParameterValue("RequestDetailsText", Localize("report.CustomerInformation.RequestDetailsText").ToString());
            customerReport.Report.SetParameterValue("CustomerSignatureText", Localize("report.CustomerInformation.CustomerSignatureText").ToString());
            customerReport.Report.RegisterData(model, "Customer");
            customerReport.Report.Prepare();

            Stream stream = new MemoryStream();
            customerReport.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return new FileStreamResult(stream, "application/pdf");

        }
       
        //public async Task<IActionResult> rrr()
        //{
        //    try
        //    {
        //        await _customersService.GetSavedFilterForCustomer();

        //        return Json(new ResponseResult(ResponseType.Success));

        //    }
        //    catch (Exception ex)
        //    {
                
        //        return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
        //    }
        //}
    }
}
