using FluentValidation;
using SS.GiftShop.Api.Identity.Models;
using SS.GiftShop.Core;

namespace SS.GiftShop.Api.Identity.Validators
{
    public abstract class EmailModelBaseValidator<T> : AbstractValidator<T>
        where T : EmailModelBase
    {
        protected EmailModelBaseValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(AppConstants.EmailLength);
        }
    }
}
