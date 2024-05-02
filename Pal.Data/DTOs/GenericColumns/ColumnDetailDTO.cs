﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.GenericColumns
{
    public class ColumnDetailDTO : BaseEntity<int>
    {
        public int ColId { get; set; }
        [ForeignKey("ColId")]
        public ColumnDTO column { get; set; }

        public int TableReferenceId { get; set; }

        public string Val { get; set; }
    }
}
