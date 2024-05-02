using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Statistics.Charts
{
    public class LineChart
    {
        public string Intervalvalue { get; set; }
        public string MaxValue { get; set; }
        public string ChartId { get; set; }
        public string ChartTitle { get; set; }
        public List<LineChartData> ChartData { get; set; }
   
    }
}
