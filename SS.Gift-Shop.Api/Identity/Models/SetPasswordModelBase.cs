using System.ComponentModel.DataAnnotations;

namespace SS.GiftShop.Api.Identity.Models
{
    public abstract class SetPasswordModelBase
    {
        [DataType(DataType.Password)]
        //[PasswordComplexityRegularExpression]
        public string NewPassword { get; set; }
    }
}
