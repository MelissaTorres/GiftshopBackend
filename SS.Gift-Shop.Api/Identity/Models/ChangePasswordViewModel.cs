using System.ComponentModel.DataAnnotations;

namespace SS.GiftShop.Api.Identity.Models
{
    public class ChangePasswordViewModel : SetPasswordModelBase
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
