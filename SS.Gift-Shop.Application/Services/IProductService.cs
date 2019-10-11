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
    public interface IProductService
    {
        Task Add(AddProductModel model);
        Task<ProductModel> Get(Guid productId);
        Task<PaginatedResult<ProductModel>> GetPage(GetProductPageQuery page);
        Task Update(Guid productId, UpdateProductModel model);
        Task Delete(Guid productId);
    }

    public class  ProductService : IProductService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;

        public ProductService(IRepository repository, IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
        }

        public async Task Add(AddProductModel model)
        {
            var entity = _mapper.Map<Product>(model);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task<ProductModel> Get(Guid productId)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Id.Equals(productId))
                .ProjectTo<ProductModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Product>(productId);
            }

            return result;
        }

        //Look for catgory
        public async Task<PaginatedResult<ProductModel>> GetPage(GetProductPageQuery search)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Category.CategoryName);

            if (!string.IsNullOrEmpty(search.Term))
            {
                var term = search.Term.Trim();
                query = query.Where(x => x.ProductName.Contains(term));
            }

            var sortCriteria = search.GetSortCriteria();
            var items = query
                .ProjectTo<ProductModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.ProductName);
            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, search);

            return page;
        }

        public async Task Update(Guid productId, UpdateProductModel model)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Id.Equals(productId))
                                        .ProjectTo<ProductModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result != null)
            {
                result.ProductName = model.ProductName;
                result.Description = model.Description;
                result.Characteristics = model.Characteristics;
                result.Price = model.Price;
                result.Category = model.Category;
                result.CategoryId = model.CategoryId;

                _repository.Update(result);

                await _repository.SaveChangesAsync();
            }
            else
            {
                throw EntityNotFoundException.For<Product>(productId);
            }
        }

        public async Task Delete(Guid productId)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Id.Equals(productId))
                            .ProjectTo<ProductModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result != null)
            {
                _repository.Remove(result);
                await _repository.SaveChangesAsync();
            }
            else
            {
                throw EntityNotFoundException.For<Product>(productId);
            }
        }

    }
}
