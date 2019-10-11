using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Domain.Entities
{
    public class Product : Entity
    {
        //public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
