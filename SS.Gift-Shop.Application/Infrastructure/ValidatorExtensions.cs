using System.Collections;
using FluentValidation;

namespace SS.GiftShop.Application.Infrastructure
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TCollection> ListNotEmpty<T, TCollection>(this IRuleBuilder<T, TCollection> ruleBuilder)
            where TCollection : ICollection
        {
            return ruleBuilder.Must(list => list.Count > 0);
        }
    }
}
