using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mosaico.Base.Enums;

namespace Mosaico.Base
{
    public static class FilterManager
    {
        public static IQueryable<TSource> ApplyFilters<TSource>(this IQueryable<TSource> data, IEnumerable<FilterParams> filterParams = null)
        {
            if (filterParams == null || !filterParams.Any()) return data;
            
            IEnumerable distinctColumns = filterParams.Where(x => !string.IsNullOrEmpty(x.ColumnName))
                .Select(x => x.ColumnName).Distinct();

            foreach (string colName in distinctColumns)
            {
                var filterColumn = data.GetType().GetProperty(colName,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (filterColumn != null)
                {
                    IEnumerable<FilterParams> filterValues =
                        filterParams.Where(x => x.ColumnName.Equals(colName)).Distinct();

                    switch (filterValues.Count())
                    {
                        case > 1:
                        {
                            IEnumerable<TSource> sameColData = Enumerable.Empty<TSource>();

                            foreach (var val in filterValues)
                            {
                                sameColData = sameColData.Concat(FilterData(data, val.FilterOption, filterColumn,
                                    val.FilterValue));
                            }

                            data = data.Intersect(sameColData);
                            break;
                        }
                        case 1:
                            data = FilterData(data, filterValues.FirstOrDefault().FilterOption, filterColumn,
                                filterValues.FirstOrDefault().FilterValue);
                            break;
                    }
                }
            }
            return data;  
        }  
        
        private static IQueryable<TEntity> FilterData<TEntity>(IQueryable<TEntity> data, FilterOptions filterOption,
            PropertyInfo filterColumn, string filterValue)
        {
            int outValue;
            DateTime dateValue;
            switch (filterOption)
            {
                case FilterOptions.StartsWith:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null)
                        .ToString().ToLower().StartsWith(filterValue.ToString().ToLower()));
                    break;
                case FilterOptions.EndsWith:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null)
                        .ToString().ToLower().EndsWith(filterValue.ToString().ToLower()));
                    break;
                case FilterOptions.Contains:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null)
                        .ToString().ToLower().Contains(filterValue.ToString().ToLower()));
                    break;
                case FilterOptions.DoesNotContain:
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                           (filterColumn.GetValue(x, null) != null && !filterColumn.GetValue(x, null)
                                               .ToString().ToLower().Contains(filterValue.ToString().ToLower())));
                    break;
                case FilterOptions.IsEmpty:
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                           (filterColumn.GetValue(x, null) != null &&
                                            filterColumn.GetValue(x, null).ToString() == string.Empty));
                    break;
                case FilterOptions.IsNotEmpty:
                    data = data.Where(x =>
                        filterColumn.GetValue(x, null) != null &&
                        filterColumn.GetValue(x, null).ToString() != string.Empty);
                    break;

                case FilterOptions.IsGreaterThan:
                    if ((filterColumn.PropertyType == typeof(int) ||
                         filterColumn.PropertyType == typeof(int?)) &&
                        int.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) > outValue);
                    }
                    else if ((filterColumn.PropertyType == typeof(DateTime?)) &&
                             DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) > dateValue);
                    }

                    break;

                case FilterOptions.IsGreaterThanOrEqualTo:
                    if ((filterColumn.PropertyType == typeof(int) ||
                         filterColumn.PropertyType == typeof(int?)) &&
                        int.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) >= outValue);
                    }
                    else if ((filterColumn.PropertyType == typeof(DateTime?)) &&
                             DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) >= dateValue);
                        break;
                    }

                    break;

                case FilterOptions.IsLessThan:
                    if ((filterColumn.PropertyType == typeof(int) ||
                         filterColumn.PropertyType == typeof(int?)) &&
                        int.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) < outValue);
                    }
                    else if ((filterColumn.PropertyType == typeof(DateTime?)) &&
                             DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) < dateValue);
                        break;
                    }

                    break;

                case FilterOptions.IsLessThanOrEqualTo:
                    if ((filterColumn.PropertyType == typeof(int) ||
                         filterColumn.PropertyType == typeof(int?)) &&
                        int.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) <= outValue);
                    }
                    else if ((filterColumn.PropertyType == typeof(DateTime?)) &&
                             DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) <= dateValue);
                        break;
                    }

                    break;

                case FilterOptions.IsEqualTo:
                    if (filterValue == string.Empty)
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null
                                               || (filterColumn.GetValue(x, null) != null &&
                                                   filterColumn.GetValue(x, null).ToString().ToLower() == string.Empty));
                    }
                    else
                    {
                        if ((filterColumn.PropertyType == typeof(int) ||
                             filterColumn.PropertyType == typeof(int?)) &&
                            int.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) == outValue);
                        }
                        else if ((filterColumn.PropertyType == typeof(DateTime?)) &&
                                 DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) == dateValue);
                            break;
                        }
                        else
                        {
                            data = data.Where(x =>
                                filterColumn.GetValue(x, null) != null &&
                                string.Equals(filterColumn.GetValue(x, null).ToString(), filterValue, StringComparison.CurrentCultureIgnoreCase));
                        }
                    }

                    break;

                case FilterOptions.IsNotEqualTo:
                    if ((filterColumn.PropertyType == typeof(int) ||
                         filterColumn.PropertyType == typeof(int?)) &&
                        int.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) != outValue);
                    }
                    else if ((filterColumn.PropertyType == typeof(DateTime?)) &&
                             DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) != dateValue);
                        break;
                    }
                    else
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                               (filterColumn.GetValue(x, null) != null &&
                                                !string.Equals(filterColumn.GetValue(x, null).ToString(), filterValue, StringComparison.CurrentCultureIgnoreCase)));
                    }

                    break;
            }

            return data;
        }
    }
}