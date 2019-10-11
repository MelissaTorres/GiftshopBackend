using System.ComponentModel.DataAnnotations;
using SS.GiftShop.Core;

namespace SS.GiftShop.Api.Identity.Models
{
    public class ResetPasswordViewModel : SetPasswordModelBase
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [StringLength(AppConstants.EmailLength)]
        public virtual string Email { get; set; }
    }
}
