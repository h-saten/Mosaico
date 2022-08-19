using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mosaico.Base
{
    public class PaginatedList<TEntity>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<TEntity> Items { get; set; } = new List<TEntity>();

        public PaginatedList()
        {
            PageIndex = default;
            TotalItems = default;
            TotalPages = default;
            Items = new List<TEntity>();
        }
        
        public PaginatedList(IEnumerable<TEntity> items, int pageIndex = 0, int pageSize = 10)
        {
            var listItems = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            
            PageIndex = pageIndex;
            TotalItems = listItems.Count;
            TotalPages = (int) Math.Ceiling(listItems.Count / (double) pageSize);

            Items?.AddRange(listItems);
        }

        public bool HasPreviousPage => (PageIndex > 1);

        public bool HasNextPage => (PageIndex < TotalPages);
    }
}