using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Core.Persistence;

namespace SS.GiftShop.Application.Infrastructure
{
    public static class Paginator
    {
        public static Task<PaginatedResult<TItem>> MakePageAsync<TCount, TItem>(this IPaginator paginator, IRepositoryBase repository,
            IQueryable<TCount> countQuery,
            IQueryable<TItem> itemsQuery,
            PaginatedQuery model, CancellationToken cancellationToken = default)
            where TCount : class where TItem : class
        {
            if (paginator == null)
            {
                throw new ArgumentNullException(nameof(paginator));
            }

            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (countQuery == null)
            {
                throw new ArgumentNullException(nameof(countQuery));
            }

            if (itemsQuery == null)
            {
                throw new ArgumentNullException(nameof(itemsQuery));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return paginator.MakePageAsync(repository, countQuery, itemsQuery, model.Page, model.PageSize, cancellationToken);
        }

        public static void ValidatePaging(int page, int pageSize)
        {
            if (page < PaginatedQuery.MinPage)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }

            if (pageSize < PaginatedQuery.MinPageSize || pageSize > PaginatedQuery.MaxPageSize)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }
        }
    }
}
