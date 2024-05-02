using Pal.Core.Enums.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.RatingAndComment
{
    public class RateingAndCommentsDTO
    {
        public int Id { get; set; }
        public string ReferenceId { get; set; }
        public string CustomerId { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public string ReferenceTypeName { get; set; }
        public float Rate { get; set; }
        public string Content { get; set; }
        public DateTime CommentedDate { get; set; }
        public string CommentedBy { get; set; }

        public string UserId { get; set; }
        public bool IsApproved { get; set; }
            
    }
}
