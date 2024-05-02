using Microsoft.AspNetCore.Http;
using Pal.Core.Enums;
using Pal.Core.Enums.Customer;
using Pal.Data.DTOs;
using Pal.Data.DTOs.Customer;
using Syncfusion.EJ2.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.Customers
{
    public interface ICustomersService
    {
        Task<long> AddAsync(CustomerDTO model);
        Task<ResponseType> DeleteAsync(long id);
        Task<SyncPaginatedListModel<CustomersListDTO>> GetCustomersAsListAsync(DataManagerRequest dm);
        Task<CustomerDTO> GetByIdAsync(long id);
        Task<string> GetImageByIdAsync();
        Task<long> UpdateAsync(long id, CustomerDTO model);
        Task<string> GetCustomerName(int v);
        Task<bool> UpdateCustomerImage(string imageUrl);
        Task<bool> AddToFavoriteAsync(CustomerFavoriteDTO model);

        Task<List<CustomerFavoriteViewModel>> GetFavoritesAsync(int? languageId);
        //Task<List<CustomerFavoriteViewModel>> GetFavoritesProjectAsync(int? languageId);
        Task UpdateCustomerProfile(long custId, CustomerDTO model);
        Task<bool> DeleteFromFavorite(int id, CustomerFavoriteType ReferenceType,long custId);
        Task<bool> IsAddedToFavorite(int Id, CustomerFavoriteType referenceType);


        Task<SyncPaginatedListModel<CustomersListDTO>> GetLeadsAsListAsync(DataManagerRequest dm);
        Task<CustomerNoteDTO> SaveCustomerNote(string note, string CustomerId);
        Task<string> UpdateCustomerNote(int id, string content);
        Task<int> DeleteCustomerNote(int id);
        Task<List<TimeLineDTO>> GetCustomerTimeLine(int id);
        Task<CustomerDetailsDTO> CustomerDetails(long id);
        Task<bool> ConvertLeadToCustomer(long id);

        Task<long> GetCustomerdByUserId(string userId);
        Task<bool> ConvertCustomerToLead(long id);
        Task<CustomersService.AllFavoritesDTO> GetAllFavorites(int? languageId);
        Task<bool> SaveCustomerSearch(CustomerSavedSearchDTO model);
        Task<List<CustomerSavedSearchDTO>> GetSavedFilterForCustomer();
    }
}