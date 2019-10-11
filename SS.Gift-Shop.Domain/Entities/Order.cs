using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Domain.Entities
{
    public class Order : Entity
    {
        //public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
