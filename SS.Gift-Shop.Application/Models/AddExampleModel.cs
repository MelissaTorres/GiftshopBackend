using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Application.Examples.Models
{
    public class AddOrderDetail: IStatus<EnabledStatus>
    {
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Email { get; set; }

        public EnabledStatus Status { get; set; }
    }
}
