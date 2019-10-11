using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SS.GiftShop.Core;

namespace SS.GiftShop.Application.Models.Validators
{
    public sealed class AddOrderDetailsModelValidator : AbstractValidator<AddOrderDetailModel>
    {
        public AddOrderDetailsModelValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty();

            RuleFor(x => x.Total);

            RuleFor(x => x.User)
                .MaximumLength(AppConstants.StandardValueLength);
        }
    }
}
