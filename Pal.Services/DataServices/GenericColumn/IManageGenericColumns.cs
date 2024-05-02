using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pal.Core.Domains.GenericColumns;
using Pal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.GenericColumn
{
    public interface IManageGenericColumns
    {
        List<string> GetTablesList();
        List<string> GetOriginalColumnsList(string tableName);
        List<EditableTable> GetExtraColumnsList(string tableName);
        bool AddExtraColumn(string TableName, string ColumnName, string InputType);
        bool EditExtraColumn(string tableName);
        bool DeleteExtraColumn(string TableName, string ColumnName, string InputType);

    }
}
