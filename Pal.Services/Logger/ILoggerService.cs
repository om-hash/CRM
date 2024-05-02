using Pal.Core.Domains.Activity_logs;
using Pal.Core.Enums.ActivityLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.Logger
{
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);

        Task LogInfoAsync(ActivityLog model);
        Task LogWarnAsync(string message);
        Task LogDebugAsync(string message);
        Task LogErrorAsync(string action,Exception exception , Importance importance = Importance.Medium);
        Task LogVisitAsync(string LocalStorageKey);
    }
}
