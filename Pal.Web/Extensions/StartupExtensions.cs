using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pal.Core.Domains.Account;
using Pal.Data.Contexts;
using Pal.Services.Caching;
using Pal.Services.Configurations;
using Pal.Services.CRM.Calls;
using Pal.Services.CRM.Deals;
using Pal.Services.CRM.Meetings;
using Pal.Services.CRM.Tasks;
using Pal.Services.DataServices.Chat;
using Pal.Services.DataServices.Companies;
using Pal.Services.DataServices.Customers;
using Pal.Services.DataServices.Employees;
using Pal.Services.DataServices.GenericColumn;
using Pal.Services.DataServices.LanguegService;
using Pal.Services.DataServices.Lookups;
using Pal.Services.DataServices.LookupsCRUDService;
using Pal.Services.DataServices.Sales;
using Pal.Services.DataServices.statistics;
using Pal.Services.Email;
using Pal.Services.ExcelManager;
using Pal.Services.FileManager;
using Pal.Services.HostedServices;
using Pal.Services.Languages;
using Pal.Services.Sms;

using Pal.Services.WebWorkContext;

namespace Pal.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void AddMyContexts(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
        

            services.AddScoped<IDbContext, ApplicationDbContext>();
   
        }

        //---------------------------------------------------------------------------
        public static void AddMyIdentity(this IServiceCollection services/*, IConfiguration Configuration*/)
        {
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Admin/Dashboard/AccessDenied");
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
            });
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = IdentityConstants.ApplicationScheme;
                option.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    //ValidAudience = Configuration["JWT:ValidAudience"],
                    //ValidIssuer = Configuration["JWT:ValidIssuer"],
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
            });

        }

        //---------------------------------------------------------------------------
        public static void AddMyLocalization(this IServiceCollection services)
        {
            services.AddScoped<ILanguageService, Services.Languages.LanguageService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddLocalization();

            services.AddControllersWithViews().AddViewLocalization();

            var serviceProvider = services.BuildServiceProvider();
            var languageService = serviceProvider.GetRequiredService<ILanguageService>();
            var languages = languageService.GetLanguages();
            var cultures = languages.Select(x => new CultureInfo(x.Culture)).ToArray();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var englishCulture = cultures.FirstOrDefault(x => x.Name == "en-US");
                options.DefaultRequestCulture = new RequestCulture(englishCulture?.Name ?? "en-US");

                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
        }

        //---------------------------------------------------------------------------
        public static void AddMyCacheService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));
        }

        //---------------------------------------------------------------------------
        public static void AddMyDbServices(this IServiceCollection services)
        {
            //services.AddScoped<IRegionService, RegionService>();

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ISalesService, SalesService>();

            services.AddScoped<ILookupsService, LookupsService>();

            services.AddScoped<ICustomersService, CustomersService>();

            services.AddScoped<ILanguagesService, Services.DataServices.LanguegService.LanguageService>();

            services.AddScoped<ILookupsCRUDService, LookupsCRUDService>();

            services.AddScoped<IChatService, ChatService>();

            services.AddScoped<IStatisticsService, StatisticsService>();

            services.AddScoped<IEmployeesService, EmployeesService>();

            services.AddScoped<IMeetingService, MeetingSerivce>();

            services.AddScoped<ICallService, CallSerivce>();

            services.AddScoped<IDealsService, DealsSerivce>();

            //services.AddScoped<IManageGenericColumns, ManageGenericColumns>();
        }

        //---------------------------------------------------------------------------
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IExcel, Excel>();
            services.AddScoped<IWebWorkContext, WebWorkContext>();
            services.AddScoped<IFileManagerService, FileManagerService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmsService, SmsService>();
            //services.AddScoped<SqlCmd>(); // add new columns to tables
            services.AddHostedService<PalBackgroundService_1Day>();
            services.AddHostedService<PalBackgroundService_1Hour>();
        }

        //---------------------------------------------------------------------------
        public static void AddMyActionFilters(this IServiceCollection services)
        {
            services.AddScoped<CheckCompanyVerifiedAttribute>();

        }

        //---------------------------------------------------------------------------
        public static void AddMyCRMServices(this IServiceCollection services)
        {
            services.AddScoped<ITasksService, TaskService>();
        }

        //public static void AddReelServices(this IServiceCollection services)
        //{
        //    //services.AddScoped<IReelService, ReelService>();
        //}
    }




}
