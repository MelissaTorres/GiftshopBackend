using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Mappings
{
    public sealed class ProductsMapping : Profile
    {
        public ProductsMapping()
        {
            CreateMap<ProductModel, Product>()
                .ForMember(x => x.Id, e => e.Ignore());

            CreateMap<Product, ProductModel>();
        }
    }
}
