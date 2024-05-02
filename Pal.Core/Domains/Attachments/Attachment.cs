using Pal.Core.Enums.Attachment;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pal.Core.Domains.Attachments
{
    public class Attachment : BaseEntity<Guid>
    {
        [StringLength(50)]
        public string ReferenceId { get; set; }

        [StringLength(500)]
        public string FileName { get; set; }

        [StringLength(10)]
        public string FileExtension { get; set; }


        [StringLength(32)]
        public string Alt { get; set; }

        public ReferenceType ReferenceType { get; set; }
        public MediaType MediaType { get; set; }
    }
}
