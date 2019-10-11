using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.GiftShop.Application.Infrastructure;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Core.Exceptions;
using SS.GiftShop.Core.Persistence;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Services
{
    public interface IOrderService
    {
        Task Add(AddOrderModel model);
        Task<OrderModel> Get(Guid id);
        Task<PaginatedResult<OrderModel>> GetPage(GetOrderPageQuery search);
    }

    public class OrderService : IOrderService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;

        public OrderService(IRepository repository, IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
        }

        public async Task Add(AddOrderModel model)
        {
            var entity = _mapper.Map<Order>(model);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task<OrderModel> Get(Guid orderId)
        {
            var query = _readOnlyRepository.Query<Order>(x => x.Id == orderId)
                .ProjectTo<OrderModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Order>(orderId);
            }

            return result;
        }

        public async Task<PaginatedResult<OrderModel>> GetPage(GetOrderPageQuery search)
        {
            var query = _readOnlyRepository.Query<Order>();

            if (!string.IsNullOrEmpty(search.Term))
            {
                var term = search.Term.Trim();
                query = query.Where(x => x.UserId.Contains(term));
            }

            var sortCriteria = search.GetSortCriteria();
            var items = query
                .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.UserId);
            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, search);

            return page;
        }
    }
}
