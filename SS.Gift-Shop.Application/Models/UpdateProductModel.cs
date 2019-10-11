using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Models
{
    public class UpdateProductModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
