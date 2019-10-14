using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SS.GiftShop.Application.Products.Models;

namespace SS.GiftShop.Application.Models.Validators
{
    public sealed class DeleteProductModelValidator : AbstractValidator<DeleteProductModel>
    {
        public DeleteProductModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
