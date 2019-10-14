using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.GiftShop.Application.Examples.Models;
using SS.GiftShop.Application.Infrastructure;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Core.Exceptions;
using SS.GiftShop.Core.Persistence;
using SS.GiftShop.Domain.Entities;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Application.Services
{
    public interface IExampleService
    {
        Task Add(AddExampleModel model);
        Task<ExampleModel> Get(Guid id);
        Task<PaginatedResult<ExampleModel>> GetPage(GetExamplePageQuery page);
    }

    public class ExampleService : IExampleService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;

        public ExampleService(IRepository repository, IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
        }

        public async Task Add(AddExampleModel model)
        {
            var entity = _mapper.Map<Example>(model);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task<ExampleModel> Get(Guid id)
        {
            var query = _readOnlyRepository.Query<Example>(x => x.Id == id && x.Status == EnabledStatus.Enabled)
                .ProjectTo<ExampleModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Example>(id);
            }

            return result;
        }

        public async Task<PaginatedResult<ExampleModel>> GetPage(GetExamplePageQuery search)
        {
            var query = _readOnlyRepository.Query<Example>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(search.Term))
            {
                var term = search.Term.Trim();
                query = query.Where(x => x.Name.Contains(term));
            }

            var sortCriteria = search.GetSortCriteria();
            var items = query
                .ProjectTo<ExampleModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Name);
            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, search);

            return page;
        }
    }
}
