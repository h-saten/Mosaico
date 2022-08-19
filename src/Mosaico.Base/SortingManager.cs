using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mosaico.Base.Enums;

namespace Mosaico.Base
{
    public static class SortingManager
    {
        public static IQueryable<TSource> ApplySortings<TSource>(this IQueryable<TSource> data, IEnumerable<SortingParams> sortingParams = null)  
        {
            if (sortingParams == null || !sortingParams.Any()) return data;
            
            IOrderedQueryable<TSource> sortedData = null;
            foreach (var sortingParam in sortingParams.Where(x => !string.IsNullOrEmpty(x.ColumnName)))
            {
                var col = data.GetType().GetProperty(sortingParam.ColumnName,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    sortedData = sortedData == null ? sortingParam.SortOrder == SortOrders.Asc
                            ? data.OrderBy(x => col.GetValue(x, null))
                            : data.OrderByDescending(x => col.GetValue(x, null))
                        : sortingParam.SortOrder == SortOrders.Asc ? sortedData.ThenBy(x => col.GetValue(x, null))
                        : sortedData.ThenByDescending(x => col.GetValue(x, null));
                }
            }
            return sortedData ?? data;
        }  
    }
}