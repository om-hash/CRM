using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Statistics
{
    public class StatisticsDTO
    {
        public int UsersCount { get; set; }
        public int ReaEstateCount { get; set; }
        public int ProjectsCount { get; set; }
        public int FavoriteCount { get; set; }
        public int TourCount { get; set; }
        public int VisitCount { get; set; }
        public int MassagesCount { get; set; }
        public List<MostRegionVisitDTO> MostRegionVisited { get; set; }
        public List<MostRealEstateVisitDTO> MostRealEstateVisited { get; set; }
        public List<MostProjectVisitDTO> MostProjectVisited { get; set; }
        public List<MostBlogVisitDTO> MostBlogsVisited { get; set; }
    }
}
