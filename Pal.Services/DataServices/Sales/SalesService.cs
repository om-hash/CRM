using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Pal.Core.Domains.Account;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Sales;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Approvment;
using Pal.Core.Enums.Attachment;
using Pal.Core.Enums.Email;
using Pal.Core.Enums.Notifications;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.Sales;
using Pal.Data.DTOs.Lookups;
using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using Pal.Data.VMs.Sales;
using Pal.Data.VMs.Pagination;
using Pal.Services.Caching;
//using Pal.Services.DataServices.Projects;
//using Pal.Services.DataServices.RatingAndComments;
//using Pal.Services.DataServices.RealEstates;
using Pal.Services.Email;
using Pal.Services.FileManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Services.WebWorkContext;

using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.Sales
{
    public class SalesService : BaseService<SalesService>, ISalesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly IFileManagerService _fileManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _loggerService;
        private readonly INotificationService _notificationService;
        //private readonly IRealEstatesService _realEstatesService;
        //private readonly IProjectsService _projectsService;
        private readonly ILoggerService _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ICacheService<List<LookupsCachingModel>> _cacheService;
        //private readonly IRatingAndCommentsService _ratingAndCommentsService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        //-----------------------------------------------------------------------------------
        public SalesService(ApplicationDbContext context, IWebWorkContext workContext, UserManager<ApplicationUser> userManager, IFileManagerService fileManager, IEmailService emailService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService loggerService, INotificationService notificationService,
            /*IRealEstatesService realEstatesService, IProjectsService projectsService,*/ ILanguageService languageService, /*IRatingAndCommentsService ratingAndCommentsService,*/
            ILoggerService logger, ICacheService<List<LookupsCachingModel>> cacheService, ILocalizationService localizationService) : base(context, logger)
        {
            _context = context;
            _userManager = userManager;
            _workContext = workContext;
            _fileManager = fileManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggerService = loggerService;
            _notificationService = notificationService;
            _logger = logger;
            _emailService = emailService;
            _cacheService = cacheService;
            _languageService = languageService;
            _localizationService = localizationService;
        }


        #region Admin Transactions
        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> CreateNewSales(CreateNewSalesDTO model)
        {
            try
            {
                var requiredFields = await CheckRequiredFields(model);
                if (requiredFields != null) if (requiredFields.Any())
                    return Error(requiredFields, System.Net.HttpStatusCode.BadRequest);

                _cacheService.Delete("GetSalesAsLookupCacheKey");
                var SalesTranslate = new List<SalesTranslate>();

                if (model.SalesTranslates != null)
                {
                    foreach (var Salestrans in model.SalesTranslates)
                    {
                        SalesTranslate.Add(new SalesTranslate
                        {
                            CompId = 0,
                            SalesName = Salestrans.SalesName,
                            LanguageId = Salestrans.LanguageId,
                            Content = Salestrans.Content,
                        });
                    }
                }
                var sales = new Sales
                {
                    CountryId = model.CountryId,
                    CityId = model.CityId,
                    FullAddress = model.FullAddress,
                    Email = model.Email,
                    Phone = model.Phone,
                    WhatsApp = model.WhatsApp,
                    Website = model.Website,
                    ManagerEmail = model.ManagerEmail,
                    ManagerName = model.ManagerName,
                    ManagedBy = "", //TODO
                    TaxNumber = model.TaxNumber,
                    SalesTranslates = SalesTranslate,
                    SalesCategoryId = model.SalesCategoryId,
                    FacebookURL = model.FacebookURL,
                    InstagramURL = model.InstagramURL,
                    linkedinURL = model.linkedinURL,
                    TwitterURL = model.TwitterURL,
                };

                _context.sales.Add(sales);
                await _context.SaveChangesAsync();

                sales.MainImgUrl = await _fileManager.UploadFileAsync(model.MainImageFile, ReferenceType.sales, true, MediaType.Photos, Sales.Id.ToString());
                sales.SalesLogo = await _fileManager.UploadFileAsync(model.SalesLogoFile, ReferenceType.sales, true, MediaType.Photos, Sales.Id.ToString());

                _context.Update(sales);
                await _context.SaveChangesAsync();

                return Success(sales.Id);
            }

            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> DeleteSales(int id)
        {
            try
            {
                _cacheService.Delete("GetSalesAsLookupCacheKey");

                var Sales = await _context.sales.FirstOrDefaultAsync(x => x.Id == id);

                Sales.IsDeleted = true;

                _context.sales.Update(Sales);

                await _context.SaveChangesAsync();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.ReferenceId == Sales.Id.ToString() && u.UserType == UserType.Sales);

                user.IsDeleted = true;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteSales), ActionType = ActionType.Delete, TransReferenceId = Sales.Id.ToString(), TransType = LogTransType.Sales });
                return Success();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteSales), ex);
                return Error(ex);
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<SyncPaginatedListModel<SalesListDTO>> GetAllAsListAsync(MyDataManagerRequest dm)
        {
            try
            {
                var WorkingLanguage = await _languageService.GetLanguageIdFromRequestAsync();
                var query = _context.sales.Where(a => a.IsDeleted == dm.ShowDeleted).AsQueryable().AsNoTracking()
                    .Select(x => new SalesListDTO
                    {
                        Id = x.Id,
                        Phone = x.Phone,
                        WhatsApp = x.WhatsApp,
                        Email = x.Email,
                        ManagedBy = "",
                        Website = x.Website,
                        SalesName = x.SalesTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).SalesName,
                        MainImgUrl = x.MainImgUrl,
                        SalesLogo = x.SalesLogo,
                        ManagerName = x.ManagerName,
                        ManagerEmail = x.ManagerEmail,
                        FullAddress = x.FullAddress,
                        CountryName = _context.SysCountryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CountryName,
                        CityName = x.SysCity.SysCityTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CityName,
                        CategoryName = x.SysSalesCategory.SysSalesCategoryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).SalesCategoryName,
                        Password = "*",
                        IsDeleted = x.IsDeleted,
                        //CreateDate = x.CreationDate,
                    });

                return await SyncGridOperations<SalesListDTO>.PagingAndFilterAsync(query, dm);
            }
            catch (Exception)
            {
                return null;
            }   
        }
        //-----------------------------------------------------------------------------------
        public async Task<SyncPaginatedListModel<SalesListDTO>> GetAllAsListToEditAsync(DataManagerRequest dm)
        {
            var WorkingLanguage = await _languageService.GetLanguageIdFromRequestAsync();
            var query = _context.sales.Where(a => !a.IsDeleted).AsQueryable().AsNoTracking()
                .Select(x => new SalesListDTO
                {
                    Id = x.Id,
                    Phone = x.Phone,
                    WhatsApp = x.WhatsApp,
                    Email = x.Email,
                    ManagedBy = "",
                    Website = x.Website,
                    SalesName = x.SalesTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).SalesName,
                    MainImgUrl = x.MainImgUrl,
                    SalesLogo = x.salesLogo,
                    ManagerName = x.ManagerName,
                    ManagerEmail = x.ManagerEmail,
                    FullAddress = x.FullAddress,
                    //CityId = x.CityId,
                    //CountryId = x.CountryId,
                    CountryName = _context.SysCountryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CountryName,
                    CityName = x.SysCity.SysCityTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CityName,
                    Categoryid = x.SalesCategoryId ?? 0,
                    CategoryName = x.SysSalesCategory.SysSalesCategoryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).SalesCategoryName,
                    Password = "*",
                    FacebookURL = x.FacebookURL,
                    InstagramURL = x.InstagramURL,
                    linkedinURL = x.linkedinURL,
                    TwitterURL = x.TwitterURL,
                    UserName = _context.Users.FirstOrDefault(a => a.UserType == UserType.Sales && a.ReferenceId == x.Id.ToString()).UserName,
                    //SalesTranslatesss = x.SalesTranslates.Select(a => new CreateNewSalesTranslateDTO
                    //{
                    //    SalesId = x.Id,
                    //    SalesName = a.SalesName,
                    //    LanguageId = a.LanguageId,
                    //    Id = a.Id,
                    //}).AsQueryable()
                });

            return await SyncGridOperations<SalesListDTO>.PagingAndFilterAsync(query, dm);

        }


        //-----------------------------------------------------------------------------------
        public async Task<CreateNewSalesDTO> GetByIdAsync(int id)
        {
            try
            {
                var Languages = _languageService.GetAllLanguages();
                var sales = await _context.sales.Where(x => x.Id == id).Include(x => x.SalesTranslates).FirstOrDefaultAsync();
                var user = await _context.Users.Where(a => a.ReferenceId == sales.Id.ToString() && a.UserType == UserType.Sales).FirstOrDefaultAsync();
                foreach (var lang in Languages)
                {
                    var IsExist = sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == lang.Id && x.CompId == sales.Id);
                    if (IsExist == null)
                    {
                        var NewCopanyTranslate = new SalesTranslate()
                        {
                            LanguageId = lang.Id,
                            CompId = sales.Id,
                            SalesName = "",
                        };
                        sales.SalesTranslates.Add(NewCopanyTranslate);
                        await _context.SaveChangesAsync();
                    }
                }
                var model = _mapper.Map<CreateNewSalesDTO>(sales);
                //model.UserId = user.Id;
                //model.OwnerName = user.FullName;
                model.Attachments = await _context.Attachments.Where(x => x.ReferenceType == ReferenceType.Sales && x.ReferenceId == id.ToString()).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetByIdAsync), ex);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> UpdateSales(CreateNewSalesDTO createNewSalesDTO)
        {
            try
            {
                var requiredFields = await CheckRequiredFields(createNewSalesDTO);
                if (requiredFields != null) if (requiredFields.Any())
                    return Error(requiredFields, System.Net.HttpStatusCode.BadRequest);

                _cacheService.Delete("GetSalesAsLookupCacheKey");
                var model = _mapper.Map<sales>(createNewSalesDTO);
                if (createNewSalesDTO.MainImageFile != null)
                {
                    model.MainImgUrl = await _fileManager.UploadFileAsync(createNewSalesDTO.MainImageFile,
                        ReferenceType.Sales, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewSalesDTO.MainImgUrl);
                }
                if (createNewSalesDTO.SalesLogoFile != null)
                {
                    model.SalesLogo = await _fileManager.UploadFileAsync(createNewSalesDTO.SalesLogoFile,
                    ReferenceType.Sales, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewSalesDTO.SalesLogo);
                }
                if (createNewSalesDTO.TaxDocumentImageFile != null)
                {
                    model.TaxDocumentImage = await _fileManager.UploadFileAsync(createNewSalesDTO.TaxDocumentImageFile,
                    ReferenceType.Sales, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewSalesDTO.TaxDocumentImage);
                }
                if (createNewSalesDTO.ManagerSignatureImageFile != null)
                {
                    model.ManagerSignatureImage = await _fileManager.UploadFileAsync(createNewSalesDTO.ManagerSignatureImageFile,
                    ReferenceType.Sales, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewSalesDTO.ManagerSignatureImage);
                }
                if (createNewSalesDTO.AttachmentsFiles != null)
                {
                    _context.Attachments.AddRange(await _fileManager.UploadFilesAsync(createNewSalesDTO.AttachmentsFiles, ReferenceType.Sales, true, MediaType.Photos, model.Id.ToString()));
                }
                _context.Update(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateSales), ActionType = ActionType.Update, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Sales });
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task SalesApprove(int id)
        {
            try
            {
                var Sales = await _context.sales.Where(x => x.Id == id).Include(x => x.SalesTranslates).FirstOrDefaultAsync();
                Sales.ApproveState = ApproveStatment.Approved;
                Sales.ApprovedBy = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Sales.ApprovedDate = DateTime.Now;
                _context.Update(Sales);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(SalesApprove), ActionType = ActionType.Approve, TransReferenceId = Sales.Id.ToString(), TransType = LogTransType.Sales });
                #region SendEmail
                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", Sales.Email, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName,
                    "https://www.insaattan.com/account/login", EmailType.SalesApprovement, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName);
                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", Sales.ManagerEmail, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName,
                   "https://www.insaattan.com/account/login", EmailType.SalesApprovement, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName);
                #endregion
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SalesApprove), ex);
            }

        }

        //-----------------------------------------------------------------------------------
        public async Task SalesDisApprove(int id)
        {
            try
            {
                var Sales = await _context.sales.Where(x => x.Id == id).FirstOrDefaultAsync();
                Sales.ApproveState = ApproveStatment.DisApproved;
                _context.Update(Sales);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(SalesDisApprove), ActionType = ActionType.DisApprove, TransReferenceId = Sales.Id.ToString(), TransType = LogTransType.Sales });

                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", Sales.Email, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName,
                   "", EmailType.SalesDisApprovement, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName);
                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", Sales.ManagerEmail, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName,
                   "", EmailType.SalesDisApprovement, Sales.SalesTranslates.FirstOrDefault(x => x.LanguageId == 1).SalesName);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SalesDisApprove), ex);
            }

        }

        //-----------------------------------------------------------------------------------
        public async Task<SalesViewModel> GetSalesByIdForView(int id, int? LangId = null)
        {
            try
            {
                LangId = LangId ?? await _languageService.GetLanguageIdFromRequestAsync();

                SalesViewModel Sales = await _context.sales.Where(c => !c.IsDeleted).Select(x => new SalesViewModel
                {
                    Id = x.Id,
                    SalesName = x.SalesTranslates.FirstOrDefault(x => x.LanguageId == LangId).SalesName,
                    MainImg = x.MainImgUrl,
                    Email = x.Email,
                    Logo = x.SalesLogo,
                    PhoneNumber = x.Phone,
                    Website = x.Website,
                    WhatsApp = x.WhatsApp,
                    //AvrageRating = x.AvrageRating,
                    TwitterURL = x.TwitterURL,
                    linkedinURL = x.linkedinURL,
                    InstagramURL = x.InstagramURL,
                    FacebookURL = x.FacebookURL,
                    SalesCategoryName = x.SysSalesCategory.SysSalesCategoryTranslates.FirstOrDefault(x => x.LanguageId == LangId).SalesCategoryName,
                    Content = x.SalesTranslates.FirstOrDefault(x => x.LanguageId == LangId).Content
                }).FirstOrDefaultAsync(c => c.Id == id);

                //Sales.AvrageRating = await _ratingAndCommentsService.GetAvg(ReferenceType.Sales, id.ToString());

                return Sales;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSalesByIdForView), ex);
                return null;
            }
        }

        public async Task<SalesViewModel> GetSalesDetailsByIdForView(int id)
        {
            try
            {
                var LangId = await _languageService.GetLanguageIdFromRequestAsync();
                var Sales = await _context.sales.Where(c => !c.IsDeleted).Select(x => new SalesViewModel
                {
                    Id = x.Id,
                    SalesName = x.SalesTranslates.FirstOrDefault(x => x.LanguageId == LangId).SalesName,
                    Email = x.Email,
                    Logo = x.SalesLogo,
                    MainImg = x.MainImgUrl,
                    PhoneNumber = x.Phone,
                    Website = x.Website,
                    WhatsApp = x.WhatsApp,
                    Content = x.SalesTranslates.FirstOrDefault(ct => ct.LanguageId == LangId).Content ?? "",
                    //AvrageRating = x.AvrageRating,
                    Address = x.FullAddress + " - " + x.SysCity.SysCityTranslates.FirstOrDefault(city => city.LanguageId == LangId).CityName
                                + " - " + x.SysCity.SysCountry.SysCountryTranslates.FirstOrDefault(country => country.LanguageId == LangId).CountryName,
                }).FirstOrDefaultAsync(c => c.Id == id);

                //Sales.Projects = PaginatedList<ProjectCardViewModel>.Create(_projectsService.GetProjectsBySalesAsync(id).Result.AsQueryable(), 1, 6);

                //Sales.RealEstates = PaginatedList<RealEstateCardViewModel>.Create(_realEstatesService.GetRealEstatesBySalesId(id).Result.AsQueryable(), 1, 6);

                return Sales;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSalesDetailsByIdForView), ex);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<bool> IsSalesVerified(int compId)
        {
            try
            {
                return (await _context.sales.FindAsync(compId)).ApproveState == ApproveStatment.Approved;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(IsSalesVerified), ex);
                return false;
            }
        }

        //----------------------------------------------------------------------------------- 
        public async Task<string> GetSalesName(int compId)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                return await _context.sales.Where(a => a.Id == compId).Select(a => a.SalesTranslates.FirstOrDefault(a => a.LanguageId == langId).SalesName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(GetSalesName), ex);
                return "Sales" + compId;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<bool> CheckIfFirstTime(int id)
        {
            try
            {
                var model = await _context.sales.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (model.IsTearmAndConditionsAccepted)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(CheckIfFirstTime), ex);
                throw;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<bool> AcceptTearmAndConditions(int id)
        {
            try
            {
                var model = await _context.sales.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (model != null)
                {
                    model.IsTearmAndConditionsAccepted = true;
                    _context.sales.Update(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return true;

            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(AcceptTearmAndConditions), ex);
                return false;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> DeletePermenantly(int id)
        {
            using var transaction = _context.BeginTransaction();

            try
            {
                _cacheService.Delete("GetSalesAsLookupCacheKey");
                //if (await _context.Projects.AnyAsync(a => !a.IsDeleted && a.CompId == id))
                //    return Error("Cannot delete Sales that has projects");

                var comp = await _context.sales.FirstOrDefaultAsync(a => a.Id == id);

                // Deleting Project Files
                _ = _fileManager.DeleteFileAsync(comp.MainImgUrl);
                var attachments = await _context.Attachments.Where(a =>
                a.ReferenceType == ReferenceType.Sales &&
                a.ReferenceId == id.ToString()).ToListAsync();
                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        _ = _fileManager.DeleteFileAsync(attachment.FileName);
                        _context.Attachments.Remove(attachment);
                    }
                }


                _context.sales.Remove(comp);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();



                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeletePermenantly), ActionType = ActionType.DeletePermenantly, TransReferenceId = comp.Id.ToString(), TransType = LogTransType.Sales });
                return Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _ = _logger.LogErrorAsync(nameof(DeletePermenantly), ex);
                return Error(ex);
            }

        }

        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> Restore(int id)
        {
            try
            {
                var comp = await _context.sales.FirstOrDefaultAsync(a => a.Id == id);

                comp.IsDeleted = false;
                _context.sales.Update(comp);
                await _context.SaveChangesAsync();


                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(Restore), ActionType = ActionType.Restore, TransReferenceId = comp.Id.ToString(), TransType = LogTransType.Sales });
                return Success();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Restore), ex);
                return Error(ex);
            }
        }

        //============//============//============//============//============
        private async Task<List<string>> CheckRequiredFields(CreateNewSalesDTO model)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var requiredFields = await _context.RequiredFields.FirstOrDefaultAsync(a => a.ReferenceType == "Sales");
                if (requiredFields == null)
                    return null;

                if (string.IsNullOrEmpty(requiredFields.Required))
                    return null;

                List<string> validations = new();
                requiredFields.Required.Split(",").ForEach(requiredPropName =>
                {
                    if (typeof(CreateNewSalesDTO).GetProperties().Any(a => a.Name == requiredPropName))
                    {
                        var val = model.GetType().GetProperty(requiredPropName)?.GetValue(model)?.ToString();
                        if (string.IsNullOrEmpty(val))
                        {
                            validations.Add(_localizationService.GetStringResource("model.Sales." + requiredPropName, langId));
                        }
                    }

                    if (typeof(CreateNewSalesTranslateDTO).GetProperties().Any(a => a.Name == requiredPropName))
                        model.SalesTranslates.ForEach(translate =>
                        {
                            var val = translate.GetType().GetProperty(requiredPropName).GetValue(translate)?.ToString();
                            if (string.IsNullOrEmpty(val))
                            {
                                var msg = _localizationService.GetStringResource("model.Sales." + requiredPropName, langId);
                                if (!validations.Any(a => a == msg))
                                    validations.Add(msg);
                            }
                        });

                });

                return validations;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion


        #region Sales Transactions
        //-----------------------------------------------------------------------------------
        public async Task<bool> UpdateMySales(CreateNewSalesDTO model)
        {
            try
            {
                var Sales = await _context.sales.Include(a => a.SalesTranslates).FirstOrDefaultAsync(a => a.Id == model.Id);
                if (Sales == null)
                    return false;

                Sales.SalesTranslates = _mapper.Map<List<SalesTranslate>>(model.SalesTranslates);
                Sales.CityId = model.CityId;
                Sales.CountryId = model.CountryId;
                Sales.FullAddress = model.FullAddress;
                Sales.Phone = model.Phone;
                Sales.WhatsApp = model.WhatsApp;
                Sales.Email = model.Email;
                Sales.Website = model.Website;
                Sales.ManagerName = model.ManagerName;
                Sales.ManagerEmail = model.ManagerEmail;


                if (model.MainImageFile != null)
                {
                    Sales.MainImgUrl = await _fileManager.UploadFileAsync(model.MainImageFile,
                        ReferenceType.Sales, true, MediaType.Photos, Sales.Id.ToString());

                    _ = _fileManager.DeleteFileAsync(model.MainImgUrl);

                }
                if (model.SalesLogoFile != null)
                {
                    Sales.SalesLogo = await _fileManager.UploadFileAsync(model.SalesLogoFile,
                    ReferenceType.Sales, true, MediaType.Photos, Sales.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(model.SalesLogo);
                }
                _context.Update(Sales);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateMySales), ActionType = ActionType.Update, TransReferenceId = Sales.Id.ToString(), TransType = LogTransType.Sales });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateMySales), ex);
                return false;
            }
        }
        public async Task<int> GetSalesIdByUserId(string userId)
        {
            try
            {
                int SalesId = int.Parse((await _context.Users.FirstOrDefaultAsync(e => e.Id == userId))?.ReferenceId);

                return SalesId;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSalesIdByUserId), ex);
                return 0;
            }
        }
        #endregion


    }
}
