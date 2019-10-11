using AutoMapper;
using SS.GiftShop.Application.Examples.Models;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Examples
{
    public sealed class ExamplesMapping : Profile
    {
        public ExamplesMapping()
        {
            CreateMap<ExampleModel, Example>()
                .ForMember(x => x.Id, e => e.Ignore());

            CreateMap<Example, ExampleModel>();
        }
    }
}
