using System.ComponentModel.DataAnnotations;

namespace SS.GiftShop.Api.Identity.Models
{
    public class LoginViewModel : EmailModelBase
    {
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
    }
}
