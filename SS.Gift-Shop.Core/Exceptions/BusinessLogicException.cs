using System;
using System.Runtime.Serialization;

namespace SS.GiftShop.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an application exception that arises from business rules validation and does not need to be logged.
    /// Furthermore, the message is intended to be displayed directly to the user explaining the nature of the error
    /// in a user-friendly fashion.
    /// </summary>
    /// <remarks>This exception is meant for business rules that are more complex than simple object validations.
    /// For object validation exceptions, use <see cref="T:SS.GiftShop.Core.Exceptions.ObjectValidationException" />.</remarks>
    [Serializable]
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException()
        {
        }

        public BusinessLogicException(string message) : base(message)
        {
        }

        public BusinessLogicException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BusinessLogicException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
