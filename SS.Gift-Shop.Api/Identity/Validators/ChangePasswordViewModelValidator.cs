using FluentValidation;
using SS.GiftShop.Api.Identity.Models;

namespace SS.GiftShop.Api.Identity.Validators
{
    public sealed class ChangePasswordViewModelValidator : SetPasswordModelBaseValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty();
        }
    }
}
