using System;
using System.Collections.Generic;
using System.Text;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Application.Models
{
    public class OrderModel
    {
        //public int OrderId { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
