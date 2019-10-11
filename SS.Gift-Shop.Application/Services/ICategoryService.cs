using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public interface ICategoryService
    {
        Task Add(AddCategoryModel model);
        Task<CategoryModel> Get(Guid categoryId);
        Task<PaginatedResult<CategoryModel>> GetPage(GetCategoryPageQuery page);
    }

    public class CategoryExample : ICategoryService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;

        public CategoryExample(IRepository repository, IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
        }

        public async Task Add(AddCategoryModel model)
        {
            var entity = _mapper.Map<Category>(model);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task<CategoryModel> Get(Guid categoryId)
        {
            var query = _readOnlyRepository.Query<Category>(x => x.Id == categoryId )
                .ProjectTo<CategoryModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Category>(categoryId);
            }

            return result;
        }

        public async Task<PaginatedResult<CategoryModel>> GetPage(GetCategoryPageQuery search)
        {
            //var query = _readOnlyRepository.Query<Category>(x => x.CategoryName);
            var query = _readOnlyRepository.Query<Category>(x => x.CategoryName);

            if (!string.IsNullOrEmpty(search.Term))
            {
                var term = search.Term.Trim();
                query = query.Where(x => x.CategoryName.Contains(term));
            }

            var sortCriteria = search.GetSortCriteria();
            var items = query
                .ProjectTo<CategoryModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.CategoryName);
            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, search);

            return page;
        }
    }
}
