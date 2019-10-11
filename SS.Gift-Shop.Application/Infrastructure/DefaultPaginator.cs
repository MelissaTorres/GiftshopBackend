using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Core.Persistence;

namespace SS.GiftShop.Application.Infrastructure
{
    public class DefaultPaginator : IPaginator
    {
        public async Task<PaginatedResult<TItem>> MakePageAsync<TCount, TItem>(IRepositoryBase repository, IQueryable<TCount> countQuery,
            IQueryable<TItem> itemsQuery, int page, int pageSize, CancellationToken cancellationToken = default)
            where TCount : class where TItem : class
        {
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

            Paginator.ValidatePaging(page, pageSize);

            var count = await repository.CountAsync(countQuery, cancellationToken);

            var items = await repository.ListAsync(itemsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize), cancellationToken);

            return PaginatedResult.From(items, count, page, pageSize);
        }
    }
}
