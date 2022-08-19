using System;
using System.Collections.Generic;

namespace Mosaico.Base
{
    public class PaginatedResult<TEntity>
    {
        public long Total { get; set; }
        public IEnumerable<TEntity> Entities { get; set; }
    }
}