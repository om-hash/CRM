using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.RatingAndComment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RealEstate
{
    public class RealEstateCreateDTO
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int NeighborhoodId { get; set; }
        //---
        public int? ProjectId { get; set; }
        public int PaymentTypeId { get; set; }
        //---
        public int RealEstateClassId { get; set; }
        public int RealEstateTypeId { get; set; }
        //---
        public int RoomsCountId { get; set; }
        //---
        public int HeatingTypeId { get; set; }
        public int BalconieDirectionId { get; set; }
        public int ViewTypeId { get; set; }
        public int SoldQuantity { get; set; }
        public float Proceeds { get; set; }
        //---
        public int AirConditionTypeId { get; set; }
        //---
        public float TourDuration { get; set; }
        [StringLength(500)]
        public string MainImgUrl { get; set; }
        public IFormFile MainImgFile { get; set; }
        //---
        public float Price { get; set; }

        //---
        public DateTime CompletionDate { get; set; }
        //---
        public float AreaTotal { get; set; }
        //---
        public float AreaNet { get; set; }
        //---
        public int FloorsCount { get; set; }
        //---
        public int Quantity { get; set; }
        //---
        public int BathRoomsCount { get; set; }
        //---
        public int WCCount { get; set; }
        //---
        public int BalconiesCount { get; set; }
        //---
        public float Dues { get; set; }
        //---
        public float MeterAvgPrice { get; set; }
        //---
        public bool IsInstallmentsSupport { get; set; }
        //---
        public bool IsFeatured { get; set; }
        public bool IsNationaltySupport { get; set; }
        //---
        public int InstallmentsCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsForRent { get; set; }
        //---





        /// <summary>
        /// List Of strings Like (Parking,Sweeming Pool,Park...etc)
        /// </summary>
        public string ExtraFeatures { get; set; }

        [StringLength(250)]
        public string FullAddress { get; set; }

        public int ApproveState { get; set; }

        [StringLength(100)]
        public string Lat { get; set; }

        public List<IFormFile> BluePrintsFiles { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }

        public string Barcode { get; set; }


        #region Childrens
        public List<RealEstateFeatureDTO> RealEstateFeatures { get; set; }
        //---
        public List<RealEstateTranslateDTO> RealEstateTranslates { get; set; }
        //---
        public List<RealEstateExtraDetailsDTO> RealEstateExtraDetails { get; set; }
        public  List<Attachment> Attachments { get; set; }
        public  List<Attachment> BluePrints { get; set; }
    
        public List<RateingAndCommentsDTO> Reviews { get; set; }
        public List<RealEstatePaymentTypesDTO> RealEstatePaymentTypes { get; set; }

        #endregion



        #region For View Model
        public int CompanyId { get; set; }
        #endregion

    }
}
