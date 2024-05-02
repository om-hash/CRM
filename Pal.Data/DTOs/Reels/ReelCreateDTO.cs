using Pal.Core.Enums.Account;
using Pal.Data.DTOs.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.Reels
{
    public class ReelCreateDTO
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Url { get; set; }
        public string ReferenceId { get; set; }
        public UserType ReferenceType { get; set; }
        public string AcceptedBy { get; set; } // userId
        public string RefuseReason { get; set; }
        public ReelStatus ReelStatus { get; set; }
        public DateTime AcceptedDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewCounts { get; set; } = 0;
        public bool IsExpired { get; set; } = false;
        public List<ReelViewCreateDTO> ReelViews { get; set; }
    }
    public class ReelViewCreateDTO
    {
        public long Id { get; set; }
        public long ReelId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ReferenceId { get; set; }
        public UserType ReferenceType { get; set; }
    }
}
