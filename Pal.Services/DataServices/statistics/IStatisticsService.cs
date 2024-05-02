using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Statistics;
using Pal.Data.DTOs.Statistics;
using Pal.Data.DTOs.Statistics.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.statistics
{
    public interface IStatisticsService
    {
        Task<BubbleChartDTO> GetDealStatistics();
        Task<StatisticsDTO> GetStatistics(DateTimeType Date, DateTime? dateF, DateTime? dateT);
        Task<LatestPropertiesDTO> LatestPropertiesAdded(LogTransType actionType);
        Task<List<MostCustomersActiveDTO>> MostActiveCustomer();
        //Task<List<PyramidDataDTO>> MostAgentsActive();
        Task<LineChart> MostPropertiesVisitingCount(DateTimeType Date, DateTime? dateF, DateTime? dateT,string ActionName);
    }
}
