﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Statistics
{
    public class BubbleChartDTO
    {
        public double MaxValue { get; set; }
        public double Step { get; set; }
        public List<BubbleChartDataDTO> Data { get; set; }
    }
}
