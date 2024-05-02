using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.DTOs
{
    public class SyncPaginatedListModel<T>
    {
        public List<T> Data { get; set; }

        public T DataSolo { get; set; }
        public int TotalCount { get; set; }

        public SyncPaginatedListModel(List<T> data, int totalCount)
        {
            this.Data = data;
            this.TotalCount = totalCount;
        }
        
        public SyncPaginatedListModel(T data, int totalCount)
        {
            this.DataSolo = data;
            this.TotalCount = totalCount;
        }

        public SyncPaginatedListModel()
        {
        }
    }
}
