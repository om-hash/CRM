using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Activity_logs;
using Pal.Core.Domains.Deals;
using Pal.Core.Enums;
using Pal.Core.Enums.ActivityLog;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Deals;
using Pal.Services.Caching;
using Pal.Services.FileManager;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using Syncfusion.EJ2.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pal.Services.CRM.Deals
{
    public class DealsSerivce : RepositoryBase<Deal>, IDealsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebWorkContext _workContext;
        private readonly IFileManagerService _fileManager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheService<Deal> _cacheService;
        private readonly ILanguageService _languageService;

        public DealsSerivce(ApplicationDbContext context, IWebWorkContext workContext, ILanguageService languageService,
          IFileManagerService fileManager, IMapper mapper,
          ILoggerService logger, IHttpContextAccessor httpContextAccessor, 
          ICacheService<Deal> cacheService  ) : base(context)
        {
            _context = context;
            _workContext = workContext;
            _fileManager = fileManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _cacheService = cacheService;
            _languageService = languageService;
        }
        public async Task<SyncPaginatedListModel<DealListDTO>> GetAllAsync(DataManagerRequest dm)
        {

            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var query = FindAll()
                    .Where(a => !a.IsDeleted)
                    .Select(a => new DealListDTO
                    {

                        Amount = a.Amount,
                        CustomerId = a.CustomerId,
                        ClosingDate = a.ClosingDate,
                        Customer = a.Customer == null ? "" : a.Customer.FullName,
                        DealName = a.DealName,
                        Description = a.Description,
                        Employee = a.Employee.FullName,
                        Id = a.Id,
                        LeadSourceString = a.LeadSource.Translates.FirstOrDefault(x => x.LanguageId == langId).SourceName,
                        NextStep = a.NextStep,
                        StageName = a.DealStage.Translates.FirstOrDefault(x => x.LanguageId == langId).StageName,
                        SuccessProbability = a.SuccessProbability,
                        TypeString = a.DealType.Translates.FirstOrDefault(x => x.LanguageId == langId).TypeName,

                    });

                return await SyncGridOperations<DealListDTO>.PagingAndFilterAsync(query, dm);
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAllAsync), ex);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<int> AddAsync(DealDTO model)
        {
            try
            {
                _cacheService.Delete("GetDealAsLookupCacheKey");
                var Deal = _mapper.Map<Deal>(model);
                _context.Add(Deal);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(model: new ActivityLog { ActionName = nameof(AddAsync), ActionType = ActionType.Add, TransReferenceId = model.Id.ToString(), TransType = LogTransType.Deal, RelatedTo = Convert.ToInt32(Deal.CustomerId) });
                return Deal.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(AddAsync), ex);
                return 0;
            }
        }

        //----------------------------------------------------------------------------------------
        public async Task<ResponseType> DeleteAsync(int id)
        {
            using var transaction = _context.BeginTransaction();

            try
            {
                _cacheService.Delete("GetDealAsLookupCacheKey");
                var meeting = await _context.Deals.FirstOrDefaultAsync(a => a.Id == id);
                meeting.IsDeleted = true;
                _context.Deals.Update(meeting);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(DeleteAsync), ActionType = ActionType.Delete, TransReferenceId = meeting.Id.ToString(), TransType = LogTransType.Deal });
                return ResponseType.Success;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteAsync), ex);
                await transaction.RollbackAsync();
                return ResponseType.Error;
            }
        }

        //----------------------------------------------------------------------------------------
        public async Task<DealDTO> GetDealByIdAsync(int id)
        {
            try
            {

                var meeting = await _context.Deals.FirstOrDefaultAsync(a => a.Id == id);

                if (meeting == null)
                    return null;
                //_context.SystemErrors.Add(error);
                var model = _mapper.Map<DealDTO>(meeting);
                var sdfjk = _context.Customers.ToList();
                var asdkjal = _context.Customers.FirstOrDefault(a => a.Id == model.CustomerId);
                model.CustomerName = _context.Customers.Where(x => x.Id == model.CustomerId).FirstOrDefault().FullName.ToString();
                var languages = _languageService.GetAllLanguages();
                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetDealByIdAsync), ex);
                return null;
            }
        }

        //----------------------------------------------------------------------------------------
        public async Task<int> UpdateAsync(DealDTO model)
        {
            try
            {
                _cacheService.Delete("GetDealAsLookupCacheKey");

                var meeting = _mapper.Map<Deal>(model);
                _context.Update(meeting);
                await _context.SaveChangesAsync();
                await _logger.LogInfoAsync(new ActivityLog { ActionName = nameof(UpdateAsync), ActionType = ActionType.Update, TransReferenceId = meeting.Id.ToString(), TransType = LogTransType.Deal });
                return meeting.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateAsync), ex);
                return 0;
            }
        }
        //----------------------------------------------------------------------------------------
        public async Task<DealDetialsDTO> GetDealDetails(int id)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var deal = await _context.Deals.Include(z => z.Customer).Where(a => a.Id == id).Select(x => new DealDetialsDTO
                {
                    Amount = x.Amount,
                    ClosingDate = x.ClosingDate,
                    CustomerAddress = x.Customer.FullAddress,
                    CustomerName = x.Customer.FullName,
                    DealName = x.DealName,
                    Description = x.Description,
                    CustomerEmail = x.Customer.Email,
                    Employee = x.Employee.FullName,
                    Id = x.Id,
                    LeadSourceString = x.LeadSource.Translates.FirstOrDefault(a => a.LanguageId == langId).SourceName,
                    NextStep = x.NextStep,
                    PhoneNumber = x.Customer.PhoneNumber,
                    StageName = x.DealStage.Translates.FirstOrDefault(x => x.LanguageId == langId).StageName,
                    SuccessProbability = x.SuccessProbability,
                    TypeString = x.DealType.Translates.FirstOrDefault(a => a.LanguageId == langId).TypeName,
                    StageId = x.StageId,
                    CustomerId = Convert.ToInt32(x.CustomerId),
                }).FirstOrDefaultAsync();

                return deal;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetDealDetails), ex);
                return null;
            }
        }
        public async Task<bool> ChangeDealStage(int id, int stageID)
        {
            try
            {
                var model = await _context.Deals.FirstOrDefaultAsync(z => z.Id == id);
                model.StageId = stageID;
                _context.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(ChangeDealStage), ex);
                return false;
            }
        }

    }
}
