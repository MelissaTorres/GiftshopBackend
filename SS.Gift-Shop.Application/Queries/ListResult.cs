using System;
using System.Collections.Generic;
using System.Linq;

namespace SS.GiftShop.Application.Queries
{
    public class ListResult<T>
    {
        public virtual IList<T> Items { get; }

        public object Metadata { get; set; }

        public ListResult(IEnumerable<T> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Items = items as List<T> ?? items.ToList();
        }
    }

    public static class ListResult
    {
        public static ListResult<T> From<T>(List<T> items, object metadata = null)
        {
            return new ListResult<T>(items)
            {
                Metadata = metadata
            };
        }
    }
}
