using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Tour
{

    public class TourCreateDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Time { get; set; }

        public int? AdvsorId { get; set; }

        public long CustomerId { get; set; }

        public bool IsTemporary { get; set; }

        public float Duration { get; set; }

        public int CityId { get; set; }

        public string CustomerNotes { get; set; }

        public List<TourDetailsListDTO> TourRefrencesListDTOs { get; set; }

    }
}
