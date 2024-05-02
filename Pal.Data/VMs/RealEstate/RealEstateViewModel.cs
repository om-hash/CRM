using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.Advertisements;
using Pal.Data.DTOs.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.RealEstate
{
    public class RealEstateViewModel
    {
        public RealEstateViewModel( )
        {
            Attachments = new List<Attachment>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }

        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string NeighborhoodName { get; set; }
        public float AvrageRating { get; set; } = 0;
        //---
        public string ProjectName { get; set; }
        public string PaymentType { get; set; }

        public string QrCode { get; set; }
        //---
        public string RealEstateType { get; set; }
        //---
        public string RoomsCount { get; set; }
        //---
        public string HeatingType { get; set; }
        public string ViewType { get; set; }
        public string BalconieDirection { get; set; }

        //---
        public string AirConditionType { get; set; }
        public float TourDuration { get; set; }

        public bool IsAddedToFavorite { get; set; }
        public bool IsAddedToTour { get; set; }

        public string Features { get; set; }
        public string ExtraFeatures1 { get; set; }
        //---

        [StringLength(100)]
        public string MainImgUrl { get; set; }
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
        public int BalconieDirectionId { get; set; }
        public int ViewTypeId { get; set; }
        public int SoldQuantity { get; set; }
        public float Proceeds { get; set; }
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
        //---
        /// <summary>
        /// List Of strings Like (Parking,Sweeming Pool,Park...etc)
        /// </summary>
        public List<string> ExtraFeatures { get; set; }




        [StringLength(250)]
        public string FullAddress { get; set; }

        public int ApproveState { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }

        public string CompanyName { get; set; }
        [Required, StringLength(250)]
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public string Content { get; set; }

        public string Terms { get; set; }


        #region Childrens
        public List<RealEstateFeatureDTO> RealEstateFeatures { get; set; }
        //---
        //---
        public List<Attachment> Attachments { get; set; }
        public List<Attachment> BluePrints { get; set; }
        public List<RealEstatePaymentTypesDTO> RealEstatePaymentTypes { get; set; }
       

        #endregion






    }
    }
