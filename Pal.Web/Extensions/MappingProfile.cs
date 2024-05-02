
using AutoMapper;

using Pal.Core.Domains.Chat;
using Pal.Core.Domains.Customers;
using Pal.Core.Domains.Languages;
using Pal.Core.Domains.Lookups;
using Pal.Core.Domains.Notifications;

using Pal.Data.DTOs.AboutUs;
using Pal.Data.DTOs.Advisors;
using Pal.Data.DTOs.Blogs;
using Pal.Data.DTOs.Chat;
using Pal.Data.DTOs.Company;
using Pal.Data.DTOs.ContactUs;
using Pal.Data.DTOs.Customer;
using Pal.Data.DTOs.GeneralInfo;
using Pal.Data.DTOs.Languages;
using Pal.Data.DTOs.Lookups;
using Pal.Data.DTOs.Notifications;
using Pal.Data.DTOs.Project;
using Pal.Data.DTOs.RealEstate;
using Pal.Data.DTOs.Region;
using Pal.Data.DTOs.RatingAndComment;
using Pal.Core.Domains.Empolyees;
using Pal.Data.DTOs.Employees;
using Pal.Core.Domains.OurServices;
using Pal.Data.DTOs.OurServices;
using Pal.Data.DTOs.Lawyer;
using System.Threading.Tasks;
using Pal.Data.DTOs.CRM.Task;
using Pal.Core.Domains.Communications;
using Pal.Data.DTOs.CRM.Meeting;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Data.DTOs.CRM.Lookups;

using Pal.Core.Domains.Deals;

using Pal.Data.DTOs.CRM.Call;
using Pal.Data.DTOs.CRM.Deals;
using Pal.Data.DTOs.Reels;
using Pal.Core.Domains.Companies;

namespace Pal.Web.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region Region
            CreateMap<SysRegion, CreateNewRegionDTO>().ReverseMap();
            CreateMap<SysRegionTranslate, CreateNewRegionTranslateDTO>().ReverseMap();
            //CreateMap<SysRegionSite, SysRegionSiteDTO>().ReverseMap();

            #endregion


            #region Customers
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerNotes, CustomerNoteDTO>().ReverseMap();
            CreateMap<CustomerFavorite, CustomerFavoriteDTO>().ReverseMap();
            #endregion


            #region Lookups
            //CreateMap<SysAirConditionType, SysAirConditionTypeDTO>().ReverseMap();
            //CreateMap<SysHeatingType, SysHeatingTypeDTO>().ReverseMap();
            CreateMap<CreateNewCompanyDTO, Company>().ReverseMap();
            CreateMap<CreateNewCompanyTranslateDTO, CompanyTranslate>().ReverseMap();
            CreateMap<SysCompanyCategory, SysSalesCategoryDTO>().ReverseMap();
            CreateMap<SysNationality, SysNationalityDTO>().ReverseMap();
            CreateMap<SysPaymentType, SysPaymentTypesDTO>().ReverseMap();
            //CreateMap<SysProjectFeature, SysProjectFeatureDTO>().ReverseMap();
            //CreateMap<SysProjectStatus, SysProjectStatusDTO>().ReverseMap();
            //CreateMap<SysProjectType, SysProjectTypeDTO>().ReverseMap();
            //CreateMap<SysRealEstateClass, SysRealStateClassDTO>().ReverseMap();
            //CreateMap<SysRealEstateType, SysRealEstateTypeDTO>().ReverseMap();
            //CreateMap<SysRoomsCount, SysRoomsCountDTO>().ReverseMap();
            CreateMap<SysCountry, SysCountryDTO>().ReverseMap();
            CreateMap<SysCity, SysCityDTO>().ReverseMap();
            CreateMap<SysRegion, SysRegionDTO>().ReverseMap();
            CreateMap<SysNeighborhood, SysNeighborhoodDTO>().ReverseMap();
            //CreateMap<SysPlacesType, SysPlacesTypeDTO>().ReverseMap();
            CreateMap<SysViewType, SysViewTypeDTO>().ReverseMap();
            //CreateMap<SysBalconieDirection, BalconieDirectionDTO>().ReverseMap();
            #endregion

            #region Languages
            CreateMap<Language, LanguageCreateDTO>().ReverseMap();
            CreateMap<LanguageStringResource, LanguageCreateDTO>().ReverseMap();
            #endregion

            #region Notifications
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<NotificationParam, NotificationParamDTO>().ReverseMap();
            #endregion

    

            #region Chat
            CreateMap<ChatMessage, ChatMessageDTO>().ReverseMap();
            #endregion

            #region Employee
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            #endregion

            #region OurServices
            CreateMap<OurServices, OurSerivesDTO>().ReverseMap();
            CreateMap<OurServicesTranslate, OurSerivesTranslateDTO>().ReverseMap();
            #endregion

            #region CRM 

            CreateMap<Pal.Core.Domains.Tasks.Task, TaskCreateDTO>().ReverseMap();
            CreateMap<Meeting, MeetingDTO>().ReverseMap();
            CreateMap<Call, CallDTO>().ReverseMap();


            CreateMap<Deal, DealDTO>().ReverseMap();

            #region Lookups

            CreateMap<SysTaskStatus, SysTaskStatusDTO>().ReverseMap();

            CreateMap<SysDecision, DecisionDTO>().ReverseMap();
            CreateMap<ComboboxModelTranslate, SysDecisionTranslate>().ReverseMap();

            CreateMap<SysCallType, CallTypeDTO>().ReverseMap();
            CreateMap<SysCallTypeTranslate, ComboboxModelTranslate>().ReverseMap();

            CreateMap<SysCallStatus, CallStatusDTO>().ReverseMap();
            CreateMap<SysCallStatusTranslate, ComboboxModelTranslate>().ReverseMap();

            CreateMap<SysCallPurpose, CallPurposeDTO>().ReverseMap();
            CreateMap<SysCallPurposeTranslate, ComboboxModelTranslate>().ReverseMap();

            CreateMap<SysCallResult, CallResultDTO>().ReverseMap();
            CreateMap<SysCallResultTranslate, ComboboxModelTranslate>().ReverseMap();

            CreateMap<SysDealStage, DealStageDTO>().ReverseMap();
            CreateMap<SysDealStageTranslate, ComboboxModelTranslate>().ReverseMap();

            CreateMap<SysDealType, DealTypeDTO>().ReverseMap();
            CreateMap<SysDealTypeTranslate, ComboboxModelTranslate>().ReverseMap();
            
            CreateMap<SysJobTitle, SysJobTitleDTO>().ReverseMap();
            CreateMap<SysJobTitleTranslate, ComboboxModelTranslate>().ReverseMap();
            
            CreateMap<SysLeadSource, LeadSourceDTO>().ReverseMap();
            CreateMap<SysLeadSourceTranslate, ComboboxModelTranslate>().ReverseMap();

            CreateMap<SysLeadRating, LeadRatingDTO>().ReverseMap();
            CreateMap<SysLeadRatingTranslate, ComboboxModelTranslate>().ReverseMap();

            CreateMap<SysLeadStatus, LeadStatusDTO>().ReverseMap();
            CreateMap<SysLeadStatusTranslate, ComboboxModelTranslate>().ReverseMap();


            #endregion


            #endregion


        }

    }
}