using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SS.GiftShop.Application.Infrastructure
{
    public static class QueryableExtensions
    {
        private static readonly MethodInfo OrderByMethod;
        private static readonly MethodInfo OrderByDescendingMethod;
        private static readonly MethodInfo ThenByMethod;
        private static readonly MethodInfo ThenByDescendingMethod;

        static QueryableExtensions()
        {
            var queryableMethods = typeof(Queryable).GetMethods();
            OrderByMethod = queryableMethods.Single(method => method.Name == nameof(Queryable.OrderBy) && method.GetParameters().Length == 2);
            OrderByDescendingMethod = queryableMethods.Single(method => method.Name == nameof(Queryable.OrderByDescending) && method.GetParameters().Length == 2);
            ThenByMethod = queryableMethods.Single(method => method.Name == nameof(Queryable.ThenBy) && method.GetParameters().Length == 2);
            ThenByDescendingMethod = queryableMethods.Single(method => method.Name == nameof(Queryable.ThenByDescending) && method.GetParameters().Length == 2);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, ListSortDirection direction)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return OrderBy(source, propertyName, direction, 0) ?? source; // Coalesce to original source in case no ordering could be applied
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ICollection<SortCriterion> ordering)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (ordering == null)
            {
                throw new ArgumentNullException(nameof(ordering));
            }

            var count = 0;

            foreach (var sortCriterion in ordering)
            {
                var temp = OrderBy(source, sortCriterion.Name, sortCriterion.Direction, count);
                if (temp != null)
                {
                    source = temp;
                    count++;
                }
            }

            return source;
        }

        public static IQueryable<T> OrderByOrDefault<T, TProp>(this IQueryable<T> source, ICollection<SortCriterion> ordering,
            Expression<Func<T, TProp>> sortExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (ordering != null && ordering.Any())
            {
                var temp = OrderBy(source, ordering);
                if (!ReferenceEquals(temp, source))
                {
                    // If we didn't get the same reference back, it means at least one ordering was applied
                    return temp;
                }
            }

            return source.OrderBy(sortExpression);
        }

        public static IQueryable<T> OrderByOrDefaultDescending<T, TProp>(this IQueryable<T> source, ICollection<SortCriterion> ordering,
            Expression<Func<T, TProp>> sortExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (ordering != null && ordering.Any())
            {
                var temp = OrderBy(source, ordering);
                if (!ReferenceEquals(temp, source))
                {
                    // If we didn't get the same reference back, it means at least one ordering was applied
                    return temp;
                }
            }
            return source.OrderByDescending(sortExpression);
        }

        private static IQueryable<T> OrderBy<T>(IQueryable<T> source, string propertyName,
            ListSortDirection direction, int times)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return null;
            }

            var props = propertyName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            var type = typeof(T);
            var propType = type;
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (var prop in props)
            {
                var pi = propType.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (pi == null)
                {
                    // Invalid property name
                    return null;
                }
                expr = Expression.Property(expr, pi);
                propType = pi.PropertyType;
            }

            MethodInfo orderingMethod;
            if (times == 0)
            {
                orderingMethod = direction == ListSortDirection.Ascending ? OrderByMethod : OrderByDescendingMethod;
            }
            else
            {
                orderingMethod = direction == ListSortDirection.Ascending ? ThenByMethod : ThenByDescendingMethod;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), propType);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            return (IOrderedQueryable<T>)orderingMethod
                .MakeGenericMethod(type, propType)
                .Invoke(null, new object[] { source, lambda });
        }
    }
}
