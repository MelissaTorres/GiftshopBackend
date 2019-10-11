using System.Collections.Generic;
using SS.GiftShop.Application.Infrastructure;
using SS.GiftShop.Core;
using SS.GiftShop.Core.Persistence;

namespace SS.GiftShop.Application.Queries
{
    public class ListQuery
    {
        public string OrderBy { get; set; }

        public string Term { get; set; }
    }

    public class ListQuery<T> : ListQuery
    {

    }

    public static class ListQueryExtensions
    {
        public static ICollection<SortCriterion> GetSortCriteria(this ListQuery query)
        {
            return SortCriteriaHelper.GetSortCriteria(query?.OrderBy);
        }
    }
}
