using Microsoft.AspNetCore.Http;
using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.Lookups;
using Pal.Data.DTOs.RatingAndComment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pal.Data.DTOs.Project
{
    public class ProjectCreateDTO
    {

        public int Id { get; set; }
        //----
        public int CompId { get; set; }
        //----
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int NeighborhoodId { get; set; }
        //----
        public int ProjectStatusId { get; set; }
        public int ProjectTypeId { get; set; }
        ////----
        //public int PaymentTypeId { get; set; }
        //----
        public DateTime CompletionDate { get; set; }
        //----
        public float Area { get; set; }
        public int BuildingAge { get; set; }
        //----
        public int BuildingCount { get; set; }
        //----
        public float RealEstateAvgPrice { get; set; }
        //----
        public float MeterAvgPrice { get; set; }
        public float TourDuration { get; set; }
        //----
        /// <summary>
        /// List Of strings Like (Parking,Sweeming Pool,Park...etc)
        /// </summary>
        public string ExtraFeatures { get; set; }

        [StringLength(100)]
        public string FullAddress { get; set; }
        //----
        [StringLength(500)]
        public string MainImg { get; set; }
        //----
        public IFormFile MainImgFile { get; set; }
        //----
        public int ApproveState { get; set; }


        public bool IsFeatured { get; set; }

        public bool IsActive { get; set; }
        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }

        public string Barcode { get; set; }
        public bool IsForRent { get; set; }

        #region Children
        public List<ProjectExtraDetailsDTO> ProjectExtraDetails { get; set; }
        public List<ProjectTranslateDTO> ProjectTranslates { get; set; }
        public List<ProjectFeaturesDTO> ProjectFeatures { get; set; }
        public List<Attachment> ProjectImages { get; set; }
        public List<ProjectPaymentTypesDTO> ProjectPaymentTypes { get; set; }
        public List<RateingAndCommentsDTO> Reviews { get; set; }
        public virtual ICollection<RealEstateGeneralInfoDTO> RealEstateGeneralInfos { get; set; }
        #endregion


    }
}
