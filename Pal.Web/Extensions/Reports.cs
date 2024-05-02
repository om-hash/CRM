using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pal.Web.Extensions
{
    public static class Reports
    {
        public static DataTable GetTable<TEntity>(IEnumerable<TEntity> table, string name) where TEntity : class
        {
            DataTable result = new DataTable(name);
            PropertyInfo[] infos = typeof(TEntity).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (info.PropertyType.IsGenericType
                    && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    result.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType)));
                else
                    result.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (var el in table)
            {
                DataRow row = result.NewRow();
                foreach (PropertyInfo info in infos)
                    if (info.PropertyType.IsGenericType
                    && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        object t = info.GetValue(el);
                        if (t == null)
                            t = Activator.CreateInstance(Nullable.GetUnderlyingType(info.PropertyType));
                        row[info.Name] = t;
                    }
                    else
                        row[info.Name] = info.GetValue(el);
                result.Rows.Add(row);
            }
            return result;
        }
    }
}
