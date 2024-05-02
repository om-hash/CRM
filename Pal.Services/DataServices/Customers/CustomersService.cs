using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Account;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Customers;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Core.Enums.ActivityLog;
using Pal.Core.Enums.Approvment;
using Pal.Core.Enums.Attachment;
using Pal.Core.Enums.Customer;
using Pal.Core.Enums.Roles;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Call;
using Pal.Data.DTOs.Customer;
using Pal.Data.DTOs.Notifications;
using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using Pal.Data.DTOs.Region;
using Pal.Data.DTOs.Tour;
using Pal.Services.Caching;
using Pal.Services.CRM.Calls;
using Pal.Services.CRM.Meetings;
using Pal.Services.CRM.Tasks;
using Pal.Services.DataServices.Lookups;
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
using System.Threading.Tasks;

namespace Pal.Services.DataServices.Customers
{
    public class CustomersService : RepositoryBase<Customer>, ICustomersService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;
        private readonly IWebWorkContext _workContext;
        private readonly IFileManagerService _fileManager;
        private readonly ILocalizationService _localization;
        private readonly ILoggerService _logger;
        private readonly ILookupsService _lookupsService;
        private readonly ICallService _callService;
        private readonly IMeetingService _meetingService;
        private readonly ITasksService _tasksService;
        private readonly ICacheService<Customer> _cacheService;
        private readonly ILanguageService _languageService;


        private readonly UserManager<ApplicationUser> _userManager;


        public CustomersService(ApplicationDbContext context, IMapper mapper, IWebWorkContext workContext, 
            IFileManagerService fileManager, ILocalizationService localization, ILookupsService lookupsService,
            ILoggerService logger, UserManager<ApplicationUser> userManager,
            ICallService callService, IMeetingService meetingService, ILanguageService languageService,
            ITasksService tasksService, ICacheService<Customer> cacheService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _workContext = workContext;
            _fileManager = fileManager;
            _localization = localization;
            _logger = logger;
            _lookupsService = lookupsService;
            _userManager = userManager;

            _callService = callService;
            _meetingService = meetingService;
            _tasksService = tasksService;
            _cacheService = cacheService;
            _languageService = languageService;
        }



