using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SS.GiftShop.Core;

namespace SS.GiftShop.Application.Models.Validators
{
    public sealed class AddOrdersModelValidator : AbstractValidator<AddOrderModel>
    {
        public AddOrdersModelValidator()
        {
            RuleFor(x => x.UserId)
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.OrderDate)
                .NotEmpty();
        }
    }
}
