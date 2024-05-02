using DocumentFormat.OpenXml.Office2016.Excel;
using Pal.Core.Domains.Lookups;
using Pal.Data.DTOs;
using Pal.Data.DTOs.CRM.Lookups;
using Pal.Data.DTOs.Lookups;
using Syncfusion.EJ2.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.LookupsCRUDService
{
    public interface ILookupsCRUDService
    {

        #region Air Condition Types

        Task<SysAirConditionTypeDTO> AirConditionTypesAdd(SysAirConditionTypeDTO model);
        Task<bool> AirConditionTypesDelete(int id);
        Task<List<SysAirConditionTypeDTO>> AirConditionTypesList();
        Task<SysAirConditionTypeDTO> AirConditionTypesUpdate( SysAirConditionTypeDTO model);

        #endregion

        #region Heating Types

        Task<SysHeatingTypeDTO> HeatingTypesAdd(SysHeatingTypeDTO model);
        Task<bool> HeatingTypesDelete(int id);
        Task<List<SysHeatingTypeDTO>> HeatingTypesList();
        Task<SysHeatingTypeDTO> HeatingTypesUpdate(SysHeatingTypeDTO model);

        #endregion

        #region Nationality

        Task<SysNationalityDTO> NationalityAdd(SysNationalityDTO model);
        Task<bool> NationalityDelete(int id);
        Task<List<SysNationalityDTO>> NationalitiesList();
        Task<SysNationalityDTO> NationalityUpdate(SysNationalityDTO model);

        #endregion

        #region Payment Types

        Task<SysPaymentTypesDTO> PaymentTypesAdd(SysPaymentTypesDTO model);
        Task<bool> PaymentTypesDelete(int id);
        Task<List<SysPaymentTypesDTO>> PaymentTypesList();
        Task<SysPaymentTypesDTO> PaymentTypesUpdate(SysPaymentTypesDTO model);

        #endregion

        #region Project Features

        Task<SysProjectFeatureDTO> ProjectFeaturesAdd(SysProjectFeatureDTO model);
        Task<bool> ProjectFeaturesDelete(int id);
        Task<List<SysProjectFeatureDTO>> ProjectFeaturesList();
        Task<SysProjectFeatureDTO> ProjectFeaturesUpdate(SysProjectFeatureDTO model);

        #endregion

        #region Project Status

        Task<SysProjectStatusDTO> ProjectStatusAdd(SysProjectStatusDTO model);
        Task<bool> ProjectStatusDelete(int id);
        Task<List<SysProjectStatusDTO>> ProjectStatusList();
        Task<SysProjectStatusDTO> ProjectStatusUpdate(SysProjectStatusDTO model);

        #endregion

        #region Project Types

        Task<SysProjectTypeDTO> ProjectTypesAdd(SysProjectTypeDTO model);
        Task<bool> ProjectTypesDelete(int id);
        Task<List<SysProjectTypeDTO>> ProjectTypesList();
        Task<SysProjectTypeDTO> ProjectTypesUpdate(SysProjectTypeDTO model);

        #endregion

        #region RealEstate Types

        Task<SysRealEstateTypeDTO> RealEstateTypesAdd(SysRealEstateTypeDTO model);
        Task<bool> RealEstateTypesDelete(int id);
        Task<List<SysRealEstateTypeDTO>> RealEstateTypesList();
        Task<SysRealEstateTypeDTO> RealEstateTypesUpdate(SysRealEstateTypeDTO model);

        #endregion

        #region RealStateClass

        Task<SysRealStateClassDTO> RealStateClassAdd(SysRealStateClassDTO model);
        Task<bool> RealStateClassDelete(int id);
        Task<List<SysRealStateClassDTO>> RealStateList();
        Task<SysRealStateClassDTO> RealStateClassUpdate(SysRealStateClassDTO model);

        #endregion

        #region Rooms Count

        Task<SysRoomsCountDTO> RoomsCountAdd(SysRoomsCountDTO model);
        Task<bool> RoomsCountDelete(int id);
        Task<List<SysRoomsCountDTO>> RoomsCountList();
        Task<SysRoomsCountDTO> RoomsCountUpdate(SysRoomsCountDTO model);

        #endregion

        #region City
        Task<List<ComboboxModel>> GetSysCities(int countryId);
        Task<SysCityDTO> CityAdd(SysCityDTO model);
        Task<bool> CityDelete(int id);
        Task<List<SysCityDTO>> CityList();
        Task<SysCityDTO> CityUpdate(SysCityDTO model);

        #endregion

        #region Country
        Task<List<ComboboxModel>> GetSysCountries();
        Task<SysCountryDTO> CountryAdd(SysCountryDTO model);
        Task<bool> CountryDelete(int id);
        Task<List<SysCountryDTO>> CountryList();
        Task<SysCountryDTO> CountryUpdate(SysCountryDTO model);

        #endregion

        #region Region
        Task<List<ComboboxModel>> GetSysRegions();
        Task<SysRegionDTO> RegionAdd(SysRegionDTO model);
        Task<bool> RegionDelete(int id);
        Task<SyncPaginatedListModel<SysRegionDTO>> RegionList(DataManagerRequest dm);
        Task<SysRegionDTO> RegionUpdate(SysRegionDTO model);

        #endregion

        #region Neighborhood

        Task<SysNeighborhoodDTO> NeighborhoodAdd(SysNeighborhoodDTO model);
        Task<bool> NeighborhoodDelete(int id);
        Task<SyncPaginatedListModel<SysNeighborhoodDTO>> NeighborhoodList(DataManagerRequest dm);
        Task<SysNeighborhoodDTO> NeighborhoodUpdate(SysNeighborhoodDTO model);

        #endregion

        Task<SysPlacesTypeDTO> PlacesTypeAdd(SysPlacesTypeDTO model);
        Task<bool> PlacesTypeDelete(int id);
        Task<List<SysPlacesTypeDTO>> PlacesTypeList();
        Task<SysPlacesTypeDTO> PlacesTypeUpdate(SysPlacesTypeDTO model);
        Task<BalconieDirectionDTO> BalconieDirectionUpdate(BalconieDirectionDTO model);
        Task<List<BalconieDirectionDTO>> BalconieDirectionList();
        Task<bool> BalconieDirectionDelete(int id);
        Task<BalconieDirectionDTO> BalconieDirectionAdd(BalconieDirectionDTO model);
        Task<SysViewTypeDTO> ViewTypeAdd(SysViewTypeDTO model);
        Task<bool> ViewTypeDelete(int id);
        Task<List<SysViewTypeDTO>> ViewTypeList();
        Task<SysViewTypeDTO> ViewTypeUpdate(SysViewTypeDTO model);
        Task<SysSalesCategoryDTO> CompanyCategoryAdd(SysSalesCategoryDTO model);
        Task<bool> CompanyCategoryDelete(int id);
        Task<List<SysSalesCategoryDTO>> CompanyCategoryList();
        Task<SysSalesCategoryDTO> CompanyCategoryUpdate(SysSalesCategoryDTO model);


        #region CRM

        #region TaskStatus

        Task<SysTaskStatusDTO> TaskStatusAdd(SysTaskStatusDTO model);
        Task<bool> TaskStatusDelete(int id);
        Task<List<SysTaskStatusDTO>> TaskStatusList();
        Task<SysTaskStatusDTO> TaskStatusUpdate(SysTaskStatusDTO model);
        Task<List<CallTypeDTO>> CallTypeList();
        Task<CallTypeDTO> CallTypeAdd(CallTypeDTO model);
        Task<CallTypeDTO> CallTypeUpdate(CallTypeDTO model);
        Task<bool> CallTypeDelete(int id);
        Task<List<CallStatusDTO>> CallStatusList();
        Task<CallStatusDTO> CallStatusAdd(CallStatusDTO model);
        Task<CallStatusDTO> CallStatusUpdate(CallStatusDTO model);
        Task<bool> CallStatusDelete(int id);
        Task<List<CallPurposeDTO>> CallPurposeList();
        Task<CallPurposeDTO> CallPurposeAdd(CallPurposeDTO model);
        Task<CallPurposeDTO> CallPurposeUpdate(CallPurposeDTO model);
        Task<bool> CallPurposeDelete(int id);
        Task<List<CallResultDTO>> CallResultList();
        Task<CallResultDTO> CallResultAdd(CallResultDTO model);
        Task<CallResultDTO> CallResultUpdate(CallResultDTO model);
        Task<bool> CallResultsDelete(int id);
        Task<List<DealStageDTO>> DealStageList();
        Task<DealStageDTO> DealStageAdd(DealStageDTO model);
        Task<DealStageDTO> DealStageUpdate(DealStageDTO model);
        Task<bool> DealStageDelete(int id);
        Task<List<DealTypeDTO>> DealTypeList();
        Task<DealTypeDTO> DealTypeAdd(DealTypeDTO model);
        Task<DealTypeDTO> DealTypeUpdate(DealTypeDTO model);
        Task<bool> DealTypeDelete(int id);

        #endregion

        #region JobTitle
        Task<List<SysJobTitleDTO>> JobTitleList();
        Task<SysJobTitleDTO> JobTitleAdd(SysJobTitleDTO model);
        Task<SysJobTitleDTO> JobTitleUpdate(SysJobTitleDTO model);
        Task<bool> JobTitleDelete(int id);
        Task<List<LeadSourceDTO>> LeadSourceList();
        Task<LeadSourceDTO> LeadSourceAdd(LeadSourceDTO model);
        Task<bool> LeadSourceDelete(int id);
        Task<List<LeadRatingDTO>> LeadRatingList();
        Task<LeadRatingDTO> LeadRatingAdd(LeadRatingDTO model);
        Task<LeadRatingDTO> LeadRatingUpdate(LeadRatingDTO model);
        Task<List<LeadStatusDTO>> LeadStatusList();
        Task<LeadStatusDTO> LeadStatusAdd(LeadStatusDTO model);
        Task<LeadStatusDTO> LeadStatusUpdate(LeadStatusDTO model);
        Task<bool> LeadStatusDelete(int id);
        Task<LeadSourceDTO> LeadSourceUpdate(LeadSourceDTO model);
        Task<bool> LeadRatingDelete(int id);
        Task<bool> RemoveAllLookupKeys();
        #endregion

        #region Decision
        Task<List<DecisionDTO>> DecisionList();
        Task<DecisionDTO> DecisionAdd(DecisionDTO model);
        Task<DecisionDTO> DecisionUpdate(DecisionDTO model);
        Task<bool> DecisionDelete(int id);
        #endregion

        #endregion
    }
}