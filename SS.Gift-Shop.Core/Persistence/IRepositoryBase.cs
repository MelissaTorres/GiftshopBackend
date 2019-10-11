using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SS.GiftShop.Core.Persistence
{
    public interface IRepositoryBase
    {
        /*
        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string.</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        int ExecuteCommand(string sql, params object[] parameters);

        /// <summary>
        /// Asynchronously executes the given DDL/DML command against the database.
        /// As with any API that accepts SQL it is important to parameterize any user
        /// input to protect against a SQL injection attack. You can include parameter
        /// place holders in the SQL query string and then supply parameter values as
        /// additional arguments. Any parameter values you supply will automatically
        /// be converted to a DbParameter.  context.Database.ExecuteSqlCommandAsync("UPDATE
        /// dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor); Alternatively,
        /// you can also construct a DbParameter and supply it to SqlQuery. This allows
        /// you to use named parameters in the SQL query string.  context.Database.ExecuteSqlCommandAsync("UPDATE
        /// dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author",
        /// userSuppliedAuthor));
        /// </summary>
        /// <param name="sql">The command string.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains
        /// the result returned by the database after executing the command.</returns>
        /// <remarks>Multiple active operations on the same context instance are not supported.
        /// Use 'await' to ensure that any asynchronous operations have completed before
        /// calling another method on this context.  If there isn't an existing local
        /// transaction a new transaction will be used to execute the command.</remarks>
        Task<int> ExecuteCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters);
        */

        /// <summary>
        /// Returns a query using the specified predicate.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The include expressions.</param>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate,
            IEnumerable<Expression<Func<T, object>>> includes) where T : class;

        /*
        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type. The type
        /// can be any type that has properties that match the names of the columns returned from
        /// the query, or can be a simple primitive type. The type does not have to be an entity
        /// type. The results of this query are never tracked by the context even if the type of
        /// object returned is an entity type.
        /// </summary>
        /// <typeparam name="T">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>
        /// A System.Collections.Generic.IEnumerable&lt;T&gt; object that will execute the
        /// query when it is enumerated.
        /// </returns>
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        */

        T Get<T, TKey>(TKey id)
            where T : class
            where TKey : IEquatable<TKey>;

        IQueryable<T> Include<T, TProperty>(IQueryable<T> query,
            Expression<Func<T, TProperty>> navigationPropertyPath)
            where T : class;

        #region Async Methods

        Task<T> SingleAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<T> FirstAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task ForEachAsync<T>(IQueryable<T> query, Action<T> action, CancellationToken cancellationToken = default);

        Task<bool> AllAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<List<T>> ListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        #endregion Async Methods
    }
}
