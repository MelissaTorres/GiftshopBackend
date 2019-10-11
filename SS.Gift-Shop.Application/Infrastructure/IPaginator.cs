using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Core.Persistence;

namespace SS.GiftShop.Application.Infrastructure
{
    public interface IPaginator
    {
        Task<PaginatedResult<TItem>> MakePageAsync<TCount, TItem>(IRepositoryBase repository, IQueryable<TCount> countQuery,
            IQueryable<TItem> itemsQuery, int page, int pageSize, CancellationToken cancellationToken = default)
            where TCount : class where TItem : class;
    }
}
