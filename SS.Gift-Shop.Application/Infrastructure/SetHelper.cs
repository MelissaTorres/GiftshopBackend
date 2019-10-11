using System;
using System.Collections.Generic;
using System.Linq;

namespace SS.GiftShop.Application.Infrastructure
{
    public static class SetHelper
    {
        /// <summary>
        /// Creates an instance of <see cref="SetHelper{T}"/> class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existing">The collection of entities that exists in the data store.</param>
        /// <param name="all">The collection of all entities that comes from the client.</param>
        /// <param name="equalityComparer">The equality comparer or <c>null</c>.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="existing"/> or <paramref name="all"/> is <c>null</c>.
        /// </exception>
        public static SetHelper<T> Create<T>(IEnumerable<T> existing, IEnumerable<T> all,
            IEqualityComparer<T> equalityComparer = null)
        {
            if (existing == null)
            {
                throw new ArgumentNullException(nameof(existing));
            }

            if (all == null)
            {
                throw new ArgumentNullException(nameof(all));
            }

            return new SetHelper<T>(existing, all, equalityComparer ?? EqualityComparer<T>.Default);
        }
    }

    /// <summary>
    /// Helper class to detect differences in two collections (sets).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SetHelper<T>
    {
        private readonly IEnumerable<T> _existing;
        private readonly IEnumerable<T> _all;
        private readonly IEqualityComparer<T> _equalityComparer;

        /// <summary>
        /// Gets the elements of the existing collection that are not present in the new collection.
        /// </summary>
        /// <value>
        /// The missing.
        /// </value>
        public IEnumerable<T> Missing
        {
            get { return _existing.Except(_all, _equalityComparer).ToList(); }
        }

        /// <summary>
        /// Gets the elements of the new collection that are not present in the existing collection.
        /// </summary>
        /// <value>
        /// The added.
        /// </value>
        public IEnumerable<T> Added
        {
            get { return _all.Except(_existing, _equalityComparer).ToList(); }
        }

        /// <summary>
        /// Gets the common elements of the new collection and the existing collection.
        /// </summary>
        /// <value>
        /// The updated.
        /// </value>
        public IEnumerable<T> Updated
        {
            get { return _existing.Intersect(_all, _equalityComparer); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetHelper{T}"/> class.
        /// </summary>
        /// <param name="existing">The existing.</param>
        /// <param name="all">All.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        public SetHelper(IEnumerable<T> existing, IEnumerable<T> all, IEqualityComparer<T> equalityComparer)
        {
            _existing = existing;
            _all = all;
            _equalityComparer = equalityComparer;
        }

        public IEnumerable<Tuple<T, T>> GetIntersection()
        {
            var all = _all.ToDictionary(x => x, x => x, _equalityComparer);
            foreach (var item in Updated)
            {
                yield return Tuple.Create(item, all[item]);
            }
        }
    }
}
