using System.ComponentModel.DataAnnotations;
using SS.GiftShop.Core;

namespace SS.GiftShop.Api.Identity.Models
{
    public class UpdatePasswordViewModel : ChangePasswordViewModel
    {
        [Required]
        [StringLength(AppConstants.EmailLength)]
        public virtual string Email { get; set; }
    }
}
