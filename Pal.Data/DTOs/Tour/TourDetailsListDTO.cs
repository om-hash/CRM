using Pal.Core.Enums.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Tour
{
    public class TourDetailsListDTO
    {
        public int TourId { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int RegerenceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public float Duration { get; set; }



    }
}
