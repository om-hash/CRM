
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RealEstate
{
    public class RealEstateListDTO
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int NeighborhoodId { get; set; }
        public string NeighborhoodName { get; set; }
        public string FullAddress { get; set; }
        public int RealEstateTypeId { get; set; }
        public string RealEstateTypeName { get; set; }
        public int? RealEstateClassId { get; set; }
        public string RealEstateClassName { get; set; }
        public int RoomsCountId { get; set; }
        public string RoomsCountName { get; set; }
        //---
        public int HeatingTypeId { get; set; }
        public string HeatingTypeName { get; set; }
        public int AirConditionTypeId { get; set; }
        public string AirConditionTypeName { get; set; }
        //---
        public int ViewTypeId { get; set; }
        public string ViewTypeName { get; set; }
        //---
        public int BalconieDirectionId { get; set; }
        public string BalconieDirectionName { get; set; }
        public float AreaTotal { get; set; }
        public float AreaNet { get; set; }
        public float MeterAvgPrice { get; set; }
        public int FloorsCount { get; set; }
        public int Quantity { get; set; }
        public int SoldQuantity { get; set; }
        //---
        public int BathRoomsCount { get; set; }
        public int WCCount { get; set; }
        public int BalconiesCount { get; set; }
        public float Price { get; set; }
        public DateTime CompletionDate { get; set; }
        public float TourDuration { get; set; }
        public float Proceeds { get; set; }
        public float Dues { get; set; }


        public bool IsInstallmentsSupport { get; set; }
        public int InstallmentsCount { get; set; }
        public bool IsNationaltySupport { get; set; }
        //---
        public bool IsFeatured { get; set; }
 

        public bool IsForRent { get; set; }
        public string CompName { get; set; }
  
        public string RealEstateName { get; set; }
    

        public bool IsActive { get; set; }

        public string ReginName { get; set; }

        // Total count of realEstates in database
        public int TotalCount { get; set; }

      
       
        public string MainImgUrl { get; set; }

        public string PaymentType { get; set; }
        public string Features { get; set; }

        public virtual ICollection<RealEstateFeatureDTO> RealEstateFeatures { get; set; }
        //---
        public virtual ICollection<RealEstateTranslateDTO> RealEstateTranslates { get; set; }
        public virtual ICollection<RealEstatePaymentTypesDTO> RealEstatePaymentTypes { get; set; }


    }
}
