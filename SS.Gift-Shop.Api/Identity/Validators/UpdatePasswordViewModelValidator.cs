using FluentValidation;
using SS.GiftShop.Api.Identity.Models;
using SS.GiftShop.Core;

namespace SS.GiftShop.Api.Identity.Validators
{
    public sealed class UpdatePasswordViewModelValidator : SetPasswordModelBaseValidator<UpdatePasswordViewModel>
    {
        public UpdatePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(AppConstants.EmailLength);
        }
    }
}
