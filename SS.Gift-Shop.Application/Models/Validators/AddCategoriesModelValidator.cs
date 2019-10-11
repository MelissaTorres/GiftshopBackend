using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SS.GiftShop.Core;

namespace SS.GiftShop.Application.Models.Validators
{
    public sealed class AddCategoriesModelValidator : AbstractValidator<AddCategoryModel>
    {
        public AddCategoriesModelValidator()
        {
            RuleFor(x => x.CategoryName)
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.Products);
        }
    }
}
