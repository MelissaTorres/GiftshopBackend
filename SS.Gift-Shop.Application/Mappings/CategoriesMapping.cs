using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Mappings
{
    public sealed class CategoriesMapping : Profile
    {
        public CategoriesMapping()
        {
            CreateMap<AddCategoryModel, Category>()
                .ForMember(x => x.Id, e => e.Ignore());

            CreateMap<Category, CategoryModel>();
        }
    }
}
