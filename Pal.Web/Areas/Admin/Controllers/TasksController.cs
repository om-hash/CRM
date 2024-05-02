using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pal.Core.Domains.Lookups;
using Pal.Core.Enums;
using Pal.Data.DTOs.CRM.Task;
using Pal.Services.CRM.Tasks;
using Pal.Services.DataServices.Employees;
using Pal.Services.DataServices.Lookups;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.PalFunctions;
using Pal.Web.Controllers;
using Pal.Web.Extensions;
using Pal.Core.Enums.Roles;
using Pal.Data.DTOs.Employees;
using Pal.Services.WebWorkContext;
using Syncfusion.EJ2.Base;
using System.Data;
using Pal.Services.ExcelManager;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class TasksController : BaseController
    {
        private readonly ITasksService _tasksService;
        private readonly ILoggerService _logger;
        private readonly IEmployeesService _employeesService;
        private readonly ILookupsService _lookupsService;
        private readonly IWebWorkContext _workContext;
        private readonly IExcel _Excel;

        public TasksController(ITasksService tasksService, ILoggerService loggerService,
            ILanguageService languageService,
            ILookupsService lookupsService,
            ILocalizationService localizationService,
            IEmployeesService employeesService, IWebWorkContext webWork, 
            IExcel Excel) : base(languageService, localizationService)
        {
            _tasksService = tasksService;
            _logger = loggerService;
            _lookupsService = lookupsService;
            _workContext = webWork;
            _employeesService = employeesService;
            _Excel = Excel;
        }

        //------------------------------------------------------------------------
        private async Task GetComboBoxes()
        {
            ViewBag.Statuses = (await _lookupsService.GetSysTaskStatuses()).ToSelectList();
            ViewBag.Employees = (await _lookupsService.GetEmployeeAsLookup()).ToSelectList();
            ViewBag.Customers = (await _lookupsService.GetCustomerAsLookup()).ToSelectList();
            ViewBag.Decisiones = (await _lookupsService.GetDecisionAsLookup()).ToSelectList();

            ViewBag.Customers1 = await _lookupsService.GetCustomerAsLookup();
            ViewBag.Leads1 = await _lookupsService.GetLeadsAsLookup();
            ViewBag.Companies1 = await _lookupsService.GetCompaniesAsLookup();
        }

        [Authorize(Roles = "Task_List, Task_Manager, SuperAdmin")]
        public IActionResult TasksList()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("ProjectsController" + nameof(TasksList), ex);
                return NotFound();
            }
        }
        
        [Authorize(Roles = "Task_List, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> TasksPaginatedList([FromBody] DataManagerRequest dm)
        {
            try
            {
                var roles = User.FindAll(ClaimTypes.Role).Where(c => c.Value == "SuperAdmin" || c.Value == "Task_Manager").ToList();
                int? employeeId = null;
                if (roles.Count == 0)
                    employeeId = await _employeesService.GetEmployeeIdByUerIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);


                var model = await _tasksService.GetAllAsListAsync(dm, employeeId);
                return dm.RequiresCounts ? Json(new { result = model.Data, count = model.TotalCount }) : Json(model);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("ProjectsController" + nameof(TasksList), ex);
                return NotFound();
            }
        }

        //------------------------------------------------------------------------
        [Authorize(Roles = "Task_Add, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> TaskAdd()
        {
            try
            {
                await GetComboBoxes();

                //if (model == null)
                //model = new TaskCreateDTO();

                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(TaskAdd), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------
        [HttpPost]
        [Authorize(Roles = "Task_Add, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> TaskAdd(TaskCreateDTO model)
        {
            model.CustomerName = string.Empty;  
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                model.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                var result = await _tasksService.AddTaskAsync(model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(TaskAdd), ex);
                return NotFound();
            }
        }

        //------------------------------------------------------------------------

        [Authorize(Roles = "Task_Add, SuperAdmin")]
        //[Route("UploadList")]
        //[HttpPost]
        public IActionResult UploadListAsync(IFormFile ExcelFile)
        {
            try
            {
                string filePath = _Excel.Documentupload(ExcelFile);
                DataTable excelDT = _Excel.excelToDt(filePath);
                
                var cmdText =
                $@"INSERT INTO {excelDT.TableName}
                ([EmployeeId] ,[Subject] ,[CustomerId] ,[StatusId] ,[TaskPriority] ,
                [Description] ,[StartDate] ,[EndDate] ,[CreationDate] ,[CreatedBy] ,[IsDeleted])
                OUTPUT INSERTED.Id
                VALUES
                (NULLIF(@EmployeeId, NULL), NULLIF(@Subject, NULL), NULLIF(@CustomerId, NULL), 
                NULLIF(@StatusId, NULL), NULLIF(@TaskPriority, NULL), 
                NULLIF(@Description, NULL), NULLIF(@StartDate, NULL), NULLIF(@EndDate, NULL), 
                NULLIF(@CreationDate, NULL), NULLIF(@CreatedBy, NULL), 0)";

                _Excel.ImportDocument(excelDT, cmdText);
                return View();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(TaskAdd), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------

        [Authorize(Roles = "Task_Edit, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> TaskUpdate(int id)
        {
            try
            {
                var project = await _tasksService.GetTaskByIdAsync(id);
                await GetComboBoxes();
                return View(project);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(TaskUpdate), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------

        [HttpPost]
        [Authorize(Roles = "Task_Edit, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> TaskUpdate(TaskCreateDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new ResponseResult(ResponseType.Error, "ModelNotValid"));

                var result = await _tasksService.UpdateTaskAsync(model.Id, model);
                if (result > 0)
                    return Json(new ResponseResult(ResponseType.Success, result.ToString()));

                else
                    return Json(new ResponseResult(ResponseType.Error, "Cannot be saved!"));
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(TaskAdd), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Task_Remove, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> TaskDelete(int id)
        {
            try
            {
                var result = await _tasksService.DeleteTaskAsync(id);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(TaskDelete), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Roles = "Task_Details, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> TaskDetails(int id)
        {
            try
            {
                var roles = User.FindAll(ClaimTypes.Role).Where(c => c.Value == "SuperAdmin" || c.Value == "Task_Manager").ToList();
                int employeeId = await _employeesService.GetEmployeeIdByUerIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var task = await _tasksService.GetTaskDetailsByIdAsync(id, roles.Count > 0, employeeId);
                ViewBag.TaskStatuses = (await _lookupsService.GetSysTaskStatuses()).ToSelectList();
                return View(task);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(TaskDetails), ex);
                return NotFound();
            }

        }

        //------------------------------------------------------------------------
        [HttpPost]
        [Authorize(Roles = "Task_Details, Task_Manager, SuperAdmin")]
        public async Task<IActionResult> ChangeTaskStatus(int id, int statusId)
        {
            try
            {
                var roles = User.FindAll(ClaimTypes.Role).Where(c => c.Value == "SuperAdmin" || c.Value == "Task_Manager").ToList();
                int employeeId = await _employeesService.GetEmployeeIdByUerIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _tasksService.ChangeTaskStatus(id, statusId, roles.Count > 0, employeeId);

                return Json(new ResponseResult(result, result.ToString()));

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(ChangeTaskStatus), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }


        //------------------------------------------------------------------------
        [Authorize(Roles = "Task_List,SuperAdmin")]
        public async Task<IActionResult> TasksScheduler()
        {
            //ViewBag.datasource = GetResourceData();
            var employees = (await _employeesService.GetAllAsListAsync(new DataManagerRequest())).Data.Select(a => new EmployeesList
            {
                Id = a.Id,
                FullName = a.FullName,
                JobTitle = a.JobTitle,
                Color = SysColor.GetRandomHexColor(),
            }).ToList();

            var tasks = ((await _tasksService.GetAllAsListAsync(null))).Data.Select(a => new
            {
                a.Id,
                a.EmployeeId,
                a.IsAllDay,
                StartTime = a.StartDate,
                EndTime = a.EndDate,
                a.Subject,
                Proirity = a.TaskPriority.ToString(),
                a.Status,
            });
            ViewBag.Resources = new string[] { "Employees" };
            ViewBag.Employees = employees;
            ViewBag.Tasks = tasks;
            return View();
        }
        //------------------------------------------------------------------------
        [Authorize(Roles = "Task_MyTasks_Scheduler,SuperAdmin")]
        public async Task<IActionResult> MyTasks()
        {
            try
            {
                var userId = _workContext.GetMyUserId();
                var isEmployee = _workContext.IsUserEmployee(userId);
                if (!isEmployee)
                    return RedirectToAction(nameof(TasksScheduler));

                var result = await _tasksService.TasksSchedulerForEmployee();
                ViewBag.Tasks = result.TaskScheduler;
                ViewBag.IsinRole = result.IsInRole;
                return View();


            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("TasksController" + nameof(MyTasks), ex);
                return NotFound();
            }
        }



        //    public List<ResourceData> GetResourceData()
        //    {
        //        List<ResourceData> resourceData = new();
        //        resourceData.Add(new ResourceData
        //        {
        //            Id = 1,
        //            Subject = "Requirement planning",
        //            StartTime = new DateTime(2022, 9, 3, 10, 0, 0),
        //            EndTime = new DateTime(2022, 9, 3, 12, 0, 0),
        //            IsAllDay = false,
        //            OwnerId = 1,
        //            ProjectId = 1,
        //        });
        //        resourceData.Add(new ResourceData
        //        {
        //            Id = 2,
        //            Subject = "Quality Analysis",
        //            StartTime = new DateTime(2022, 9, 4, 10, 0, 0),
        //            EndTime = new DateTime(2022, 9, 4, 12, 0, 0),
        //            IsAllDay = false,
        //            OwnerId = 2,
        //            ProjectId = 1,
        //        });
        //        resourceData.Add(new ResourceData
        //        {
        //            Id = 3,
        //            Subject = "Resource planning",
        //            StartTime = new DateTime(2022, 9, 5, 10, 0, 0),
        //            EndTime = new DateTime(2022, 9, 5, 12, 0, 0),
        //            IsAllDay = false,
        //            OwnerId = 3,
        //            ProjectId = 2,
        //        });
        //        return resourceData;
        //    }

        //    public class ResourceData
        //    {
        //        public int Id { get; set; }
        //        public string Subject { get; set; }
        //        public DateTime StartTime { get; set; }
        //        public DateTime EndTime { get; set; }
        //        public bool IsAllDay { get; set; }
        //        public int OwnerId { get; set; }
        //        public int ProjectId { get; internal set; }
        //    }
        //    public class OwnerResource
        //    {
        //        public string OwnerText { set; get; }
        //        public int Id { set; get; }
        //        public string OwnerColor { set; get; }
        //        public int GroupId { get; internal set; }
        //    }
        //}

        //internal class ResourceDataSourceModel
        //{
        //    public string text { get; set; }
        //    public int id { get; set; }
        //    public string color { get; set; }

        //}








        // public class ScheduleData
        // {
        //     public List<BlockoutData> GetBlockData()
        //     {
        //         List<BlockoutData> blockData = new List<BlockoutData>();
        //         blockData.Add(new BlockoutData { id = 1, subject = "MAINTENANCE", StartTime = new DateTime(2022, 9, 22, 8, 0, 0), EndTime = new DateTime(2022, 9, 22, 10, 0, 0), Ownerid = "2", Resourceid = "3", IsBlockAppointment = true, IsAllDay = false });
        //         blockData.Add(new BlockoutData { id = 2, subject = "SERVICE", StartTime = new DateTime(2022, 9, 26, 9, 0, 0), EndTime = new DateTime(2022, 9, 26, 11, 0, 0), Ownerid = "1", Resourceid = "1", IsBlockAppointment = true, IsAllDay = false });
        //         blockData.Add(new BlockoutData { id = 3, subject = "SERVICE", StartTime = new DateTime(2022, 9, 28), EndTime = new DateTime(2022, 9, 28), Ownerid = "1", Resourceid = "1", IsBlockAppointment = true, IsAllDay = true });
        //         blockData.Add(new BlockoutData { id = 4, subject = "BAD MONSOON", StartTime = new DateTime(2022, 9, 29, 15, 30, 0), EndTime = new DateTime(2022, 9, 29, 17, 0, 0), Ownerid = "1", Resourceid = "1", IsBlockAppointment = true, IsAllDay = false });
        //         blockData.Add(new BlockoutData { id = 5, subject = "HOLIDAY", StartTime = new DateTime(2022, 9, 22, 15, 30, 0), EndTime = new DateTime(2022, 9, 22, 17, 0, 0), Ownerid = "2", Resourceid = "3", IsBlockAppointment = true, IsAllDay = false });
        //         blockData.Add(new BlockoutData { id = 6, subject = "BAD MONSOON", StartTime = new DateTime(2022, 9, 30, 10, 30, 0), EndTime = new DateTime(2022, 9, 30, 17, 0, 0), Ownerid = "1", Resourceid = "5", IsBlockAppointment = true, IsAllDay = false });
        //         return blockData;
        //     }
        // }

        // public class BlockoutData
        // {
        //     public int id { get; set; }
        //     public string subject { get; set; }
        //     public DateTime StartTime { get; set; }
        //     public DateTime EndTime { get; set; }
        //     public string Resourceid { get; set; }
        //     public string Ownerid { get; set; }
        //     public bool IsAllDay { get; set; }
        //     public bool IsBlockAppointment { get; set; }
        // }


    }
}