//using Pal.Core.Domains.Projects;
using System;
using System.Collections.Generic;

namespace Pal.Data.DTOs.Project
{
    public class ProjectListDTO
    {
        public int Id { get; set; }
        //----
        public string CompName { get; set; }
        public int CompId { get; set; }
        //----
        public string ProjectName { get; set; }

        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string ReginName { get; set; }

        //----
        public string NeighborhoodName { get; set; }
        //----
        public string ProjectTypeName { get; set; }
        public string ProjectStatus { get; set; }
        //----
        public string PaymentType { get; set; }
        public string Feature { get; set; }
        //----
        public float Area { get; set; }
        //----
        public int BuildingCount { get; set; }
        //----
        public float RealEstateAvgPrice { get; set; }
        //----
        public float MeterAvgPrice { get; set; }

        public bool IsActive { get; set; }

        // the total count of projects in database 
        public int TotalCount { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int NeighborhoodId { get; set; }

        public int ProjectTypeId { get; set; }
        public int ProjectStatusId { get; set; }
        public DateTime CompletionDate { get; set; }

        public float AvrageRating { get; set; } = 0;

        public float TourDuration { get; set; }
        public string FullAddress { get; set; }
        public int BuildingAge { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsForRent { get; set; }
        public ICollection<ProjectPaymentTypesDTO> PaymentTypes { get; set; }
        public ICollection<ProjectFeaturesDTO> Features { get; set; }
        public virtual ICollection<ProjectTranslateDTO> ProjectTranslates { get; set; }
    }
}
