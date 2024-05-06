using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;

using Pal.Core.Domains.Account;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Attachments;
using Pal.Core.Domains.Chat;
using Pal.Core.Domains.Communications;
using Pal.Core.Domains.Companies;
using Pal.Core.Domains.Config;
using Pal.Core.Domains.Customers;
using Pal.Core.Domains.Deals;
using Pal.Core.Domains.Empolyees;
using Pal.Core.Domains.GenericColumns;
using Pal.Core.Domains.Languages;
using Pal.Core.Domains.Lookups;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Core.Domains.Notifications;
using Pal.Core.Domains.Required;
using Pal.Core.Domains.Sales;
using Pal.Core.Domains.Sms;
using Pal.Core.Domains.SystemErrors;
using Pal.Core.Domains.Tasks;

//using Task = Pal.Core.Domains.Tasks.Task;

namespace Pal.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>, IDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Attachment> Attachments { get; set; }

        #region GeneralColumns
        public DbSet<EditableTable> EditableTables { get; set; }
        public DbSet<ColumnDetail> ColumnDetails { get; set; }
        #endregion

        #region System

        public DbSet<SysCity> SysCities { get; set; }
        public DbSet<SysCityTranslate> SysCityTranslates { get; set; }
        public DbSet<SysCountry> SysCountries { get; set; }
        public DbSet<SysCountryTranslate> SysCountryTranslates { get; set; }

        public DbSet<SysCompanyCategory> SysCompanyCategories { get; set; }
        public DbSet<SysSalesCategory> SysSalesCategories { get; set; }
        public DbSet<SysCompanyCategoryTranslate> SysCompanyCategoryTranslates { get; set; }
        public DbSet<SysSalesCategoryTranslate> SysSalesCategoryTranslates { get; set; }
        public DbSet<SysNationality> SysNationalities { get; set; }
        public DbSet<SysNationalityTranslate> SysNationalityTranslates { get; set; }
        public DbSet<SysNeighborhood> SysNeighborhoods { get; set; }
        public DbSet<SysNeighborhoodTranslate> SysNeighborhoodTranslates { get; set; }
        public DbSet<SysPaymentType> SysPaymentTypes { get; set; }
        public DbSet<SysPaymentTypeTranslate> SysPaymentTypeTranslates { get; set; }

        public DbSet<SysViewType> SysViewTypes { get; set; }
        public DbSet<SysViewTypeTranslate> SysViewTypeTranslates { get; set; }

        public DbSet<SysRegion> SysRegions { get; set; }
        //public DbSet<SysRegionSite> SysRegionSites { get; set; }
        public DbSet<SysRegionTranslate> SysRegionTranslates { get; set; }
     
        public DbSet<Language> Languages { get; set; }
        public DbSet<RequiredFields> RequiredFields { get; set; }


        #endregion

        #region Customer
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerFavorite> CustomerFavorites { get; set; }
        public DbSet<CustomerNotes> CustomerNotes { get; set; }
        public DbSet<CustomerSavedSearch> CustomerSavedSearches { get; set; }
        #endregion

        #region Chat
        public DbSet<ChatInbox> ChatInbox { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        #endregion

        #region Employee
        public DbSet<Employee> Employees { get; set; }
        #endregion

        #region Companies
        public DbSet<Company> Companies { get; set; }

        #endregion
        #region Sales
        public DbSet<Sales> sales { get; set; }
        #endregion
        #region CRM System
        public DbSet<Pal.Core.Domains.Tasks.Task> Tasks { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Call> Calls { get; set; }
        public DbSet<Deal> Deals { get; set; }

        #region CRM System LookUps
        public DbSet<SysCallPurpose> SysCallPurposes { get; set; }
        public DbSet<SysCallPurposeTranslate> SysCallPurposeTranslates { get; set; }

        public DbSet<SysDecision> SysDecisions { get; set; }
        public DbSet<SysDecisionTranslate> SysDecisionTranslates { get; set; }

        public DbSet<SysCallResult> SysCallResults { get; set; }
        public DbSet<SysCallResultTranslate> SysCallResultTranslates { get; set; }

        public DbSet<SysCallStatus> SysCallStatus { get; set; }
        public DbSet<SysCallStatusTranslate> SysCallStatusTranslates { get; set; }

        public DbSet<SysCallType> SysCallTypes { get; set; }
        public DbSet<SysCallTypeTranslate> SysCallTypeTranslates { get; set; }

        public DbSet<SysDealStage> SysDealStages { get; set; }
        public DbSet<SysDealStageTranslate> SysDealStageTranslates { get; set; }

        public DbSet<SysDealType> SysDealTypes { get; set; }
        public DbSet<SysDealTypeTranslate> SysDealTypeTranslates { get; set; }

        public DbSet<SysLeadRating> SysLeadRatings { get; set; }
        public DbSet<SysLeadRatingTranslate> SysLeadRatingTranslates { get; set; }

        public DbSet<SysLeadSource> SysLeadSources { get; set; }
        public DbSet<SysLeadSourceTranslate> SysLeadSourceTranslates { get; set; }

        public DbSet<SysLeadStatus> SysLeadStatus { get; set; }
        public DbSet<SysLeadStatusTranslate> SysLeadStatusTranslates { get; set; }

        public DbSet<SysTaskStatus> SysTaskStatus { get; set; }
        public DbSet<SysTaskStatusTranslate> SysTaskStatusTranslates { get; set; }

        public DbSet<SysJobTitle> SysJobTitles { get; set; }
        public DbSet<SysJobTitleTranslate> SysJobTitleTranslates { get; set; }

        #endregion




        #endregion

        #region Localization
        public DbSet<LanguageStringResource> LanguageStringResources { get; set; }
        #endregion

        #region Website Configurations
        public DbSet<WebsiteConfiguration> WebsiteConfigurations { get; set; }
        #endregion

        #region Notifications
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationParam> NotificationParams { get; set; }
        public DbSet<NotificationTypeTranslate> NotificationTypeTranslates { get; set; }
        #endregion

        #region Sms
        public DbSet<SmsMsg> SmsMsgs { get; set; }
        public DbSet<SmsTemplate> SmsTemplates { get; set; }
        public DbSet<SmsTemplateTranslate> SmsTemplateTranslates { get; set; }
        #endregion

        #region ActivityLog
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        #endregion

        #region System Errors
        public DbSet<SystemError> SystemErrors { get; set; }

        #endregion

        public DbSet<AspNetPhoneVerificationToken> PhoneVerificationTokens { get; set; }

        public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();

        public bool IsEnable_SoftDelete()
        {
            return false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Install from NuGet Microsoft.Extensions.Configuration.Json to be able to AddJsonFile
                //var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
                optionsBuilder
                    //https://docs.microsoft.com/en-us/ef/core/querying/related-data/#lazy-loading
                    //.UseLazyLoadingProxies()
                    .UseSqlServer(ConnectionStrings.AppConnectionString, o =>
                    {
                        //o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        //o.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    });
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);





            //DbInitializer.Seed(builder);
        }

    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(ConnectionStrings.AppConnectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}