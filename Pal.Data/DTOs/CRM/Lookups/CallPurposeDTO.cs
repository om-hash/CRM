
using Pal.Core.Domains.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Lookups
{
    public class CallPurposeDTO
    {
        public int Id { get; set; }
        public string CallPurpose { get; set; }
        public List<ComboboxModelTranslate> Translates { get; set; }
    }
}
