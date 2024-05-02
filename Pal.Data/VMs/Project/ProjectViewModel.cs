using Pal.Core.Domains.Attachments;
using Pal.Data.DTOs.Advertisements;
using Pal.Data.DTOs.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.VMs.Project
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        //----
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }

        public int ComapnyId { get; set; }
        public string CompanyName { get; set; }


        //----


        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string NeighborhoodName { get; set; }
        //----
        public string ProjectStatusName { get; set; }
        public string ProjectTypeName { get; set; }
        public float AvrageRating { get; set; } = 0;
        //----
        public string PaymentTypeName { get; set; }
        //----
        public DateTime CompletionDate { get; set; }
        //----
        public float Area { get; set; }
        //----
        public float TourDuration { get; set; }
        public int BuildingCount { get; set; }
        //----
        public float RealEstateAvgPrice { get; set; }
        //----
        public double RealEstateMaxPrice { get; set; }
        //----
        public double RealEstateMinPrice { get; set; }
        //----
        public float MeterAvgPrice { get; set; }
        //----
        public int BuildingAge { get; set; }
        /// <summary>
        /// List Of strings Like (Parking,Sweeming Pool,Park...etc)
        /// </summary>
        public List<string> ExtraFeatures { get; set; }

        [StringLength(100)]
        public string FullAddress { get; set; }
        //----
        [StringLength(100)]
        public string MainImg { get; set; }
        
        //----
        public int ApproveState { get; set; }

        public bool IsAddedToFavorite { get; set; }
        public bool IsAddedToTour { get; set; }
        public bool IsFeatured { get; set; }

        public bool IsActive { get; set; }
        [StringLength(100)]
        public string Lat { get; set; }

        [StringLength(100)]
        public string Lng { get; set; }


        public int LanguageId { get; set; }
        public string ProjectName { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }

        #region Children
      
        public List<ProjectFeaturesViewMdoel> ProjectFeatures { get; set; }
        public List<Attachment> ProjectImages { get; set; }
        public List<ProjectPaymentTypesDTO> PaymentTypes { get; set; }
   
        #endregion

    }
}
