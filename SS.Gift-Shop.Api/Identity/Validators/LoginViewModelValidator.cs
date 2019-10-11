using FluentValidation;
using SS.GiftShop.Api.Identity.Models;

namespace SS.GiftShop.Api.Identity.Validators
{
    public sealed class LoginViewModelValidator : EmailModelBaseValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
