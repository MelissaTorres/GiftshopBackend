using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SS.GiftShop.Application.Infrastructure
{
    public static class SortCriteriaHelper
    {
        private static readonly SortCriterion[] Empty = new SortCriterion[0];

        public static ICollection<SortCriterion> GetSortCriteria(string orderByExpression)
        {
            if (string.IsNullOrEmpty(orderByExpression))
            {
                return Empty;
            }

            return GetSortCriteriaInternal(orderByExpression)
                .ToList();
        }

        private static IEnumerable<SortCriterion> GetSortCriteriaInternal(string orderBy)
        {
            orderBy = orderBy.Trim();
            foreach (var criterion in orderBy.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var direction = ListSortDirection.Ascending;
                var startIndex = 0;
                switch (criterion[0])
                {
                    case '-':
                        direction = ListSortDirection.Descending;
                        startIndex = 1;
                        break;

                    case '+':
                        startIndex = 1;
                        break;
                }

                if (criterion.Length > startIndex)
                {
                    yield return new SortCriterion(criterion.Substring(startIndex), direction);
                }
            }
        }
    }
}
