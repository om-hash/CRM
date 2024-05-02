using Microsoft.AspNetCore.Mvc;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Enums;
using Pal.Core.Enums.ActivityLog;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using Pal.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoggerController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly IWebWorkContext _workContext;
        public LoggerController(ILoggerService logger, IWebWorkContext workContext)
        {
            _loggerService = logger;
            _workContext = workContext;
        }
        //--------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> LogInfo(string actionName, ActionType actionType, LogTransType transType, string transReferenceId, int relatedTo)
        {
            try
            {
                await _loggerService.LogInfoAsync(new ActivityLog { ActionName = actionName, ActionType = actionType, TransReferenceId = transReferenceId, TransType = transType, RelatedTo = (relatedTo == 0) ?  int.Parse(_workContext.GetMyCompanyId()): relatedTo });
                return Json(new ResponseResult(ResponseType.Success));
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("LoggerController" + nameof(LogInfo), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
        //--------------------------------------------------------------------------------------------

        public async Task<IActionResult> LogVisit(string key)
        {
            try
            {
                await _loggerService.LogVisitAsync(key);
                return Json(new ResponseResult(ResponseType.Success));
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync("LoggerController" + nameof(LogVisit), ex);
                return Json(new ResponseResult(ResponseType.Error, ex.GetError()));
            }
        }
    }
}
