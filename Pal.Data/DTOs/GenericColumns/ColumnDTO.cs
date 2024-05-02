using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.GenericColumns
{
    public class ColumnDTO : BaseEntity<int>
    {
        public string tableName { get; set; }
        public string columnName { get; set; }
        public string inputType { get; set; }

        public ICollection<ColumnDetailDTO> columnDetails { get; set; }
    }
}
