using FluentValidation;
using SS.GiftShop.Api.Identity.Models;

namespace SS.GiftShop.Api.Identity.Validators
{
    public sealed class ActivateAccountViewModelValidator : AbstractValidator<ActivateAccountViewModel>
    {
        public ActivateAccountViewModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
