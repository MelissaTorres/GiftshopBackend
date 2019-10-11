using System;

namespace SS.GiftShop.Domain.Model
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Not equal
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>Result</returns>
        public static bool operator !=(Entity x, Entity y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Equal
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>Result</returns>
        public static bool operator ==(Entity x, Entity y)
        {
            return Equals(x, y);
        }

        public void EnsureId()
        {
            if (Id == default(Guid))
            {
                Id = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        public bool Equals(Entity other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            if (Id == default(Guid))
            {
                return base.GetHashCode();
            }

            return GetType().GetHashCode() ^ Id.GetHashCode();
        }

        /// <summary>
        /// Is transient
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Result</returns>
        private static bool IsTransient(Entity obj)
        {
            return obj != null && Equals(obj.Id, default(Guid));
        }

        /// <summary>
        /// Get unproxied type
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }
    }
}
