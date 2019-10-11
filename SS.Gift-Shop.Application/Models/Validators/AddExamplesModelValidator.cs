using FluentValidation;
using SS.GiftShop.Application.Examples.Models;
using SS.GiftShop.Core;

namespace SS.GiftShop.Application.Examples.Commands.Add
{
    public sealed class AddExamplesModelValidator : AbstractValidator<AddOrderDetail>
    {
        public AddExamplesModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(AppConstants.EmailLength);
        }
    }
}
