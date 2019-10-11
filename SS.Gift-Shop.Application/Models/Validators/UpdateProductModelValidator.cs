using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SS.GiftShop.Core;

namespace SS.GiftShop.Application.Models.Validators
{
    public sealed class UpdateProductModelValidator : AbstractValidator<UpdateProductModel>
    {
        public UpdateProductModelValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.Description)
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.Characteristics)
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.Price);
        }
    }
}
