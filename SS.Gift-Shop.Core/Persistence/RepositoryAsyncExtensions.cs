using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SS.GiftShop.Core.Persistence
{
    public static class RepositoryAsyncExtensions
    {
        public static Task<T> FirstAsync<T>(this IRepositoryBase repository, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var query = repository.Query(null, includes);
            return repository.FirstAsync(query);
        }

        public static Task<T> FirstAsync<T>(this IRepositoryBase repository, Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (includes is null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            var query = repository.Query(condition, includes);
            return repository.FirstAsync(query);
        }

        public static Task<T> SingleAsync<T>(this IRepositoryBase repository, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var query = repository.Query(null, includes);
            return repository.SingleAsync(query);
        }

        public static Task<T> SingleAsync<T>(this IRepositoryBase repository, Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (includes is null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            var query = repository.Query(condition, includes);
            return repository.SingleAsync(query);
        }

        public static Task<bool> AnyAsync<T>(this IRepositoryBase repository, CancellationToken cancellationToken = default)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var query = repository.Query<T>();
            return repository.AnyAsync(query, cancellationToken);
        }

        public static Task<bool> AnyAsync<T>(this IRepositoryBase repository, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var query = repository.Query(condition);
            return repository.AnyAsync(query, cancellationToken);
        }

        public static Task<bool> AllAsync<T>(this IRepositoryBase repository, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var query = repository.Query<T>();
            return repository.AllAsync(query, condition, cancellationToken);
        }

        public static Task ForEachAsync<T>(this IRepositoryBase repository, Action<T> action, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (includes is null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            var query = repository.Query(null, includes);
            return repository.ForEachAsync(query, action);
        }

        public static Task ForEachAsync<T>(this IRepositoryBase repository, Expression<Func<T, bool>> condition, Action<T> action, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (includes is null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            var query = repository.Query(condition, includes);
            return repository.ForEachAsync(query, action);
        }

        public static Task<int> CountAsync<T>(this IRepositoryBase repository)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var query = repository.Query<T>();
            return repository.CountAsync(query);
        }

        public static Task<int> CountAsync<T>(this IRepositoryBase repository, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var query = repository.Query(condition);
            return repository.CountAsync(query, cancellationToken);
        }

        public static Task<List<T>> ListAsync<T>(this IRepositoryBase repository, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (includes is null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            var query = repository.Query(null, includes);
            return repository.ListAsync(query);
        }

        public static Task<List<T>> ListAsync<T>(this IRepositoryBase repository, Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (includes is null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            var query = repository.Query(condition, includes);
            return repository.ListAsync(query);
        }
    }
}
