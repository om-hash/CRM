//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Pal.Core.Domains.GenericColumns;
//using Pal.Data.Contexts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Pal.Services.DataServices.GenericColumn
//{
//    public class ManageGenericColumns : IManageGenericColumns
//    {
//        private readonly ApplicationDbContext _context;
//        public ManageGenericColumns(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public List<string> GetTablesList()
//        {
//            List<string> tablesList = new List<string>();

//            var models = _context.Model.GetEntityTypes();
//            foreach (var modelName in models)
//            {
//                tablesList.Add(modelName.Name);
//            }

//            return tablesList;
//        }
//        public List<string> GetOriginalColumnsList(string tableName)
//        {
//            List<string> ColumnsList = new List<string>();
//            var Columns = _context.Model.FindEntityType(tableName)!.GetProperties();
//            foreach (var colName in Columns)
//            {
//                ColumnsList.Add(colName.Name);
//            }
//            return ColumnsList;
//        }
//        public List<EditableTable> GetExtraColumnsList(string tableName)
//        {
//            List<EditableTable> ColumnsList = new List<EditableTable>();
//            var Columns = _context.columns.Where(w => w.tableName == tableName).ToList();
//            foreach (var col in Columns)
//            {
//                ColumnsList.Add(col);
//            }
//            return ColumnsList;
//        }
//        public bool AddExtraColumn(string TableName, string ColumnName, string InputType)
//        {
//            var newColumn = new EditableTable { columnName = ColumnName, tableName = TableName, inputType = InputType };
//            _context.columns.Add(newColumn);
//            var result = _context.SaveChanges();
//            return result > 0 ? true : false;
//        }
//        public bool EditExtraColumn(string tableName)
//        {
//            return true;
//        }
//        public bool DeleteExtraColumn(string TableName, string ColumnName, string InputType)
//        {
//            var cols = _context.columns.Where(w => w.columnName == ColumnName & w.tableName == TableName).ToList();
//            foreach (var col in cols)
//            {
//                cols.Remove(col);
//            }
//            var result = _context.SaveChanges();
//            return result > 0 ? true : false;
//        }
//    }
//}
