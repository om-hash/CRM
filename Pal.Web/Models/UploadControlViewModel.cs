using Pal.Core.Enums.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Models
{
    public class UploadControlViewModel
    {
        public string ReferenceTitle { get; set; } = "Upload Files";
        public ReferenceType ReferenceType { get; set; }
        public string ReferenceId { get; set; }

    }
}
