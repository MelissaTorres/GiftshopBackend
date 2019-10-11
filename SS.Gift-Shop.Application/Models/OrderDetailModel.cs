using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Models
{
    public class OrderDetailModel
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public string User { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
