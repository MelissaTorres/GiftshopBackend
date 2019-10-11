using System;
using System.Runtime.Serialization;

namespace SS.GiftShop.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Represents errors that occur due to entities that don't exist for a given key.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public Type EntityType { get; }

        public object Key { get; }

        public EntityNotFoundException()
            : base(Properties.Resources.EntityNotFoundErrorMessage)
        {
        }

        public EntityNotFoundException(Type entityType, object key)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public EntityNotFoundException(string message, Type entityType, object key) : base(message)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public EntityNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EntityNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public static EntityNotFoundException For<TEntity>(Guid key)
            where TEntity : class
        {
            return new EntityNotFoundException(typeof(TEntity), key);
        }

        public static EntityNotFoundException For<TEntity>(Guid key, string message)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new EntityNotFoundException(message, typeof(TEntity), key);
        }
    }
}
