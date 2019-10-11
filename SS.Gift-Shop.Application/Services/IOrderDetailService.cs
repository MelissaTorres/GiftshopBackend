using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Application.Infrastructure;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Application.Examples.Models;
using System.Threading.Tasks;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Core.Persistence;
using AutoMapper;
using SS.GiftShop.Domain.Entities;
using SS.GiftShop.Core.Exceptions;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace SS.GiftShop.Application.Services
{
    public interface IOrderDetailService
    {
        Task Add(AddOrderDetailModel model);
        Task <OrderDetailModel> Get(Guid orderId);
        Task<PaginatedResult<OrderDetailModel>> GetPage(GetOrderDetailPageQuery search);
    }

    public class OrderDetailService : IOrderDetailService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;

        public OrderDetailService(IRepository repository, IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
        }

        public async Task Add(AddOrderDetailModel model)
        {
            var entity = _mapper.Map<OrderDetail>(model);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task<OrderDetailModel> Get(Guid orderId)
        {
            var query = _readOnlyRepository.Query<OrderDetail>(x => x.OrderId.Equals(orderId))
                .ProjectTo<OrderDetailModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<OrderDetail>(orderId);
            }

            return result;
        }

        public async Task<PaginatedResult<OrderDetailModel>> GetPage(GetOrderDetailPageQuery search)
        {
            var query = _readOnlyRepository.Query<OrderDetail>();

            if (!string.IsNullOrEmpty(search.Term))
            {
                var term = search.Term.Trim();
                query = query.Where(x => x.Product.ProductName.Contains(term));
            }

            var sortCriteria = search.GetSortCriteria();
            var items = query
                .ProjectTo<OrderDetailModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Product.ProductName);
            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, search);

            return page;
        }
    }
}
