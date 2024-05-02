﻿
using Pal.Core.Domains.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs.CRM.Lookups
{
    public class CallResultDTO
    {
        public int Id { get; set; }
        public string CallResult { get; set; }
        public List<ComboboxModelTranslate> Translates { get; set; }
    }
}
