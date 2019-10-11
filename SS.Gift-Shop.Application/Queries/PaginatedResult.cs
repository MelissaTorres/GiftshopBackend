using System;
using System.Collections.Generic;

namespace SS.GiftShop.Application.Queries
{
    public class PaginatedResult<T> : ListResult<T>
    {
        public virtual long Current { get; set; }

        public virtual long PageSize { get; set; }

        public virtual long Total { get; set; }

        public virtual long TotalPages
        {
            get
            {
                if (PageSize > 0)
                {
                    return (long)Math.Ceiling((double)Total / PageSize);
                }

                return 0;
            }
        }

        public PaginatedResult(IEnumerable<T> items)
            : base(items)
        {
        }
    }

    public static class PaginatedResult
    {
        public static PaginatedResult<T> From<T>(List<T> items, long total, long current, long itemsPerPage, object metadata = null)
        {
            return new PaginatedResult<T>(items)
            {
                Current = current,
                Total = total,
                PageSize = itemsPerPage,
                Metadata = metadata
            };
        }

        public static PaginatedResult<T> Empty<T>(long current, long itemsPerPage, object metadata = null)
        {
            return From(new List<T>(), 0, current, itemsPerPage, metadata);
        }
    }
}
