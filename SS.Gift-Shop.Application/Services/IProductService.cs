using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.GiftShop.Application.Products.Models;
using SS.GiftShop.Application.Infrastructure;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Core.Exceptions;
using SS.GiftShop.Core.Persistence;
using SS.GiftShop.Domain.Entities;
using SS.GiftShop.Domain.Model;
using System.Collections.Generic;
using SS.GiftShop.Application.Models;

namespace SS.GiftShop.Application.Services
{
    public interface IProductService
    {
        Task Add(ProductModel model);
        Task<ProductModel> Get(Guid id);
        Task<PaginatedResult<ProductModel>> GetPage(GetProductPageQuery page);
        Task Update(Guid id, ProductModel model);
        Task Delete(Guid id);
        Task<List<CategoryModel>> GetCategories();
        //Task<List<CategoryModel>> GetCategories();
        //Task<ListResult<CategoryModel>> GetCategories();
    }

    public class  ProductService : IProductService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;
        private Product product;

        public ProductService(IRepository repository, IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
            this.product = new Product();
        }

        public async Task Add(ProductModel model)
        {
            var g = new Guid();
            var entity = _mapper.Map<Product>(model);
            entity.Id = g;
            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task<ProductModel> Get(Guid id)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Id.Equals(id))
                .ProjectTo<ProductModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Product>(id);
            }

            return result;
        }

        //Look for catgory
        public async Task<PaginatedResult<ProductModel>> GetPage(GetProductPageQuery search)
        {
            //var query = _readOnlyRepository.Query<Product>(x => x.Category.CategoryName);
            var query = _readOnlyRepository.Query<Product>();

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

        public async Task Update(Guid id, ProductModel model)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Id.Equals(id))
                                        .ProjectTo<ProductModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result != null)
            {
                //result.Id = id;
                //result.ProductName = model.ProductName;
                //result.Description = model.Description;
                //result.Characteristics = model.Characteristics;
                //result.Price = model.Price;
                ////result.Category = model.Category;
                //result.CategoryId = model.CategoryId;
                var entity = _mapper.Map<Product>(model);
                entity.Id = id;
                _repository.Update(entity);

                await _repository.SaveChangesAsync();
            }
            else
            {
                throw EntityNotFoundException.For<Product>(id);
            }
        }

        public async Task Delete(Guid id)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Id.Equals(id))
                            .ProjectTo<ProductModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);
            if (result != null)
            {
                product.Id = result.Id;
                product.ProductName = result.ProductName;
                product.Description = result.Description;
                product.Characteristics = result.Characteristics;
                product.Price = result.Price;
                //product.Category = result.Category;
                product.CategoryId = result.CategoryId;
                _repository.Remove(product);
                await _repository.SaveChangesAsync();
            }
            else
            {
                throw EntityNotFoundException.For<Product>(id);
            }
        }

        //public async Task<ListResult<CategoryModel>> GetCategories()
        //{
        //    var query = _readOnlyRepository.Query<CategoryModel>();

        //    if(query.Count() > 0)
        //    {
        //        var listResult = new ListResult<CategoryModel>(query);
        //        return listResult;
        //    }
        //    return null;
        //}

        public async Task<List<CategoryModel>> GetCategories()
        {
            var query = _readOnlyRepository.Query<Category>()
                .ProjectTo<CategoryModel>(_mapper.ConfigurationProvider);
            var result = await _readOnlyRepository.ListAsync(query);
            
            return result;
        }
    }
}