        #region MyRegion
        //----------------------------------------------------------------------------------------
        public async Task<SyncPaginatedListModel<CustomersListDTO>> GetCustomersAsListAsync(DataManagerRequest dm)
        {
            try
            {
                var user = await _workContext.GetMyUserDetails();
                var employeeId = await _workContext.GetEmployeeIdByUserId(user.Id);
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var query = FindByCondition(c => !c.IsDeleted && !c.IsLead).AsQueryable().AsNoTracking()
                    .Select(x => new CustomersListDTO
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        NationalityName = x.SysNationality.SysNationalityTranslates.FirstOrDefault(a => a.LanguageId == langId) == null ? "" : x.SysNationality.SysNationalityTranslates.FirstOrDefault(a => a.LanguageId == langId).NationalityName,
                        PhoneNumber = x.PhoneNumber,
                        CountryName = x.SysCountry.SysCountryTranslates.FirstOrDefault(a => a.LanguageId == langId) == null ? "" : x.SysCountry.SysCountryTranslates.FirstOrDefault(a => a.LanguageId == langId).CountryName,
                        RegisterdByAdmin = x.IsRegisterdByAdmin ? "Yes" : "No",
                        CreatedBy = x.CreatedBy != null || x.CreatedBy != "" ? x.CreatedBy : "No",
                        // FollowedByEmpId = x.FollowedByEmpId,
                    });

                //if (!await _userManager.IsInRoleAsync(user, PalRoles.SuperAdmin.ToString()) && employeeId > 0)
                //    query.Where(a => a.FollowedByEmpId == employeeId);


                var data = await SyncGridOperations<CustomersListDTO>.PagingAndFilterAsync(query, dm);


                //_userManager.IsInRoleAsync()
                foreach (var item in data.Data)
                {
                    var userSS = _context.Users.FirstOrDefault(a => a.ReferenceId == item.Id.ToString() && a.UserType == UserType.Customers);

                    item.LastLogInDate = userSS?.LastLogInDate.ToString("dd MMMM yyyy");
                }

                return data;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCustomersAsListAsync), ex);
                return null;
            }

        }

        public async Task<string> GetImageByIdAsync()
        {
            try
            {
                var customerId = long.Parse(_workContext.GetMyCustomerId());
                var image = (await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId)).MainImage;
                return image;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetImageByIdAsync), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<CustomerDTO> GetByIdAsync(long id)
        {
            try
            {
                var customer = await _context.Customers
                    .Include(a => a.CustomerFavorites).Include(x => x.CustomerNotes)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (customer == null)
                    return null;

                var user = await _context.Users.FirstOrDefaultAsync(a => a.ReferenceId == id.ToString() && a.UserType == UserType.Customers);
                var customerDTO = _mapper.Map<CustomerDTO>(customer);

                //if (customerDTO.MostInterestingProjects != null)
                //{
                //    var projectIds = customerDTO.MostInterestingProjects.Split(",").Skip(1).Select(a => int.Parse(a));
                //    //customerDTO.MostInterestingProjectsId = projectIds.ToList();
                //}
                customerDTO.Username = user?.UserName;
                customerDTO.CustomerNotes = customer.CustomerNotes.Select(x => new CustomerNoteDTO { AddedByEmpId = x.AddedByEmpId, Content = x.Content, CustomerId = x.CustomerId, Date = x.Date, Id = x.Id }).ToList();
                customerDTO.Attachments = await _context.Attachments.Where(a => a.ReferenceType == ReferenceType.Customer && a.ReferenceId == customerDTO.Id.ToString()).ToListAsync();
                customerDTO.IsEmailVerified = user?.EmailConfirmed ?? false;
                //customerDTO.CustomerNotes.AddRange(customer.CustomerNotes.Select(x => new CustomerNoteDTO { AddedByEmpId = x.AddedByEmpId, Content = x.Content, CustomerId = x.CustomerId, Date = x.Date }).ToList());
                var languages = _languageService.GetAllLanguages();
                return customerDTO;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetByIdAsync), ex);
                return null;
            }
        }

        //----------------------------------------------------------------------------------------
        public async Task<long> AddAsync(CustomerDTO model)
        {
            try
            {
                _cacheService.Delete("GetCutomerAsLookupCacheKey");
                Customer customer = _mapper.Map<Customer>(model);
                _context.Add(customer);
                await _context.SaveChangesAsync();
                if (model.MainImageFile != null)
                {
                    var newImagePath = await _fileManager.UploadFileAsync(model.MainImageFile, ReferenceType.Customer, true, MediaType.Photos, customer.Id.ToString());
                    customer.MainImage = newImagePath;
                }
                _context.Update(customer);
                await _context.SaveChangesAsync();

                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(AddAsync), ActionType = ActionType.Add, TransReferenceId = customer.Id.ToString(), TransType = LogTransType.Customer });
                return customer.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomerService.AddAsync", ex);
                return 0;
            }
        }



        //----------------------------------------------------------------------------------------
        public async Task<long> UpdateAsync(long id, CustomerDTO model)
        {
            using var transaction = _context.BeginTransaction();

            try
            {
                _cacheService.Delete("GetCutomerAsLookupCacheKey");
                var cust = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

                //model.GetType().GetProperties().ForEach(itm => itm.SetValue(model, 
                //(itm.Name == cust.GetType().GetProperties().FirstOrDefault(prop => prop.Name == itm.Name).Name && 
                //itm.GetValue(model.GetType()) == null)? 
                //cust.GetType().GetProperties().FirstOrDefault(prop => prop.Name == itm.Name).GetValue(model.GetType()) : 
                //itm.GetValue(model.GetType())
                //));

                model.Id = id;
                model.CreatedBy = cust.CreatedBy;
               
                var customer = _mapper.Map<Customer>(model);
                if (model.MainImageFile != null)
                {
                    var newImagePath = await _fileManager.UploadFileAsync(model.MainImageFile, ReferenceType.Customer, true, MediaType.Photos, model.Id.ToString());
                    if (newImagePath != null)
                    {
                        await _fileManager.DeleteFileAsync(model.MainImage);
                        customer.MainImage = newImagePath;
                    }

                }
                _context.Update(customer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateAsync), ActionType = ActionType.Update, TransReferenceId = customer.Id.ToString(), TransType = LogTransType.Customer });
                return customer.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomerService.UpdateAsync", ex);
                await transaction.RollbackAsync();
                return 0;
            }

        }


        //----------------------------------------------------------------------------------------
        public async Task<ResponseType> DeleteAsync(long id)
        {
            using var transaction = _context.BeginTransaction();

            try
            {
                //TODO من الممكن انه رح يصير مستقبلا جداول تربطها بالزبون فـ دير بالك تحذفها!!

                var customer = await _context.Customers.FirstOrDefaultAsync(a => a.Id == id);
                //_context.Customers.Remove(customer);

                customer.IsDeleted = true;

                _context.Customers.Update(customer);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // delete customer from cache
                _cacheService.Delete("GetCutomerAsLookupCacheKey");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == customer.UserId);
                if (user != null)
                {
                    user.IsDeleted = true;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }



                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteAsync), ActionType = ActionType.Delete, TransReferenceId = customer.Id.ToString(), TransType = LogTransType.Customer });
                return ResponseType.Success;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomerService.DeleteAsync", ex);
                await transaction.RollbackAsync();
                return ResponseType.Error;
            }

        }

        //----------------------------------------------------------------------------------------
        public async Task<string> GetCustomerName(int customerId)
        {
            try
            {
                return await _context.Customers.Where(a => a.Id == customerId).Select(a => a.FullName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCustomerName), ex);
                return "Customer";
            }
        }



        //-----------------------------------------------------------------------------------------
        public async Task<bool> AddToFavoriteAsync(CustomerFavoriteDTO model)
        {
            try
            {
                model.CustomerId = long.Parse(_workContext.GetMyCustomerId());
                var customerFavorite = new CustomerFavorite
                {
                    CustomerId = model.CustomerId,
                    Date = model.Date,
                    FavoriteReference = model.FavoriteReference,
                    FavoriteType = model.FavoriteType,
                };
                _context.CustomerFavorites.Add(customerFavorite);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(AddToFavoriteAsync), ActionType = ActionType.Add, TransReferenceId = "", TransType = LogTransType.Customer });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(AddToFavoriteAsync), ex);
                return false;
            }
        }

        ////-----------------------------------------------------------------------------------------
        //public async Task<List<CustomerFavoriteViewModel>> GetFavoritesProjectAsync(int? languageId)
        //{
        //    var langId = languageId ?? await _languageService.GetLanguageIdFromRequestAsync();
        //    long customerId = long.Parse(_workContext.GetMyCustomerId());
        //    try
        //    {
        //        //if (cusId > 0)
        //        //    customerId = cusId;
        //        //else
        //        //    customerId = long.Parse(_workContext.GetMyCustomerId());

        //        List<CustomerFavoriteViewModel> result = new List<CustomerFavoriteViewModel>();

        //        var favs = await _context.CustomerFavorites.Where(cf => cf.CustomerId == customerId && cf.FavoriteType == CustomerFavoriteType.Projects).ToListAsync();
        //        var favRegions = favs.Where(a => a.FavoriteType == Core.Enums.Customer.CustomerFavoriteType.Region).ToList();
        //        List<CustomerFavoriteViewModel> favorites = new();
        //        favorites.AddRange(_context.SysRegions.Where(sysregion => favRegions.Select(a => a.FavoriteReference)
        //            .Contains(sysregion.Id)).Select(a => new CustomerFavoriteViewModel
        //            {
        //                Id = a.Id,
        //                Name = a.SysRegionTranslates.FirstOrDefault(r => r.LanguageId == langId).RegionName,
        //                Image = a.MainImg,
        //                Address = a.FullAddress,
        //                Price = a.MeterAvgPrice,

        //                ItemType = CustomerFavoriteType.Region,
        //                CityId = a.CityId,
        //            }).ToList());
        //        var favRealEstates = favs.Where(a => a.FavoriteType == Core.Enums.Customer.CustomerFavoriteType.RealEstates).ToList();
        //        //favorites.AddRange(await _context.RealEstates
        //        //    .Where(r => favRealEstates.Select(a => a.FavoriteReference)
        //        //    .Contains(r.Id)).Select(a => new CustomerFavoriteViewModel
        //        //    {
        //        //        Id = a.Id,
        //        //        Name = a.RealEstateTranslates.FirstOrDefault(r => r.LanguageId == langId).Title,
        //        //        Image = a.MainImgUrl,
        //        //        Address = a.FullAddress,
        //        //        Price = a.Price,
        //        //        Link = "/RealEstates" + "/RealEstate/" + a.Id,
        //        //        AdminDashboardLink = "/admin/RealEstates/RealEstateUpdate/" + a.Id,
        //        //        ItemType = CustomerFavoriteType.RealEstates,
        //        //        ReferenceType = ReferenceType.RealEstates,
        //        //        CityId = a.CityId,
        //        //    }).ToListAsync());
        //        var favProjects = favs.Where(a => a.FavoriteType == Core.Enums.Customer.CustomerFavoriteType.Projects).ToList();
        //        favorites.AddRange(await _context.Projects
        //            .Where(r => favProjects.Select(a => a.FavoriteReference)
        //            .Contains(r.Id)).Select(a => new CustomerFavoriteViewModel
        //            {
        //                Id = a.Id,
        //                Name = a.ProjectTranslates.FirstOrDefault(r => r.LanguageId == langId).ProjectName,
        //                Image = a.MainImg,
        //                Address = a.FullAddress,
        //                Price = a.MeterAvgPrice,
        //                Link = "/Projects" + "/Project/" + a.Id,
        //                AdminDashboardLink = "/admin/Projects/ProjectUpdate/" + a.Id,
        //                ItemType = CustomerFavoriteType.Projects,
        //                ReferenceType = ReferenceType.Projects,
        //                CityId = a.CityId,

        //            }).ToListAsync());

        //        foreach (var item in favorites)
        //        {
        //            //item.IsAddedToTour = await _tourService.IsAddedToTour(item.Id, item.ReferenceType);
        //        }

        //        return favorites;
        //    }
        //    catch (Exception ex)
        //    {
        //        _ = _logger.LogErrorAsync(nameof(GetFavoritesAsync), ex);
        //        return null;
        //    }
        //}

        //=========================================================================================
        public async Task<List<CustomerFavoriteViewModel>> GetFavoritesAsync(int? languageId)
        {
            var langId = languageId ?? await _languageService.GetLanguageIdFromRequestAsync();
            long customerId = long.Parse(_workContext.GetMyCustomerId());
            try
            {
                //if (cusId > 0)
                //    customerId = cusId;
                //else
                //    customerId = long.Parse(_workContext.GetMyCustomerId());

                List<CustomerFavoriteViewModel> result = new List<CustomerFavoriteViewModel>();

                var favs = await _context.CustomerFavorites.Where(cf => cf.CustomerId == customerId).ToListAsync();
                var favRegions = favs.Where(a => a.FavoriteType == Core.Enums.Customer.CustomerFavoriteType.Region).ToList();
                List<CustomerFavoriteViewModel> favorites = new();
                favorites.AddRange(_context.SysRegions.Where(sysregion => favRegions.Select(a => a.FavoriteReference)
                    .Contains(sysregion.Id)).Select(a => new CustomerFavoriteViewModel
                    {
                        Id = a.Id,
                        Name = a.SysRegionTranslates.FirstOrDefault(r => r.LanguageId == langId).RegionName,
                        Image = a.MainImg,
                        Address = a.FullAddress,
                        ItemType = CustomerFavoriteType.Region,
                        CityId = a.CityId,
                    }).ToList());
                var favRealEstates = favs.Where(a => a.FavoriteType == Core.Enums.Customer.CustomerFavoriteType.RealEstates).ToList();
                //favorites.AddRange(await _context.RealEstates
                //    .Where(r => favRealEstates.Select(a => a.FavoriteReference)
                //    .Contains(r.Id)).Select(a => new CustomerFavoriteViewModel
                //    {
                //        Id = a.Id,
                //        Name = a.RealEstateTranslates.FirstOrDefault(r => r.LanguageId == langId).Title,
                //        Image = a.MainImgUrl,
                //        Address = a.FullAddress,
                //        Price = a.Price,
                //        Link = "/RealEstates" + "/RealEstate/" + a.Id,
                //        AdminDashboardLink = "/admin/RealEstates/RealEstateUpdate/" + a.Id,
                //        ItemType = CustomerFavoriteType.RealEstates,
                //        ReferenceType = ReferenceType.RealEstates,
                //        CityId = a.CityId,
                //    }).ToListAsync());
                //var favProjects = favs.Where(a => a.FavoriteType == Core.Enums.Customer.CustomerFavoriteType.Projects).ToList();
                //favorites.AddRange(await _context.Projects
                //    .Where(r => favProjects.Select(a => a.FavoriteReference)
                //    .Contains(r.Id)).Select(a => new CustomerFavoriteViewModel
                //    {
                //        Id = a.Id,
                //        Name = a.ProjectTranslates.FirstOrDefault(r => r.LanguageId == langId).ProjectName,
                //        Image = a.MainImg,
                //        Address = a.FullAddress,
                //        Price = a.MeterAvgPrice,
                //        Link = "/Projects" + "/Project/" + a.Id,
                //        AdminDashboardLink = "/admin/Projects/ProjectUpdate/" + a.Id,
                //        ItemType = CustomerFavoriteType.Projects,
                //        ReferenceType = ReferenceType.Projects,
                //        CityId = a.CityId,

                //    }).ToListAsync());
                var image = (await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId))?.MainImage;
                if (favorites.Count > 0)
                {
                    favorites[0].ProfileSidebarViewModel = new Data.VMs.Customer.ProfileSidebarViewModel(image);
                }
                foreach (var item in favorites)
                {
                    //item.IsAddedToTour = await _tourService.IsAddedToTour(item.Id, item.ReferenceType);
                }

                return favorites;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetFavoritesAsync), ex);
                return null;
            }
        }

        //=========================================================================================
        public class AllFavoritesDTO
        {
            public AllFavoritesDTO()
            {
                regions = new();
                projects = new();
                realestates = new();
            }

            public List<RegionListDTO> regions { get; set; }
            public List<ProjectCardViewModel> projects { get; set; }
            public List<RealEstateCardViewModel> realestates { get; set; }
        }
        public async Task<AllFavoritesDTO> GetAllFavorites(int? languageId)
        {
            var langId = languageId ?? await _languageService.GetLanguageIdFromRequestAsync();
            long customerId = long.Parse(_workContext.GetMyCustomerId());
            try
            {
                var result = new AllFavoritesDTO();
                var favs = await _context.CustomerFavorites.Where(cf => cf.CustomerId == customerId).ToListAsync();

                result.regions = (await _context.SysRegions.Where(r => !r.IsDeleted && r.IsOnlyLookup == false)
                  .Where(p => _context.CustomerFavorites.Any(a => a.CustomerId == customerId && a.FavoriteType == CustomerFavoriteType.Region && a.FavoriteReference == p.Id))
                  .Select(p => new RegionListDTO
                  {
                      Id = p.Id,
                      RegionName = p.SysRegionTranslates.FirstOrDefault(t => t.LanguageId == langId).RegionName,
                      LocationUrl = p.LocationUrl,
                      MainImgUrl = p.MainImg,
                      SubTitle = p.SysRegionTranslates.FirstOrDefault(t => t.LanguageId == langId).SubTitle,
                      CityName = p.SysCity.SysCityTranslates.FirstOrDefault(c => c.LanguageId == langId).CityName,
                      CityId = p.CityId,
                      CountryId = p.CountryId,
                      IsAddedToFavorite = true,

                  }).ToListAsync())
                  .OrderByDescending(a => favs.FirstOrDefault(x => x.FavoriteType == CustomerFavoriteType.Region && x.FavoriteReference == a.Id).Date)
                           .ToList();

                return result;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetFavoritesAsync), ex);
                return null;
            }
        }


        //-----------------------------------------------------------------------------------------
        public async Task UpdateCustomerProfile(long custId, CustomerDTO model)
        {
            try
            {
                _cacheService.Delete("GetCutomerAsLookupCacheKey");
                var customer = await _context.Customers.FirstOrDefaultAsync(a => a.Id == custId);
                customer.FullName = model.FullName;
                customer.NationalityId = model.NationalityId;
                customer.CountryId = model.CountryId;
                customer.Email = model.Email;
                customer.WhatsappNumber = model.WhatsappNumber;
                if (model.MainImageFile != null)
                {
                    var newImagePath = await _fileManager.UploadFileAsync(model.MainImageFile, ReferenceType.Customer, true, MediaType.Photos, model.Id.ToString(), "customerProfile");
                    if (newImagePath != null)
                    {
                        await _fileManager.DeleteFileAsync(model.MainImage);
                        customer.MainImage = newImagePath;
                    }

                }

                _context.Update(customer);
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateCustomerProfile), ActionType = ActionType.Update, TransReferenceId = customer.Id.ToString(), TransType = LogTransType.Customer });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateCustomerProfile), ex);
            }

        }
        //-----------------------------------------------------------------------------------------
        public async Task<bool> DeleteFromFavorite(int id, CustomerFavoriteType referenceType, long custId)
        {
            try
            {
                var item = await _context.CustomerFavorites.Where(x => x.FavoriteReference == id && x.FavoriteType == referenceType && x.CustomerId == custId).FirstOrDefaultAsync();
                _context.CustomerFavorites.Remove(item);
                _context.SaveChanges();
                //await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteFromFavorite), ActionType = ActionType.Delete, TransReferenceId = item.Id.ToString(), TransType = LogTransType.Customer });
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteFromFavorite), ex);
                return false;
            }
        }
        //-----------------------------------------------------------------------------------------
        public async Task<bool> IsAddedToFavorite(int Id, CustomerFavoriteType referenceType)
        {
            var customerId = long.Parse(_workContext.GetMyCustomerId());
            var IsExist = await _context.CustomerFavorites.AnyAsync(x => x.CustomerId == customerId && x.FavoriteType == referenceType && x.FavoriteReference == Id);
            return IsExist;
        }

        //-----------------------------------------------------------------------------------------
        public async Task<long> GetCustomerdByUserId(string userId)
        {
            try
            {
                int customerId = (int)(await _context.Customers.FirstOrDefaultAsync(e => e.UserId == userId))?.Id;

                return customerId;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCustomerdByUserId), ex);
                return 0;
            }
        }
        //----------------------------------------------------------------------------------------- 

        public async Task<bool> UpdateCustomerImage(string imageUrl)
        {
            try
            {
                var customerId = long.Parse(_workContext.GetMyCustomerId());
                var customer = await _context.Customers.FirstOrDefaultAsync(a => a.Id == customerId);
                //await _fileManager.DeleteFileAsync(customer.MainImage);
                customer.MainImage = imageUrl;

                _context.Update(customer);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateCustomerImage), ex);
                return false;
            }
        }
        //----------------------------------------------------------------------------------------- 
        public async Task<bool> SaveCustomerSearch(CustomerSavedSearchDTO model)
        {
            try
            {
                string coutryIds = "";
                string cityIds = "";
                string regionIds = "";
                string neighborhoodIds = "";
                string roomsCountIds = "";
                string BathRoomsCountIds = "";
                string featuresIds = "";

                if (model.CountryId != null)
                {
                    foreach (var item in model.CountryId)
                    {
                        coutryIds = coutryIds + "," + item;
                    }
                }
                if (model.CityId != null )
                {
                    foreach (var item in model.CityId)
                    {
                        cityIds = cityIds + "," + item;
                    }
                }
                if (model.RegionId != null )
                {
                    foreach (var item in model.RegionId)
                    {
                        regionIds = regionIds + "," + item;
                    }
                }
                if (model.neighborhoodId != null )
                {
                    foreach (var item in model.neighborhoodId)
                    {
                        neighborhoodIds = neighborhoodIds + "," + item;
                    }
                }
                if (model.RoomCount != null )
                {
                    foreach (var item in model.RoomCount)
                    {
                        roomsCountIds = roomsCountIds + "," + item;
                    }
                }
                if (model.BathRoomCount != null )
                {
                    foreach (var item in model.BathRoomCount)
                    {
                        BathRoomsCountIds = BathRoomsCountIds + "," + item;
                    }
                }
                if (model.FeaturesList != null )
                {
                    foreach (var item in model.FeaturesList)
                    {
                        featuresIds = featuresIds + "," + item;
                    }
                }

                var result = new CustomerSavedSearch()
                {
                    Date = DateTime.Now,
                    CustomerId = model.CustomerId,
                    IsForSale = model.IsForSale,
                    IsForRent = model.IsForRent,
                    RealestateTypeId = model.RealEstateTypeId,
                    StatusId = model.StatusId,
                    CountryIds = coutryIds,
                    CityIds = cityIds,
                    RegionIds = regionIds,
                    neighborhoodIds = neighborhoodIds,
                    RoomsCountIds = roomsCountIds,
                    BathRoomsCountIds = BathRoomsCountIds,
                    FeaturesIds = featuresIds,
                    MinPrice = model.MinPrice,
                    MaxPrice = model.MaxPrice,
                    MinArea = model.MinArea,
                    MaxArea = model.MaxArea,
                    AddressNamesAsString = model.AddressNamesAsString,
                    Address = model.Address,

                    IsDeleted = false,

                };

                _context.CustomerSavedSearches.Add(result);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SaveCustomerSearch), ex);
                return false;
            }
        }
        //--------------------------------------------------------------------------------
        public async Task<List<CustomerSavedSearchDTO>> GetSavedFilterForCustomer()
        {
            try
            {
                var customerId = long.Parse(_workContext.GetMyCustomerId());
                var bathRoomsList = new List<int>();
                var model = new CustomerSavedSearchDTO();
                model.BathRoomCount = new();
                model.RoomCount = new();
                model.FeaturesList = new();
                model.CountryId = new();
                model.CityId = new();
                model.RegionId = new();
                model.neighborhoodId = new();
                var result = new List<CustomerSavedSearchDTO>();
                var filters = await _context.CustomerSavedSearches.Where(x => x.CustomerId == customerId && x.IsDeleted == false).ToListAsync();
                foreach (var item in filters)
                {
                    var pasrsingList = new List<string>();
                    pasrsingList.AddRange(item.BathRoomsCountIds.Split(","));
                    pasrsingList.RemoveAll(a => a == "" || a == 0.ToString());
                    foreach (var bath in pasrsingList)
                    {
                        var ss = int.Parse(bath);
                        model.BathRoomCount.Add(ss);
                    }
                    pasrsingList.Clear();
                    pasrsingList.AddRange(item.RoomsCountIds.Split(","));
                    pasrsingList.RemoveAll(a => a == "" || a == 0.ToString());
                    foreach (var bath in pasrsingList)
                    {
                        var ss = int.Parse(bath);
                        model.RoomCount.Add(ss);
                    }
                    pasrsingList.Clear();
                    pasrsingList.AddRange(item.FeaturesIds.Split(","));
                    pasrsingList.RemoveAll(a => a == "" || a == 0.ToString());
                    foreach (var bath in pasrsingList)
                    {
                        var ss = int.Parse(bath);
                        model.FeaturesList.Add(ss);
                    }
                    pasrsingList.Clear();
                    pasrsingList.AddRange(item.CountryIds.Split(","));
                    pasrsingList.RemoveAll(a => a == "" || a == 0.ToString());
                    foreach (var bath in pasrsingList)
                    {
                        var ss = int.Parse(bath);
                        model.CountryId.Add(ss);
                    }
                    pasrsingList.Clear();
                    pasrsingList.AddRange(item.CityIds.Split(","));
                    pasrsingList.RemoveAll(a => a == "" || a == 0.ToString());
                    foreach (var bath in pasrsingList)
                    {
                        var ss = int.Parse(bath);
                        model.CityId.Add(ss);
                    }
                    pasrsingList.Clear();
                    pasrsingList.AddRange(item.RegionIds.Split(","));
                    pasrsingList.RemoveAll(a => a == "" || a == 0.ToString());
                    foreach (var bath in pasrsingList)
                    {
                        var ss = int.Parse(bath);
                        model.RegionId.Add(ss);
                    }

                    pasrsingList.Clear();
                    pasrsingList.AddRange(item.neighborhoodIds.Split(","));
                    pasrsingList.RemoveAll(a => a == "" || a == 0.ToString());
                    foreach (var bath in pasrsingList)
                    {
                        var ss = int.Parse(bath);
                        model.neighborhoodId.Add(ss);
                    }
                    model.RealEstateTypeId = item.RealestateTypeId;
                    model.StatusId = item.StatusId;
                    model.IsForSale = item.IsForSale;
                    model.IsForRent = item.IsForRent;
                    model.MinArea = item.MinArea;
                    model.MaxArea = item.MaxArea;
                    model.MaxPrice = item.MaxPrice;
                    model.MaxPrice = item.MaxPrice;
                    model.Date = item.Date;
                    model.AddressNamesAsString = item.AddressNamesAsString;
                    model.Address = item.Address;
                    result.Add(model);
                    model = new CustomerSavedSearchDTO();
                    model.BathRoomCount = new();
                    model.RoomCount = new();
                    model.FeaturesList = new();
                    model.CountryId = new();
                    model.CityId = new();
                    model.RegionId = new();
                    model.neighborhoodId = new();
                }

                return result;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetSavedFilterForCustomer), ex);
                return null;
            }
        }
        #endregion

        #region Leads
        //---------------------------------------------------------------------------------------- 
        public async Task<SyncPaginatedListModel<CustomersListDTO>> GetLeadsAsListAsync(DataManagerRequest dm)
        {
            try
            {
                var user = await _workContext.GetMyUserDetails();
                var employeeId = await _workContext.GetEmployeeIdByUserId(user.Id);
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var query = FindByCondition(c => !c.IsDeleted && c.IsLead).AsQueryable().AsNoTracking()
                   .Select(x => new CustomersListDTO
                   {
                       Id = x.Id,
                       FullName = x.FullName,
                       NationalityName = x.SysNationality.SysNationalityTranslates.FirstOrDefault(a => a.LanguageId == langId) == null ? "" : x.SysNationality.SysNationalityTranslates.FirstOrDefault(a => a.LanguageId == langId).NationalityName,
                       PhoneNumber = x.PhoneNumber,
                       CountryName = x.SysCountry.SysCountryTranslates.FirstOrDefault(a => a.LanguageId == langId) == null ? "" : x.SysCountry.SysCountryTranslates.FirstOrDefault(a => a.LanguageId == langId).CountryName,
                       RegisterdByAdmin = x.IsRegisterdByAdmin ? "Yes" : "No",
                       CreatedBy = x.CreatedBy != null || x.CreatedBy != "" ? x.CreatedBy : "No",
                       Email = x.Email,
                       Facebook = x.Facebook,
                       FullAddress = x.FullAddress,
                       Instagram = x.Instagram,
                       IsLead = x.IsLead,
                       MobileNumber = x.MobileNumber,
                       RegisterDate = x.RegisterDate,
                       TwitterId = x.TwitterId,
                       WhatsappNumber = x.WhatsappNumber,
                   });

                var x = query.ToList();

                if (!await _userManager.IsInRoleAsync(user, PalRoles.SuperAdmin.ToString()) && employeeId > 0)
                    query.Where(a => a.FollowedByEmpId == employeeId);

                var data = await SyncGridOperations<CustomersListDTO>.PagingAndFilterAsync(query, dm);



                foreach (var item in data.Data)
                {
                    var user1 = _context.Users.FirstOrDefault(a => a.ReferenceId == item.Id.ToString() && a.UserType == UserType.Customers);

                    item.LastLogInDate = user1?.LastLogInDate.ToString("dd MMMM yyyy");

                }

                return data;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCustomersAsListAsync), ex);
                return null;
            }

        }
        //------------------------------------------------------------------------------------------
        public async Task<CustomerNoteDTO> SaveCustomerNote(string note, string customerId)
        {
            try
            {
                var userId = _workContext.GetMyUserId();
                var emplyeeId = await _workContext.GetEmployeeIdByUserId(userId);

                var Note = new CustomerNotes
                {
                    AddedByEmpId = emplyeeId,
                    Content = note,
                    CustomerId = long.Parse(customerId),
                    Date = DateTime.Now,
                };
                await _context.CustomerNotes.AddAsync(Note);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(SaveCustomerNote), ActionType = ActionType.Add, TransReferenceId = Note.Id.ToString(), TransType = LogTransType.Note, RelatedTo = Convert.ToInt32(Note.CustomerId) });
                var model = new CustomerNoteDTO { AddedByEmpName = _context.Employees.FirstOrDefault(x => x.Id == emplyeeId).FullName, Content = note, Date = DateTime.Now, Id = Note.Id, CustomerId = Note.CustomerId };
                return model;

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(SaveCustomerNote), ex);
                return null;
            }
        }
        //------------------------------------------------------------------------------------------
        public async Task<string> UpdateCustomerNote(int id, string content)
        {
            try
            {
                var note = _context.CustomerNotes.FirstOrDefault(z => z.Id == id);
                note.Content = content;
                _context.Update(note);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateCustomerNote), ActionType = ActionType.Update, TransReferenceId = note.Id.ToString(), TransType = LogTransType.Note });
                return content;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateCustomerNote), ex);
                return "";
            }
        }
        //------------------------------------------------------------------------------------------
        public async Task<int> DeleteCustomerNote(int id)
        {
            try
            {
                var model = await _context.CustomerNotes.FirstOrDefaultAsync(x => x.Id == id);
                _context.CustomerNotes.Remove(model);
                await _context.SaveChangesAsync();
                return id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteCustomerNote), ex);
                return 0;
            }
        }
        //------------------------------------------------------------------------------------------
        public async Task<List<TimeLineDTO>> GetCustomerTimeLine(int id)
        {
            try
            {
                var result = new List<TimeLineDTO>();
                var model = _context.ActivityLogs.Where(x => x.ActionType == ActionType.Add && x.RelatedTo == id).AsQueryable().AsNoTracking();

                //addning Notes
                result.AddRange(model.Where(z => z.TransType == LogTransType.Note).Select(x => new TimeLineDTO()
                {
                    Date = x.TransDate,
                    CreatedBy = x.UserName,
                    Link = "/Admin/Leads/LeadUpdate/" + x.TransReferenceId,
                    Type = LogTransType.Note,
                    TypeName = LogTransType.Note.ToString(),
                    TypeCount = model.Where(x => x.TransType == LogTransType.Note).Count(),
                }).ToList());

                //addning Calls
                result.AddRange(model.Where(z => z.TransType == LogTransType.Call).Select(x => new TimeLineDTO()
                {
                    Date = x.TransDate,
                    CreatedBy = x.UserName,
                    Link = "/Admin/calls/CallUpdate/" + x.TransReferenceId,
                    Type = LogTransType.Call,
                    TypeName = LogTransType.Call.ToString(),
                    TypeCount = model.Where(x => x.TransType == LogTransType.Call).Count(),
                }).ToList());

                //addning Meetings
                result.AddRange(model.Where(z => z.TransType == LogTransType.Meeting).Select(x => new TimeLineDTO()
                {
                    Date = x.TransDate,
                    CreatedBy = x.UserName,
                    Link = "/Admin/Meetings/MeetingUpdate/" + x.TransReferenceId,
                    Type = LogTransType.Meeting,
                    TypeName = LogTransType.Meeting.ToString(),
                    TypeCount = model.Where(x => x.TransType == LogTransType.Meeting).Count(),
                }).ToList());

                //addning Deals
                result.AddRange(model.Where(z => z.TransType == LogTransType.Deal).Select(x => new TimeLineDTO()
                {
                    Date = x.TransDate,
                    CreatedBy = x.UserName,
                    Link = "/Admin/Deals/DealUpdate/" + x.TransReferenceId,
                    Type = LogTransType.Deal,
                    TypeName = LogTransType.Deal.ToString(),
                    TypeCount = model.Where(x => x.TransType == LogTransType.Deal).Count(),
                }).ToList());


                var resopnse = result.OrderByDescending(x => x.Date).ToList();
                return resopnse;
            }

            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetCustomerTimeLine), ex);
                return null;
            }
        }
        //------------------------------------------------------------------------------------------
        public async Task<CustomerDetailsDTO> CustomerDetails(long id)
        {
            CustomerDetailsDTO model = new CustomerDetailsDTO();

            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                try
                {
                model = _context.Customers.Where(x => x.Id == id).Select(x => new CustomerDetailsDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    NationalityName = x.SysNationality.SysNationalityTranslates.FirstOrDefault(a => a.LanguageId == langId).NationalityName,
                    PhoneNumber = x.PhoneNumber,
                    CountryName = x.SysCountry.SysCountryTranslates.FirstOrDefault(a => a.LanguageId == langId).CountryName,
                    CustomerStatusName = _localization.GetStringResource(x.CustomerStatus.ToString(), langId),
                    Email = x.Email,
                    Facebook = x.Facebook,
                    FullAddress = x.FullAddress,
                    Instagram = x.Instagram,
                    IsLead = x.IsLead,
                    LeadRatingName = x.LeadRating.Translates.FirstOrDefault(w => w.LeadRatingId == x.LeadRatingId && w.LanguageId == langId).RateName,
                    LeadSourceName = x.LeadSource.Translates.FirstOrDefault(w => w.LeadSourceId == x.LeadSourceId && w.LanguageId == langId).SourceName,
                    LeadStatusName = x.LeadStatus.Translates.FirstOrDefault(w => w.LeadStatusId == x.LeadStatusId && w.LanguageId == langId).StatusName,
                    MobileNumber = x.MobileNumber,
                    RegisterDate = x.RegisterDate,
                    TwitterId = x.TwitterId,
                    WhatsappNumber = x.WhatsappNumber,
                    FollowedByEmpName = _context.Employees.FirstOrDefault(a => a.Id == x.FollowedByEmpId).FullName,
                    RegionName = _context.SysRegionTranslates.FirstOrDefault(a => a.RegionId == x.RegionId && a.LanguageId == langId).RegionName,
                    CityName = _context.SysCityTranslates.FirstOrDefault(a => a.CityId == x.CityId && a.LanguageId == langId).CityName,
                    CreationDate = x.CreationDate,
                    RegisterdByAdmin = x.IsRegisterdByAdmin,

                }).FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                var user = _context.Users.FirstOrDefault(a => a.UserType == UserType.Customers && a.ReferenceId == model.Id.ToString());
                model.RegisterdByAdmin = user == null? false : user.RegisterdByAdmin;
                if (user != null)
                    model.LastLogInDate = user.LastLogInDate;

                // todo
                model.Calls = (await _callService.GetAllAsync(new DataManagerRequest())).DataSolo.DoneCalls.Where(x => x.CustomerId == id).OrderByDescending(a => a.CallStart).ToList();
                model.Meetings = (await _meetingService.GetAllAsync(new DataManagerRequest())).Data.Where(x => x.CustomerId == id).OrderByDescending(a => a.FromDateTime).ToList();
                //model.Deals = (await _dealsService.GetAllAsync(new DataManagerRequest())).Data.Where(x => x.CustomerId == id).OrderByDescending(a => a.Id).ToList();
                model.Tasks = (await _tasksService.GetAllAsListAsync(new DataManagerRequest())).Data.Where(x => x.CustomerId == id).ToList();
                model.Favorites = (await GetFavoritesAsync(langId));
                ////model.Tours = (await _tourService.GetTourListByCustomerId(id)).OrderByDescending(a => a.Id).ToList();
                model.Notes = await _context.CustomerNotes.Where(x => x.CustomerId == id).Select(x => new CustomerNoteDTO { AddedByEmpName = _context.Employees.FirstOrDefault(a => a.Id == x.AddedByEmpId).FullName, Content = x.Content, Date = x.Date, Id = x.Id, CustomerId = x.CustomerId }).OrderByDescending(a => a.Id).ToListAsync();

                return model;
            }
            catch (Exception ex)
            { await _logger.LogErrorAsync(nameof(CustomerDetails), ex);
                return null;
            }
        }
        //------------------------------------------------------------------------------------------
        public async Task<bool> ConvertLeadToCustomer(long id)
        {
            try
            {
                var lead = await _context.Customers.FindAsync(id);
                if (lead == null)
                    return false;

                lead.IsLead = false;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(ConvertLeadToCustomer), ex);
                return false;
            }
        }
        //-----------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public async Task<bool> ConvertCustomerToLead(long id)
        {
            try
            {
                var lead = await _context.Customers.FindAsync(id);
                if (lead == null)
                    return false;

                lead.IsLead = true;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync("CustomersController" + nameof(ConvertCustomerToLead), ex);
                return false;
            }
        }

        //Task<List<CustomerFavoriteViewModel>> ICustomersService.GetFavoritesProjectAsync(int? languageId)
        //{
        //    throw new NotImplementedException();
        //}
        //-----------------------------------------------------------------------------------------
        #endregion
    }

}
