using System;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Application.Examples.Models
{
    public class ExampleModel : IStatus<EnabledStatus>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Email { get; set; }

        public EnabledStatus Status { get; set; }
    }
}
