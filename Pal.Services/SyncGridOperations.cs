using Microsoft.EntityFrameworkCore;
using Pal.Data.DTOs;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services
{
    static public class SyncGridOperations<T>
    {

        public static async Task<SyncPaginatedListModel<T>> PagingAndFilterAsync(IQueryable<T> query, DataManagerRequest dm)
        {
            try
            {
                var count = 0;
                if (dm != null)
                {

                    DataOperations operation = new DataOperations();
                    if (dm.Search != null && dm.Search.Count > 0)
                    {
                        foreach (var search in dm.Search)
                        {
                            if (search.Fields != null)
                            {
                                search.Fields = typeof(T).GetProperties().Select(a => a.Name).ToList();
                            }

                            try
                            {
                                query = operation.PerformSearching(query, dm.Search);  //Search
                            }
                            catch (Exception)
                            { // ignore
                            }
                        }


                    }
                    if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
                    {
                        query = operation.PerformSorting(query, dm.Sorted);
                    }
                    if (dm.Where != null && dm.Where.Count > 0) //Filtering
                    {
                        foreach (var where in dm.Where)
                        {
                            if (where.predicates != null)
                            {
                                where?.predicates.ForEach(predicate =>
                                {
                                    var actualName = typeof(T).GetProperties()
                                    .FirstOrDefault(a => a.Name.ToLower() == predicate.Field.ToLower())?
                                    .Name;

                                    predicate.Field = actualName;
                                });
                            }

                            if (!string.IsNullOrEmpty(where.Field))
                            {
                                var actualName = typeof(T).GetProperties()
                                   .FirstOrDefault(a => a.Name.ToLower() == where.Field.ToLower())?
                                   .Name;

                                where.Field = actualName;
                            }

                            try
                            {
                                query = operation.PerformFiltering(query, dm.Where, where.Condition);
                            }
                            catch (Exception ex)
                            { // ignore
                            }
                        }

                    }
                    count = await query.CountAsync();
                    if (dm.Skip != 0)
                    {
                        query = operation.PerformSkip(query, dm.Skip);   //Paging
                    }
                    if (dm.Take != 0)
                    {
                        query = operation.PerformTake(query, dm.Take);
                    }
                }
                return new SyncPaginatedListModel<T>
                {
                    Data = await query.ToListAsync(),
                    TotalCount = count,
                };
            }
            catch (Exception ex)
            {

                return null;
            }

        }

    }

    public class MyDataManagerRequest : DataManagerRequest
    {
        public bool ShowDeleted { get; set; }
    }
}
