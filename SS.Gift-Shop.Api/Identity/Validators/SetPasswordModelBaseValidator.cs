using FluentValidation;
using SS.GiftShop.Api.Identity.Models;

namespace SS.GiftShop.Api.Identity.Validators
{
    public abstract class SetPasswordModelBaseValidator<T> : AbstractValidator<T>
        where T : SetPasswordModelBase
    {
        protected SetPasswordModelBaseValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(AuthConstants.MinPasswordLength)
                .MaximumLength(AuthConstants.MaxPasswordLength);
        }
    }
}
