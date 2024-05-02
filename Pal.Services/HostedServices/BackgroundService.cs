using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pal.Core.Enums.Account;
using Pal.Data.Contexts;
using Pal.Services.DataServices.Customers;
using Pal.Services.DataServices.Lookups;

namespace Pal.Services.HostedServices
{
    public class PalBackgroundService_1Day : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PalBackgroundService_1Day(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<ILookupsService>();
                    var ssss = await customerService.GetSysCity();

                    Console.WriteLine(string.Format("{0} - {1}", ssss.Count(), DateTime.UtcNow.ToString("HH:mm:ss")));
                    await Task.Delay(new TimeSpan(24, 0, 0));
                }
            }
        }

    }

    //----------------------------- ---------------------------------- ------------------------------//
    public class PalBackgroundService_1Hour : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PalBackgroundService_1Hour(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<ILookupsService>();
                    var ssss = await customerService.GetSysCity();

                    Console.WriteLine(string.Format("{0} - {1}", ssss.Count(), DateTime.UtcNow.ToString("HH:mm:ss")));
                    await Task.Delay(new TimeSpan(1, 0, 0));//every 1 hours implement
                }
            }
        }

    }

    public class PalBackgroundService_5Minutes : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PalBackgroundService_5Minutes(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    int checkUpdate = 0;
                    //var reelService = scope.ServiceProvider.GetRequiredService<ReelService>();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    //var reels = await reelService.GetAllReelsByConditionAsync(r => r.ReelStatus == ReelStatus.Accepted);
                    //foreach (var reel in reels)
                    //{
                    //    reel.EndDate = reel.AcceptedDate.AddDays(1);
                    //    checkUpdate++;
                    //    if (reel.EndDate < DateTime.Now)
                    //    {
                    //        reel.IsExpired = true;
                    //    }
                    //    context.Update(reel);





                    //}
                    if (checkUpdate > 0)
                        await context.SaveChangesAsync();

                    //var startingReels = await reelService.GetAllReelsByConditionAsync(r => r.ReelStatus == ReelStatus.Starting);
                    //foreach (var reel in reels)
                    //{
                    //    startingCheck = 0;
                    //    if (reel.EndDate < DateTime.Now)
                    //    {
                    //        //reel.ReelStatus = ReelStatus.Ending;
                    //        startingCheck++;
                    //    }
                    //    if (startingCheck != 0)
                    //    {
                    //        checkUpdate = 1;
                    //        context.Update(reel);
                    //    }
                    //}
                    //if (checkUpdate != 0)
                    //    await context.SaveChangesAsync();
                    await Task.Delay(new TimeSpan(0, 0, 1));//every 5 minutes implement
                }
            }
        }

    }
}
