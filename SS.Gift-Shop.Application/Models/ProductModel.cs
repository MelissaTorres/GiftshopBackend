using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Products.Models
{
    public class ProductModel
    {
        //public int ProductId { get; set; }
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
        public decimal Price { get; set; }
        //public Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
