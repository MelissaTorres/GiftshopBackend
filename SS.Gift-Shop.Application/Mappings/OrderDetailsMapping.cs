using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Mappings
{
    public sealed class OrderDetailsMapping : Profile
    {
        public OrderDetailsMapping()
        {
            CreateMap<OrderDetailModel, OrderDetail>()
                .ForMember(x => x.Id, e => e.Ignore());

            //CreateMap<OrderDetailModel, OrderDetail>()
               // .ForMember(x => x.ProductId, e => e.Ignore());

            CreateMap<OrderDetail, OrderDetailModel>();
        }
    }
}
