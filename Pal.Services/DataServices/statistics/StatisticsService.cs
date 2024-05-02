using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Statistics;
using Pal.Data.Contexts;
using Pal.Data.DTOs.Statistics;
using Pal.Data.DTOs.Statistics.Charts;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;
       
        private readonly ILoggerService _logger;
        private readonly IWebWorkContext _workContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILanguageService _languageService;

        public StatisticsService(ApplicationDbContext context, IWebWorkContext workContext,  ILoggerService logger,
            IHttpContextAccessor httpContextAccessor, ILanguageService languageService)
        {
            _context = context;
      
            _workContext = workContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _languageService = languageService;
        }

        private string GetintervalValue(string MaxVal)
        {
            try
            {
                var response = int.Parse(MaxVal) / 5;
                return response.ToString();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetintervalValue), ex);
                return null;
            }
        }

        private string GetChartMaxY(string type)
        {
            try
            {
                var model = _context.ActivityLogs.Where(a => a.ActionName == type && a.ActionType == ActionType.View).Count().ToString();
                int zeroDigits = model.Length - 1;
                var firstDigit = int.Parse(model.Substring(0, 1)) + 1;
                var max = firstDigit.ToString();
                for (int i = 0; i < zeroDigits; ++i)
                {
                    max = max + "0";
                }
                return max;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetChartMaxY), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------
        private string GetPrapirtyName(string id, LogTransType TransType)
        {
            try
            {
                var langId = _languageService.GetLanguageIdFromRequestAsync().Result;
                switch (TransType)
                {

                    case LogTransType.Regions:
                        return _context.SysRegionTranslates.Where(x => x.RegionId == int.Parse(id) && x.LanguageId == langId).Select(x => x.RegionName).FirstOrDefault();
                    //case LogTransType.Projects:
                    //    return _context.ProjectTranslates.Where(x => x.ProjectId == int.Parse(id) && x.LanguageId == langId).Select(x => x.ProjectName).FirstOrDefault();
                    //case LogTransType.RealEstates:
                    //    return _context.RealEstateTranslates.Where(x => x.RealEstateId == int.Parse(id) && x.LanguageId == langId).Select(x => x.Title).FirstOrDefault();
                    //    //case LogTransType.Customer:
                    //    //    return _context.c

                }
                return id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetPrapirtyName), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------
        public async Task<StatisticsDTO> GetStatistics(DateTimeType period, DateTime? dateF, DateTime? dateT)
        {

            try
            {

                var statistic = _context.ActivityLogs.AsQueryable();

                string whoAmI = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value;

                switch (whoAmI)
                {
                    case nameof(UserType.Companies):
                        var compId = int.Parse(_workContext.GetMyCompanyId());
                        statistic = statistic.Where(x => x.RelatedTo == compId);
                        break;
                }

                var statisticList = Enumerable.Empty<ActivityLog>().AsQueryable();
                switch (period)
                {
                    case DateTimeType.All:
                        statisticList = statistic;
                        break;
                    case DateTimeType.ToDay:
                        statisticList = statistic.Where(x => x.TransDate.Date == DateTime.Today);
                        break;
                    case DateTimeType.Yesterday:
                        statisticList = statistic.Where(x => x.TransDate.Date == DateTime.Today.AddDays(-1));
                        break;
                    case DateTimeType.LastWeek:
                        statisticList = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-7) && x.TransDate.Date < DateTime.Today);
                        break;
                    case DateTimeType.LastMonth:
                        statisticList = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-30) && x.TransDate.Date < DateTime.Today);
                        break;
                    case DateTimeType.Last3Month:
                        statisticList = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-90) && x.TransDate.Date < DateTime.Today);
                        break;
                    case DateTimeType.custom:
                        statisticList = statistic.Where(x => x.TransDate.Date >= dateF!.Value.Date && x.TransDate.Date < dateT.Value.Date);
                        break;
                }
                var response = await statisticList.Select(x => new StatisticsDTO
                {
                    FavoriteCount = statisticList.Where(x => x.ActionName == "AddToFavoriteAsync" && x.ActionType == ActionType.Add).Count(),
                    ProjectsCount = statisticList.Where(x => x.ActionName == "AddProjectAsync" && x.ActionType == ActionType.Add).Count(),
                    ReaEstateCount = statisticList.Where(x => x.ActionName == "AddRealEstate" && x.ActionType == ActionType.Add).Count(),
                    TourCount = statisticList.Where(x => x.ActionName == "SaveTour" && x.ActionType == ActionType.Add).Count(),
                    VisitCount = statisticList.Where(x => x.ActionType == ActionType.Visite).Count(),
                }).FirstOrDefaultAsync();
                if (response != null)
                {


                    response.MostProjectVisited = (await statisticList.Where(x => x.ActionName == "GetProjectByIdForView" && x.ActionType == ActionType.View).GroupBy(w => w.TransReferenceId)
                          .Select(a => new MostProjectVisitDTO
                          {
                              Id = int.Parse(a.Key),
                              Link = "/Admin/projects/ProjectUpdate/" + a.Key,
                              VisitingCount = a.Count(),
                          }).ToListAsync()).OrderBy(x => x.VisitingCount).ToList();
                    response.MostRealEstateVisited = (await statisticList.Where(a => a.ActionName == "GetRealEstateByIdForView" && a.ActionType == ActionType.View).GroupBy(w => w.TransReferenceId)
                         .Select(a => new MostRealEstateVisitDTO
                         {
                             Id = int.Parse(a.Key),
                             Link = "/Admin/RealEstates/RealEstateUpdate/" + a.Key,
                             VisitingCount = a.Count(),
                         }).ToListAsync()).OrderByDescending(x => x.VisitingCount).ToList();
                    response.MostRegionVisited = (await statisticList.Where(x => x.ActionName == "GetRegionByidForView" && x.ActionType == ActionType.View).GroupBy(w => w.TransReferenceId)
                        .Select(a => new MostRegionVisitDTO
                        {
                            Id = int.Parse(a.Key),
                            Link = "/Admin/Region/RegionUpdate/" + a.Key,
                            VisitingCount = a.Count(),
                        }).ToListAsync()).OrderBy(x => x.VisitingCount).ToList();
                    response.MostBlogsVisited = await statisticList.Where(x => x.ActionName == "GetBlogByidForView" && x.ActionType == ActionType.View).GroupBy(w => w.TransReferenceId)
                        .Select(a => new MostBlogVisitDTO
                        {
                            Id = int.Parse(a.Key),
                            Link = "/Admin/Blogs/UpdateBlog/" + a.Key,
                            VisitingCount = a.Count(),
                        }).ToListAsync();

                    foreach (var item in response.MostProjectVisited)
                    {
                        item.Name = GetPrapirtyName(item.Id.ToString(), LogTransType.Projects);
                    }
                    foreach (var item in response.MostRealEstateVisited)
                    {
                        item.Name = GetPrapirtyName(item.Id.ToString(), LogTransType.RealEstates);
                    }
                    foreach (var item in response.MostRegionVisited)
                    {
                        item.Name = GetPrapirtyName(item.Id.ToString(), LogTransType.Regions);
                    }
                    foreach (var item in response.MostBlogsVisited)
                    {
                        item.Name = GetPrapirtyName(item.Id.ToString(), LogTransType.Blog);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetStatistics), ex);
                return null;
            }
        }

        //----------------------------------------------------------------------------------
        public async Task<LineChart> MostPropertiesVisitingCount(DateTimeType Date, DateTime? dateF, DateTime? dateT, string ActionName)
        {
            try
            {

                TimeSpan DateDifference = TimeSpan.FromDays(2);
                if (dateF != null)
                    DateDifference = dateF.Value.Date - dateT.Value.Date;
                var model = new LineChart();
                var statistic = _context.ActivityLogs.AsQueryable();
                string whoAmI = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value;

                switch (whoAmI)
                {
                    case nameof(UserType.Companies):
                        var compId = int.Parse(_workContext.GetMyCompanyId());
                        statistic = statistic.Where(x => x.RelatedTo == compId);

                        break;
                }


                var SelectedDateStatistic = new List<ActivityLog>();
                var LastDateStatistic = new List<ActivityLog>();
                switch (Date)
                {
                    case DateTimeType.All:
                        SelectedDateStatistic = statistic.OrderBy(x => x.TransDate).ToList();
                        break;
                    case DateTimeType.ToDay:
                        SelectedDateStatistic = statistic.Where(x => x.TransDate.Date == DateTime.Today).ToList();
                        LastDateStatistic = statistic.Where(x => x.TransDate.Date == DateTime.Today.AddDays(-1)).ToList();
                        break;
                    case DateTimeType.Yesterday:
                        SelectedDateStatistic = statistic.Where(x => x.TransDate.Date == DateTime.Today.AddDays(-1)).ToList();
                        LastDateStatistic = statistic.Where(x => x.TransDate.Date == DateTime.Today.AddDays(-2)).ToList();
                        break;
                    case DateTimeType.LastWeek:
                        SelectedDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-7) && x.TransDate.Date < DateTime.Today).ToList();
                        LastDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-14) && x.TransDate.Date < DateTime.Today.AddDays(-7)).ToList();
                        break;
                    case DateTimeType.LastMonth:
                        SelectedDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-30) && x.TransDate.Date < DateTime.Today).ToList();
                        LastDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-60) && x.TransDate.Date < DateTime.Today.AddDays(-30)).ToList();
                        break;
                    case DateTimeType.Last3Month:
                        SelectedDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-90) && x.TransDate.Date < DateTime.Today).ToList();
                        LastDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(-180) && x.TransDate.Date < DateTime.Today.AddDays(-90)).ToList();
                        break;
                    case DateTimeType.custom:
                        SelectedDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(DateDifference.Days) && x.TransDate.Date < DateTime.Today).ToList();
                        LastDateStatistic = statistic.Where(x => x.TransDate.Date >= DateTime.Today.AddDays(DateDifference.Days * 2) && x.TransDate.Date < DateTime.Today.AddDays(DateDifference.Days)).ToList();
                        break;
                }
                var Groubing = new List<LineChartData>();
                switch (Date)
                {
                    case DateTimeType.All:
                        Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.Date.ToString("MM/dd/yyyy")).OrderBy(a => a.Key).Select(a => new LineChartData
                        {
                            xValue = a.Key,
                            yValue = a.Count(),
                        }).ToList();
                        break;
                    case DateTimeType.ToDay:
                        Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.ToString("hh tt")).OrderBy(a => a.Key).Select(a => new LineChartData
                        {
                            xValue = a.Key,
                            yValue = a.Count(),
                            yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.ToString("hh tt") == a.Key).Count(),
                        }).ToList();
                        break;
                    case DateTimeType.Yesterday:
                        Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.ToString("hh tt")).OrderBy(a => a.Key).Select(a => new LineChartData
                        {
                            xValue = a.Key,
                            yValue = a.Count(),
                            yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.ToString("hh tt") == a.Key).Count(),
                        }).ToList();
                        break;
                    case DateTimeType.LastWeek:
                        Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.Date.ToString("dddd, dd")).OrderBy(a => a.Key).Select(a => new LineChartData
                        {
                            xValue = a.Key,
                            yValue = a.Count(),
                            yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.Date.ToString("dddd") == a.Key).Count(),
                        }).OrderBy(q => q.xValue).ToList();
                        break;
                    case DateTimeType.LastMonth:
                        Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.Date.ToString("MMMM, dd")).OrderBy(a => a.Key).Select(a => new LineChartData
                        {
                            xValue = a.Key,
                            yValue = a.Count(),
                            yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.Date.Day == DateTime.Parse(a.Key).Day).Count(),
                        }).ToList();
                        break;
                    case DateTimeType.Last3Month:
                        Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.Date.ToString("MMMM, dd")).OrderBy(a => a.Key).Select(a => new LineChartData
                        {
                            xValue = a.Key,
                            yValue = a.Count(),
                            yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.Date.Day == DateTime.Parse(a.Key).Day).Count(),
                        }).ToList();
                        break;
                    case DateTimeType.custom:
                        if (DateDifference.Days > -2)
                        {
                            Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.ToString("hh tt")).OrderBy(a => a.Key).Select(a => new LineChartData
                            {
                                xValue = a.Key,
                                yValue = a.Count(),
                                yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.ToString("hh tt") == a.Key).Count(),
                            }).ToList();
                        }
                        else if (DateDifference.Days > -7)
                        {
                            Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.Date.ToString("dddd, dd")).OrderBy(a => a.Key).Select(a => new LineChartData
                            {
                                xValue = a.Key,
                                yValue = a.Count(),
                                yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.Date.ToString("dddd") == a.Key).Count(),
                            }).ToList();
                        }
                        else
                        {
                            Groubing = SelectedDateStatistic.Where(x => x.ActionName == ActionName && x.ActionType == ActionType.View).GroupBy(w => w.TransDate.Date.ToString("MMMM, dd")).OrderBy(a => a.Key).Select(a => new LineChartData
                            {
                                xValue = a.Key,
                                yValue = a.Count(),
                                yValue1 = LastDateStatistic.Where(f => f.ActionName == ActionName && f.TransDate.Date.Day == DateTime.Parse(a.Key).Day).Count(),
                            }).ToList();
                        }
                        break;
                }
                var maxvalue = model.MaxValue = GetChartMaxY(ActionName);
                model.ChartData = Groubing;
                model.Intervalvalue = GetintervalValue(maxvalue);
                model.ChartId = ActionName;
                if (ActionName == "GetRealEstateByIdForView")
                    model.ChartTitle = "global.realestate";
                else
                    model.ChartTitle = "global.Project";

                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(MostPropertiesVisitingCount), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<LatestPropertiesDTO> LatestPropertiesAdded(LogTransType actionType)
        {
            try
            {



                List<LatestPropertiesListDTO> dataSource = new List<LatestPropertiesListDTO>();
                var model = _context.ActivityLogs.AsQueryable();

                string whoAmI = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value;

                switch (whoAmI)
                {
                    case nameof(UserType.Companies):
                        var compId = int.Parse(_workContext.GetMyCompanyId());
                        model = model.Where(x => x.UserType == UserType.Companies && x.RelatedTo == compId);
                        break;
                }

                switch (actionType)
                {
                    case LogTransType.Projects:
                        dataSource = await model.Where(x => x.TransType == LogTransType.Projects && x.ActionType == ActionType.Add).Select(a => new LatestPropertiesListDTO
                        {
                            Id = int.Parse(a.TransReferenceId),
                            Date = a.TransDate.Date,
                            Link = "/Admin/Projects/ProjectUpdate/" + a.TransReferenceId,
                        }).OrderByDescending(x => x.Date).Take(10).ToListAsync();
                        break;
                    case LogTransType.RealEstates:
                        dataSource = await model.Where(x => x.TransType == LogTransType.RealEstates && x.ActionType == ActionType.Add).Select(a => new LatestPropertiesListDTO
                        {
                            Id = int.Parse(a.TransReferenceId),
                            Date = a.TransDate.Date,
                            Link = "/Admin/RealEstates/RealEstateUpdate/" + a.TransReferenceId,
                        }).OrderByDescending(x => x.Date).Take(10).ToListAsync();
                        break;
                }

                foreach (var item in dataSource)
                {
                    item.Name = GetPrapirtyName(item.Id.ToString(), actionType);
                }

                var response = new LatestPropertiesDTO()
                {
                    GridId = actionType.ToString(),
                    dataSource = dataSource,
                };
                return response;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(LatestPropertiesAdded), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        private async Task<MostCustomersActiveDTO> GetCustomerInfoAsync(string id)
        {
            try
            {

                var customer = await _context.Customers.Where(x => x.Id == int.Parse(id)).Select(x => new MostCustomersActiveDTO
                {

                    CustomerName = x.FullName,
                    CustomerPhone = x.PhoneNumber,

                }).FirstOrDefaultAsync();

                return customer;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCustomerInfoAsync), ex);
                return null;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<List<MostCustomersActiveDTO>> MostActiveCustomer()
        {
            try
            {

                var model = _context.ActivityLogs.AsQueryable();

                string whoAmI = _httpContextAccessor.HttpContext.User.FindFirst(PalClaimType.UserType.ToString()).Value;

                switch (whoAmI)
                {
                    case nameof(UserType.Companies):
                        var compId = int.Parse(_workContext.GetMyCompanyId());
                        model = model.Where(x => x.RelatedTo == compId);
                        break;
                }

                var usersGroubing = await _context.ActivityLogs.Where(x => x.UserType == UserType.Customers).GroupBy(x => x.UserTypeReferenceId).Select(z => new MostCustomersActiveDTO
                {
                    CustomerId = z.Key,
                    ActionsCount = model.Where(x => x.UserTypeReferenceId == z.Key).Count(),
                    Link = "/admin/Customers/CustomerUpdate/" + z.Key,
                }).ToListAsync();

                //foreach (var item in usersGroubing)
                //{
                //    var customerIfon = await GetCustomerInfoAsync(item.CustomerId);
                //    item.CustomerName = customerIfon.CustomerName;
                //    item.CustomerPhone = customerIfon.CustomerPhone;
                //}
                return usersGroubing;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(MostActiveCustomer), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------------------------------------- 
        public async Task<BubbleChartDTO> GetDealStatistics()
        {
            try
            {
                var Deals = await _context.Deals.Where(x=>!x.IsDeleted).Select(x => new BubbleChartDataDTO
                {
                    size = Convert.ToDouble(x.Amount),
                    x = x.SuccessProbability,
                    y = Convert.ToDouble(x.Amount),
                    text = x.DealName,

                }).ToListAsync();
                var result = new BubbleChartDTO {Data = Deals, MaxValue = Deals.Max(a=>a.y + 1000), Step = Deals.Max(a=>a.y / 8) };
               

                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetDealStatistics), ex);
                return null;
            }
        }

        //Task<List<PyramidDataDTO>> IStatisticsService.MostAgentsActive()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
