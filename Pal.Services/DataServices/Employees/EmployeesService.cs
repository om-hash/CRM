using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains.Account;
using Pal.Core.Domains.Empolyees;
using Pal.Core.Enums;
using Pal.Core.Enums.Account;
using Pal.Data.Contexts;
using Pal.Data.DTOs;
using Pal.Data.DTOs.Employees;
using Pal.Services.Caching;
using Pal.Services.Languages;
using Pal.Services.Logger;
using Pal.Services.WebWorkContext;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Pal.Services.DataServices.Employees
{
    public class EmployeesService : IEmployeesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IWebWorkContext _webWorkContext;
   
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService<Employee> _cacheService;
        private readonly ILanguageService _languageService;
        public EmployeesService(ApplicationDbContext context, IWebWorkContext webWorkContext,
            UserManager<ApplicationUser> userManager, IMapper mapper, ILoggerService logger,
   ICacheService<Employee> cacheService, ILanguageService languageService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
    
            _userManager = userManager;
            _cacheService = cacheService;
            _languageService = languageService;
            _webWorkContext = webWorkContext;
        }

        //----------------------------------------------------------- xxx
        public async Task<SyncPaginatedListModel<EmployeesList>> GetAllAsListAsync(DataManagerRequest dm)
        {
            try
            {
                var langId = await _languageService.GetLanguageIdFromRequestAsync();
                var query = _context.Employees.AsNoTracking().Where(e => !e.IsDeleted && 
                _userManager.Users.Where(u => !u.IsDeleted).Select(w => w.ReferenceId).ToList().Contains(e.Id.ToString()))
                    .Select(x => new EmployeesList
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        JobTitle = x.JobTitle.Translates.FirstOrDefault(jt => jt.LanguageId == langId) != null ? x.JobTitle.Translates.FirstOrDefault(jt => jt.LanguageId == langId).JobTitleName : "",
                        PhoneNumber = x.PhoneNumber,
                    });


                return await SyncGridOperations<EmployeesList>.PagingAndFilterAsync(query, dm);

            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetAllAsListAsync), ex);
                return null;
            }
        }

        public async Task<List<Employee>> getEmployees()
        {
            var result = await _context.Employees.Where(e => !e.IsDeleted).ToListAsync();

            return result;
        }
        //-----------------------------------------------------------
        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
                var model = _mapper.Map<EmployeeDTO>(entity);

                return model;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetEmployeeByIdAsync), ex);
                return null;
            }
        }
        //-----------------------------------------------------------
        public async Task<int> GetEmployeeIdByUerIdAsync(string id)
        {
            try
            {
                int empId = (int)(await _context.Employees.FirstOrDefaultAsync(e => e.UserId == id))?.Id;

                return empId;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(GetEmployeeByIdAsync), ex);
                return 0;
            }
        }

        //-----------------------------------------------------------
        public async Task<int> AddEmployeeAsync(EmployeeDTO model)
        {
            try
            {
                _cacheService.Delete("GetEmployeeAsLookupCacheKey");
                var entity = _mapper.Map<Employee>(model);

                entity.CreatedBy = (await _webWorkContext.GetMyUserDetails()).FullName;
                entity.DeletedBy = "";
                entity.ModifiedBy = "";

                await _context.Employees.AddAsync(entity);
                await _context.SaveChangesAsync();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
                if (user.UserType != UserType.Adviser && user.UserType != UserType.Lawyer)
                {
                    user.ReferenceId = entity.Id.ToString();

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    //var claims = new List<Claim>
                    //{
                    //    new Claim(type: PalClaimType.EmployeeId.ToString(), value: entity.Id.ToString()),
                    //};
                    //await _userManager.AddClaimsAsync(user, claims);
                }

                return entity.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(AddEmployeeAsync), ex);
                return 0;
            }
        }

        //-----------------------------------------------------------
        public async Task<int> UpdateEmployeeAsync(EmployeeDTO model)
        {
            try
            {
                _cacheService.Delete("GetEmployeeAsLookupCacheKey");
                var entity = _mapper.Map<Employee>(model);

                _context.Employees.Update(entity);
                await _context.SaveChangesAsync();

                return entity.Id;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(UpdateEmployeeAsync), ex);
                return 0;
            }
        }

        //-----------------------------------------------------------
        public async Task<ResponseType> DeleteEmployeeAsync(int id)
        {
            try
            {
                if (await _context.Tasks.AnyAsync(t => t.EmployeeId == id && !t.IsDeleted) || await _context.ChatInbox.AnyAsync(t => t.EmployeeId == id))
                    return ResponseType.Error;

                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

                employee.IsDeleted = true;

                _context.Employees.Update(employee);

                await _context.SaveChangesAsync();

                // delete employee from cache
                _cacheService.Delete("GetEmployeeAsLookupCacheKey");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == employee.UserId);
                if (user != null)
                {
                    if (await _context.Employees.AnyAsync(e => e.UserId == user.Id && !e.IsDeleted))
                        return ResponseType.Success;

                    user.ReferenceId = null;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                return ResponseType.Success;
            }
            catch (Exception ex)
            {
                _ = _logger.LogErrorAsync(nameof(DeleteEmployeeAsync), ex);
                return ResponseType.Error;
            }
        }

    }
}
