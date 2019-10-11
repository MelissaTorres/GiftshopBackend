using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SS.GiftShop.Core.Persistence
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Returns a query without a predicate
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static IQueryable<T> Query<T>(this IRepositoryBase repository)
            where T : class
        {
            return repository.Query(default(Expression<Func<T, bool>>), default(IEnumerable<Expression<Func<T, object>>>));
        }

        /// <summary>
        /// Returns a query without a predicate and a number of include expressions.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="includes">The include expressions.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static IQueryable<T> Query<T>(this IRepositoryBase repository, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            return repository.Query(default(Expression<Func<T, bool>>), includes);
        }

        /// <summary>
        /// Returns a query without a predicate and a number of include expressions.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The include expressions.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> is <c>null</c>
        /// </exception>
        public static IQueryable<T> Query<T>(this IRepositoryBase repository, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            return repository.Query(predicate, includes);
        }

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="entities">The entities to be added.</param>
        public static IEnumerable<T> Add<T>(this IRepository repository, IEnumerable<T> entities) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return entities.Select(repository.Add);
        }

        /// <summary>
        /// Removes the entity specified in the expression.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="predicate">The expression.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> or <paramref name="predicate" /> is <c>null</c>
        /// </exception>
        public static void Remove<T>(this IRepository repository, Expression<Func<T, bool>> predicate) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var entity in repository.Query<T>().Where(predicate))
            {
                repository.Remove(entity);
            }
        }

        /// <summary>
        /// Removes the specified entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="repository">The <see cref="IRepository"/> instance.</param>
        /// <param name="entities">The entities to be deleted.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="repository" /> or <paramref name="entities" /> is <c>null</c>
        /// </exception>
        public static void RemoveAll<T>(this IRepository repository, IEnumerable<T> entities) where T : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                repository.Remove(entity);
            }
        }
    }
}
