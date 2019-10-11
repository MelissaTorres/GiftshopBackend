using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Domain.Entities
{
    public class Category : Entity
    {
        //public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
