using Pal.Core.Domains.Lookups;
using Pal.Core.Enums.Account;
using Pal.Core.VMs;
using Pal.Data.VMs.Address;
using Pal.Data.VMs.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Pal.Services.DataServices.Lookups.LookupsService;

namespace Pal.Services.DataServices.Lookups
{
    public interface ILookupsService
    {
        #region CP Methods
        Task<List<ComboboxModel>> GetSysPaymentType();
        Task<List<ComboboxModel>> GetSysFeatures(int? languageId);
        Task<List<ComboboxModel>> GetSysProjectStatus(int? languageId);
        Task<List<ComboboxModel>> GetCompaniesAsLookup();
        Task<List<ComboboxModel>> GetSalesAsLookup();
        Task<List<ComboboxModel>> GetProjectsAsLookup();
        Task<List<ComboboxModel>> GetProjectsAsLookup(int CompId);
        Task<List<ComboboxModel>> GetSysRealEstateTypesByClassId(int classId);
        Task<List<ComboboxModel>> GetSysRealEstateTypes(int? languageId);
        Task<List<ComboboxModel>> GetSysRealEstateClasses();
        Task<List<ComboboxModel>> GetSysRoomsCounts(int? languageId);
        Task<List<ComboboxModel>> GetSysHeatingTypes();
        Task<List<ComboboxModel>> GetSysAirConditionTypes();
        Task<List<ComboboxModel>> GetSysNationalities();
        Task<List<ComboboxModel>> GetSysCountries();
        Task<List<ComboboxModel>> GetLeadSources();
        Task<List<ComboboxModel>> GetLeadStatus();
        Task<List<ComboboxModel>> GetLeadRating();
        //Task<List<ComboboxModel>> GetRealEstateFeautres();
        Task<AddressSearchComboboxModel> GetAddressByType(int id, ItemType type);

        Task<List<ComboboxModel>> GetSysProjectType();
        Task<List<ComboboxModel>> GetAdvisorsAsLookups();

        Task<FullAddressViewModel> GetAddressLookups();
        Task<dynamic> GetUsersAsLookup(int userType, string userId);
        #endregion

        #region Project lowest and highest value
        Task<float> GetSysLowestMeterAvragePrice();
        Task<float> GetSysHighestMeterAvragePrice();
        Task<float> GetSysHighestRealEstateAvgPrice();
        Task<float> GetSysLowestRealEstateAvgPrice();
        Task<float> GetSysHighestProjectArea();
        Task<float> GetSysLowestProjectArea();
        #endregion

        #region RealEstate lowest and highest value
        Task<float> GetSysLowestRealEstatePrice();
        Task<float> GetSysHighestRealEstatePrice();
        Task<float> GetSysHighestRealEstateArea();
        Task<float> GetSysLowestRealEstateArea();
        #endregion

        #region Address 

        string GetAddressByNeighborhoodId(int id);

        string GetAddressByNeighborhoodIdAllLanguages(int neighborhoodId);

        List<ComboboxModel> GetSysWorkHours();
        Task<List<ComboboxModel>> GetSysPlacesTypes();
        Task<List<ComboboxModel>> GetSysViewType();
        Task<List<ComboboxModel>> GetSysBlaconieDirection();
        Task<List<ComboboxModel>> GetSysCompanyCategories();
        Task<List<ComboboxModel>> GetSyssSalesCategories();
        Task<List<ComboboxModel>> GetSysCity(string text = "");
        Task<List<AddressSearchComboboxModel>> GetSysCountries(string text = "");
        Task<List<AddressSearchComboboxModel>> GetSysCities(string text = "");
        Task<List<AddressSearchComboboxModel>> GetSysRegions(string text = "");
        Task<List<AddressSearchComboboxModel>> GetSysNeighborhoods(string text = "");
        #endregion

        #region Address Without Chaching 

        public Task<List<ComboboxModel>> GetCountries();

        public Task<List<ComboboxModel>> GetCitiesByCountryId(int id);

        public Task<List<ComboboxModel>> GetRegionsByCityId(int id);

        public Task<List<NeighborhoodGroubingModel>> GetNeighborhoodsByRegionId(int id);
        Task<List<ComboboxModel>> GetEmployeeAsLookup();
        Task<List<ComboboxModel>> GetCustomerAsLookup();
        Task<List<ComboboxModel>> GetCallType();
        Task<List<ComboboxModel>> GetCallStatus();
        Task<List<ComboboxModel>> GetCallPurpose();
        Task<List<ComboboxModel>> GetCallResult();
        Task<List<ComboboxModel>> GetDealAsLookup();
        Task<List<ComboboxModel>> GetDealStage();
        Task<List<ComboboxModel>> GetDealType();

        #endregion

        #region CRM
        Task<List<ComboboxModel>> GetSysTaskStatuses();
        Task<List<ComboboxModel>> GetSysJobTitles();
        Task<List<ComboboxModel>> GetLeadsAsLookup();
        Task<List<ComboboxModel>> GetRealEstatesAsLookup();
        Task<List<ComboboxModel>> GetDecisionAsLookup();
        #endregion
    }
}
