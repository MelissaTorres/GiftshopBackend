using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Models
{
    public class AddCategoryModel
    {
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
