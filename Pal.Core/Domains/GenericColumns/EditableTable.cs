using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains.GenericColumns
{
    public class EditableTable : BaseEntity<int>
    {
        public string tableName { get; set; }
    }
}
