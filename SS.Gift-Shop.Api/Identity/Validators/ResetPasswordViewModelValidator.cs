using FluentValidation;
using SS.GiftShop.Api.Identity.Models;
using SS.GiftShop.Core;

namespace SS.GiftShop.Api.Identity.Validators
{
    public sealed class ResetPasswordViewModelValidator : SetPasswordModelBaseValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(AppConstants.EmailLength);
        }
    }
}