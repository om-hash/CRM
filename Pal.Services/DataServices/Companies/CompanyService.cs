using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Pal.Core.Domains.Account;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Companies;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Approvment;
using Pal.Core.Enums.Attachment;
using Pal.Core.Enums.Email;
using Pal.Core.Enums.Notifications;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.Company;
using Pal.Data.DTOs.Lookups;
using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using Pal.Data.VMs.Company;
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

namespace Pal.Services.DataServices.Companies
{
    public class CompanyService : BaseService<CompanyService>, ICompanyService
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
        public CompanyService(ApplicationDbContext context, IWebWorkContext workContext, UserManager<ApplicationUser> userManager, IFileManagerService fileManager, IEmailService emailService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService loggerService, INotificationService notificationService,
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
            //_realEstatesService = realEstatesService;
            //_projectsService = projectsService;
            _logger = logger;
            _emailService = emailService;
            _cacheService = cacheService;

            _languageService = languageService;
            //_ratingAndCommentsService = ratingAndCommentsService;
            _localizationService = localizationService;
        }


        #region Admin Transactions
        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> CreateNewCompany(CreateNewCompanyDTO model)
        {
            try
            {
                var requiredFields = await CheckRequiredFields(model);
                if (requiredFields != null) if (requiredFields.Any())
                    return Error(requiredFields, System.Net.HttpStatusCode.BadRequest);

                _cacheService.Delete("GetCompaniesAsLookupCacheKey");
                var CompanyTranslate = new List<CompanyTranslate>();

                if (model.CompanyTranslates != null)
                {
                    foreach (var companytrans in model.CompanyTranslates)
                    {
                        CompanyTranslate.Add(new CompanyTranslate
                        {
                            CompId = 0,
                            CompanyName = companytrans.CompanyName,
                            LanguageId = companytrans.LanguageId,
                            Content = companytrans.Content,
                        });
                    }
                }
                var company = new Company
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
                    CompanyTranslates = CompanyTranslate,
                    CompanyCategoryId = model.CompanyCategoryId,
                    FacebookURL = model.FacebookURL,
                    InstagramURL = model.InstagramURL,
                    linkedinURL = model.linkedinURL,
                    TwitterURL = model.TwitterURL,
                };

                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                company.MainImgUrl = await _fileManager.UploadFileAsync(model.MainImageFile, ReferenceType.Companies, true, MediaType.Photos, company.Id.ToString());
                company.ComapnyLogo = await _fileManager.UploadFileAsync(model.CompanyLogoFile, ReferenceType.Companies, true, MediaType.Photos, company.Id.ToString());

                _context.Update(company);
                await _context.SaveChangesAsync();

                //await _notificationService.SendNotificationAsync(new Data.DTOs.Notifications.NotificationDTO
                //{
                //    GroupId = UserType.Admins.ToString(),
                //    NotificationFor = UserType.Admins,
                //    NotificationTypeId = (int)NotificationTypes.ToAdmin_NewCompanyNeedsVerification,
                //    Url = "/Admin/Companies/CompanyUpdate/" + company.Id,
                //    NotificationParams = new List<Data.DTOs.Notifications.NotificationParamDTO>
                //    {
                //        new Data.DTOs.Notifications.NotificationParamDTO{ Param = company.CompanyTranslates.FirstOrDefault().CompanyName },
                //    },
                //});
                //await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CreateNewCompany), ActionType = ActionType.Add, TransReferenceId = company.Id.ToString(), TransType = LogTransType.Companies });
                
                return Success(company.Id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> DeleteCompany(int id)
        {
            try
            {
                _cacheService.Delete("GetCompaniesAsLookupCacheKey");

                var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);

                company.IsDeleted = true;

                _context.Companies.Update(company);

                await _context.SaveChangesAsync();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.ReferenceId == company.Id.ToString() && u.UserType == UserType.Companies);

                user.IsDeleted = true;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteCompany), ActionType = ActionType.Delete, TransReferenceId = company.Id.ToString(), TransType = LogTransType.Companies });
                return Success();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteCompany), ex);
                return Error(ex);
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<SyncPaginatedListModel<CompanyListDTO>> GetAllAsListAsync(MyDataManagerRequest dm)
        {
            try
            {
                var WorkingLanguage = await _languageService.GetLanguageIdFromRequestAsync();
                var query = _context.Companies.Where(a => a.IsDeleted == dm.ShowDeleted).AsQueryable().AsNoTracking()
                    .Select(x => new CompanyListDTO
                    {
                        Id = x.Id,
                        Phone = x.Phone,
                        WhatsApp = x.WhatsApp,
                        Email = x.Email,
                        ManagedBy = "",
                        Website = x.Website,
                        CompanyName = x.CompanyTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CompanyName,
                        MainImgUrl = x.MainImgUrl,
                        CompanyLogo = x.ComapnyLogo,
                        ManagerName = x.ManagerName,
                        ManagerEmail = x.ManagerEmail,
                        FullAddress = x.FullAddress,
                        CountryName = _context.SysCountryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CountryName,
                        CityName = x.SysCity.SysCityTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CityName,
                        CategoryName = x.SysCompanyCategory.SysCompanyCategoryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CompanyCategoryName,
                        Password = "*",
                        IsDeleted = x.IsDeleted,
                        //CreateDate = x.CreationDate,
                    });

                return await SyncGridOperations<CompanyListDTO>.PagingAndFilterAsync(query, dm);
            }
            catch (Exception)
            {
                return null;
            }   
        }
        //-----------------------------------------------------------------------------------
        public async Task<SyncPaginatedListModel<CompanyListDTO>> GetAllAsListToEditAsync(DataManagerRequest dm)
        {
            var WorkingLanguage = await _languageService.GetLanguageIdFromRequestAsync();
            var query = _context.Companies.Where(a => !a.IsDeleted).AsQueryable().AsNoTracking()
                .Select(x => new CompanyListDTO
                {
                    Id = x.Id,
                    Phone = x.Phone,
                    WhatsApp = x.WhatsApp,
                    Email = x.Email,
                    ManagedBy = "",
                    Website = x.Website,
                    CompanyName = x.CompanyTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CompanyName,
                    MainImgUrl = x.MainImgUrl,
                    CompanyLogo = x.ComapnyLogo,
                    ManagerName = x.ManagerName,
                    ManagerEmail = x.ManagerEmail,
                    FullAddress = x.FullAddress,
                    //CityId = x.CityId,
                    //CountryId = x.CountryId,
                    CountryName = _context.SysCountryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CountryName,
                    CityName = x.SysCity.SysCityTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CityName,
                    Categoryid = x.CompanyCategoryId ?? 0,
                    CategoryName = x.SysCompanyCategory.SysCompanyCategoryTranslates.FirstOrDefault(x => x.LanguageId == WorkingLanguage).CompanyCategoryName,
                    Password = "*",
                    FacebookURL = x.FacebookURL,
                    InstagramURL = x.InstagramURL,
                    linkedinURL = x.linkedinURL,
                    TwitterURL = x.TwitterURL,
                    UserName = _context.Users.FirstOrDefault(a => a.UserType == UserType.Companies && a.ReferenceId == x.Id.ToString()).UserName,
                    //CompanyTranslatesss = x.CompanyTranslates.Select(a => new CreateNewCompanyTranslateDTO
                    //{
                    //    CompanyId = x.Id,
                    //    CompanyName = a.CompanyName,
                    //    LanguageId = a.LanguageId,
                    //    Id = a.Id,
                    //}).AsQueryable()
                });

            return await SyncGridOperations<CompanyListDTO>.PagingAndFilterAsync(query, dm);

        }


        //-----------------------------------------------------------------------------------
        public async Task<CreateNewCompanyDTO> GetByIdAsync(int id)
        {
            try
            {
                var Languages = _languageService.GetAllLanguages();
                var Comapny = await _context.Companies.Where(x => x.Id == id).Include(x => x.CompanyTranslates).FirstOrDefaultAsync();
                var user = await _context.Users.Where(a => a.ReferenceId == Comapny.Id.ToString() && a.UserType == UserType.Companies).FirstOrDefaultAsync();
                foreach (var lang in Languages)
                {
                    var IsExist = Comapny.CompanyTranslates.FirstOrDefault(x => x.LanguageId == lang.Id && x.CompId == Comapny.Id);
                    if (IsExist == null)
                    {
                        var NewCopanyTranslate = new CompanyTranslate()
                        {
                            LanguageId = lang.Id,
                            CompId = Comapny.Id,
                            CompanyName = "",
                        };
                        Comapny.CompanyTranslates.Add(NewCopanyTranslate);
                        await _context.SaveChangesAsync();
                    }
                }
                var model = _mapper.Map<CreateNewCompanyDTO>(Comapny);
                //model.UserId = user.Id;
                //model.OwnerName = user.FullName;
                model.Attachments = await _context.Attachments.Where(x => x.ReferenceType == ReferenceType.Companies && x.ReferenceId == id.ToString()).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetByIdAsync), ex);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<MyResponseResult> UpdateCompany(CreateNewCompanyDTO createNewCompanyDTO)
        {
            try
            {
                var requiredFields = await CheckRequiredFields(createNewCompanyDTO);
                if (requiredFields != null) if (requiredFields.Any())
                    return Error(requiredFields, System.Net.HttpStatusCode.BadRequest);

                _cacheService.Delete("GetCompaniesAsLookupCacheKey");
                var model = _mapper.Map<Company>(createNewCompanyDTO);
                if (createNewCompanyDTO.MainImageFile != null)
                {
                    model.MainImgUrl = await _fileManager.UploadFileAsync(createNewCompanyDTO.MainImageFile,
                        ReferenceType.Companies, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewCompanyDTO.MainImgUrl);
                }
                if (createNewCompanyDTO.CompanyLogoFile != null)
                {
                    model.ComapnyLogo = await _fileManager.UploadFileAsync(createNewCompanyDTO.CompanyLogoFile,
                    ReferenceType.Companies, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewCompanyDTO.ComapnyLogo);
                }
                if (createNewCompanyDTO.TaxDocumentImageFile != null)
                {
                    model.TaxDocumentImage = await _fileManager.UploadFileAsync(createNewCompanyDTO.TaxDocumentImageFile,
                    ReferenceType.Companies, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewCompanyDTO.TaxDocumentImage);
                }
                if (createNewCompanyDTO.ManagerSignatureImageFile != null)
                {
                    model.ManagerSignatureImage = await _fileManager.UploadFileAsync(createNewCompanyDTO.ManagerSignatureImageFile,
                    ReferenceType.Companies, true, MediaType.Photos, model.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(createNewCompanyDTO.ManagerSignatureImage);
                }
                if (createNewCompanyDTO.AttachmentsFiles != null)
                {
                    _context.Attachments.AddRange(await _fileManager.UploadFilesAsync(createNewCompanyDTO.AttachmentsFiles, ReferenceType.Companies, true, MediaType.Photos, model.Id.ToString()));
                }
                _context.Update(model);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateCompany), ActionType = ActionType.Update, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Companies });
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task CompanyApprove(int id)
        {
            try
            {
                var company = await _context.Companies.Where(x => x.Id == id).Include(x => x.CompanyTranslates).FirstOrDefaultAsync();
                company.ApproveState = ApproveStatment.Approved;
                company.ApprovedBy = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                company.ApprovedDate = DateTime.Now;
                _context.Update(company);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CompanyApprove), ActionType = ActionType.Approve, TransReferenceId = company.Id.ToString(), TransType = LogTransType.Companies });
                #region SendEmail
                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", company.Email, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName,
                    "https://www.insaattan.com/account/login", EmailType.CompanyApprovement, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName);
                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", company.ManagerEmail, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName,
                   "https://www.insaattan.com/account/login", EmailType.CompanyApprovement, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName);
                #endregion
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CompanyApprove), ex);
            }

        }

        //-----------------------------------------------------------------------------------
        public async Task CompanyDisApprove(int id)
        {
            try
            {
                var company = await _context.Companies.Where(x => x.Id == id).FirstOrDefaultAsync();
                company.ApproveState = ApproveStatment.DisApproved;
                _context.Update(company);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(CompanyDisApprove), ActionType = ActionType.DisApprove, TransReferenceId = company.Id.ToString(), TransType = LogTransType.Companies });

                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", company.Email, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName,
                   "", EmailType.CompanyDisApprovement, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName);
                await _emailService.SendEmail("noreply@insaattan.com", "insaattan", company.ManagerEmail, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName,
                   "", EmailType.CompanyDisApprovement, company.CompanyTranslates.FirstOrDefault(x => x.LanguageId == 1).CompanyName);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(CompanyDisApprove), ex);
            }

        }

        //-----------------------------------------------------------------------------------
        public async Task<CompanyViewModel> GetCompanyByIdForView(int id, int? LangId = null)
        {
            try
            {
                LangId = LangId ?? await _languageService.GetLanguageIdFromRequestAsync();

                CompanyViewModel company = await _context.Companies.Where(c => !c.IsDeleted).Select(x => new CompanyViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyTranslates.FirstOrDefault(x => x.LanguageId == LangId).CompanyName,
                    MainImg = x.MainImgUrl,
                    Email = x.Email,
                    Logo = x.ComapnyLogo,
                    PhoneNumber = x.Phone,
                    Website = x.Website,
                    WhatsApp = x.WhatsApp,
                    //AvrageRating = x.AvrageRating,
                    TwitterURL = x.TwitterURL,
                    linkedinURL = x.linkedinURL,
                    InstagramURL = x.InstagramURL,
                    FacebookURL = x.FacebookURL,
                    CompanyCategoryName = x.SysCompanyCategory.SysCompanyCategoryTranslates.FirstOrDefault(x => x.LanguageId == LangId).CompanyCategoryName,
                    Content = x.CompanyTranslates.FirstOrDefault(x => x.LanguageId == LangId).Content
                }).FirstOrDefaultAsync(c => c.Id == id);

                //company.AvrageRating = await _ratingAndCommentsService.GetAvg(ReferenceType.Companies, id.ToString());

                return company;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompanyByIdForView), ex);
                return null;
            }
        }

        public async Task<CompanyViewModel> GetCompanyDetailsByIdForView(int id)
        {
            try
            {
                var LangId = await _languageService.GetLanguageIdFromRequestAsync();
                var company = await _context.Companies.Where(c => !c.IsDeleted).Select(x => new CompanyViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyTranslates.FirstOrDefault(x => x.LanguageId == LangId).CompanyName,
                    Email = x.Email,
                    Logo = x.ComapnyLogo,
                    MainImg = x.MainImgUrl,
                    PhoneNumber = x.Phone,
                    Website = x.Website,
                    WhatsApp = x.WhatsApp,
                    Content = x.CompanyTranslates.FirstOrDefault(ct => ct.LanguageId == LangId).Content ?? "",
                    //AvrageRating = x.AvrageRating,
                    Address = x.FullAddress + " - " + x.SysCity.SysCityTranslates.FirstOrDefault(city => city.LanguageId == LangId).CityName
                                + " - " + x.SysCity.SysCountry.SysCountryTranslates.FirstOrDefault(country => country.LanguageId == LangId).CountryName,
                }).FirstOrDefaultAsync(c => c.Id == id);

                //company.Projects = PaginatedList<ProjectCardViewModel>.Create(_projectsService.GetProjectsByCompanyAsync(id).Result.AsQueryable(), 1, 6);

                //company.RealEstates = PaginatedList<RealEstateCardViewModel>.Create(_realEstatesService.GetRealEstatesByCompanyId(id).Result.AsQueryable(), 1, 6);

                return company;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompanyDetailsByIdForView), ex);
                return null;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<bool> IsCompanyVerified(int compId)
        {
            try
            {
                return (await _context.Companies.FindAsync(compId)).ApproveState == ApproveStatment.Approved;
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(IsCompanyVerified), ex);
                return false;
            }
        }

        //----------------------------------------------------------------------------------- 
        public async Task<string> GetCompanyName(int compId)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                return await _context.Companies.Where(a => a.Id == compId).Select(a => a.CompanyTranslates.FirstOrDefault(a => a.LanguageId == langId).CompanyName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _ = _loggerService.LogErrorAsync(nameof(GetCompanyName), ex);
                return "Company" + compId;
            }
        }

        //-----------------------------------------------------------------------------------
        public async Task<bool> CheckIfFirstTime(int id)
        {
            try
            {
                var model = await _context.Companies.Where(a => a.Id == id).FirstOrDefaultAsync();
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
                var model = await _context.Companies.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (model != null)
                {
                    model.IsTearmAndConditionsAccepted = true;
                    _context.Companies.Update(model);
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
                _cacheService.Delete("GetCompaniesAsLookupCacheKey");
                //if (await _context.Projects.AnyAsync(a => !a.IsDeleted && a.CompId == id))
                //    return Error("Cannot delete company that has projects");

                var comp = await _context.Companies.FirstOrDefaultAsync(a => a.Id == id);

                // Deleting Project Files
                _ = _fileManager.DeleteFileAsync(comp.MainImgUrl);
                var attachments = await _context.Attachments.Where(a =>
                a.ReferenceType == ReferenceType.Companies &&
                a.ReferenceId == id.ToString()).ToListAsync();
                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        _ = _fileManager.DeleteFileAsync(attachment.FileName);
                        _context.Attachments.Remove(attachment);
                    }
                }


                _context.Companies.Remove(comp);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();



                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeletePermenantly), ActionType = ActionType.DeletePermenantly, TransReferenceId = comp.Id.ToString(), TransType = LogTransType.Companies });
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
                var comp = await _context.Companies.FirstOrDefaultAsync(a => a.Id == id);

                comp.IsDeleted = false;
                _context.Companies.Update(comp);
                await _context.SaveChangesAsync();


                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(Restore), ActionType = ActionType.Restore, TransReferenceId = comp.Id.ToString(), TransType = LogTransType.Companies });
                return Success();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(Restore), ex);
                return Error(ex);
            }
        }

        //============//============//============//============//============
        private async Task<List<string>> CheckRequiredFields(CreateNewCompanyDTO model)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var requiredFields = await _context.RequiredFields.FirstOrDefaultAsync(a => a.ReferenceType == "Company");
                if (requiredFields == null)
                    return null;

                if (string.IsNullOrEmpty(requiredFields.Required))
                    return null;

                List<string> validations = new();
                requiredFields.Required.Split(",").ForEach(requiredPropName =>
                {
                    if (typeof(CreateNewCompanyDTO).GetProperties().Any(a => a.Name == requiredPropName))
                    {
                        var val = model.GetType().GetProperty(requiredPropName)?.GetValue(model)?.ToString();
                        if (string.IsNullOrEmpty(val))
                        {
                            validations.Add(_localizationService.GetStringResource("model.Company." + requiredPropName, langId));
                        }
                    }

                    if (typeof(CreateNewCompanyTranslateDTO).GetProperties().Any(a => a.Name == requiredPropName))
                        model.CompanyTranslates.ForEach(translate =>
                        {
                            var val = translate.GetType().GetProperty(requiredPropName).GetValue(translate)?.ToString();
                            if (string.IsNullOrEmpty(val))
                            {
                                var msg = _localizationService.GetStringResource("model.Company." + requiredPropName, langId);
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


        #region Company Transactions
        //-----------------------------------------------------------------------------------
        public async Task<bool> UpdateMyCompany(CreateNewCompanyDTO model)
        {
            try
            {
                var company = await _context.Companies.Include(a => a.CompanyTranslates).FirstOrDefaultAsync(a => a.Id == model.Id);
                if (company == null)
                    return false;

                company.CompanyTranslates = _mapper.Map<List<CompanyTranslate>>(model.CompanyTranslates);
                company.CityId = model.CityId;
                company.CountryId = model.CountryId;
                company.FullAddress = model.FullAddress;
                company.Phone = model.Phone;
                company.WhatsApp = model.WhatsApp;
                company.Email = model.Email;
                company.Website = model.Website;
                company.ManagerName = model.ManagerName;
                company.ManagerEmail = model.ManagerEmail;


                if (model.MainImageFile != null)
                {
                    company.MainImgUrl = await _fileManager.UploadFileAsync(model.MainImageFile,
                        ReferenceType.Companies, true, MediaType.Photos, company.Id.ToString());

                    _ = _fileManager.DeleteFileAsync(model.MainImgUrl);

                }
                if (model.CompanyLogoFile != null)
                {
                    company.ComapnyLogo = await _fileManager.UploadFileAsync(model.CompanyLogoFile,
                    ReferenceType.Companies, true, MediaType.Photos, company.Id.ToString());
                    _ = _fileManager.DeleteFileAsync(model.ComapnyLogo);
                }
                _context.Update(company);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateMyCompany), ActionType = ActionType.Update, TransReferenceId = company.Id.ToString(), TransType = LogTransType.Companies });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateMyCompany), ex);
                return false;
            }
        }
        public async Task<int> GetCompanyIdByUserId(string userId)
        {
            try
            {
                int companyId = int.Parse((await _context.Users.FirstOrDefaultAsync(e => e.Id == userId))?.ReferenceId);

                return companyId;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCompanyIdByUserId), ex);
                return 0;
            }
        }
        #endregion


    }
}
