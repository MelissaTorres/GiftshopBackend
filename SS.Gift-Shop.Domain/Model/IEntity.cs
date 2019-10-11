using System;

namespace SS.GiftShop.Domain.Model
{
    /// <summary>
    /// Represents a model entity with an unique identifier of type <see cref="System.Guid" />.
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
