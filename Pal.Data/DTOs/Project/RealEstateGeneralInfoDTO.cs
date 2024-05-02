using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Project
{
    public class RealEstateGeneralInfoDTO
    {
        //---
        public long Id { get; set; }

        //---
        public float AreaTotal { get; set; }

        //---
        public float AreaNet { get; set; }

        //---
        public string Notes { get; set; }

        //---
        public int RealEstateTypeId { get; set; }

        //---
        public string RealEstateTypeName { get; set; }

        //---
        public int RoomsCountId { get; set; }

        //---
        public string RoomsCountName { get; set; }

        //---
        public int ProjectId { get; set; }
    }
}
