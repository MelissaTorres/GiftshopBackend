using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SS.GiftShop.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Occurs when an object does not pass validation criteria.
    /// </summary>
    [Serializable]
    public class ObjectValidationException : Exception
    {
        private static readonly KeyValuePair<string, string>[] Empty = new KeyValuePair<string, string>[0];
        private readonly IEnumerable<KeyValuePair<string, string>> _errors;

        public IEnumerable<KeyValuePair<string, string>> Errors
        {
            get { return _errors ?? Empty; }
        }

        public ObjectValidationException(IEnumerable<KeyValuePair<string, string>> errors)
        {
            _errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public ObjectValidationException(string errorKey, string errorMessage)
            : base(errorMessage)
        {
            if (errorKey == null)
            {
                throw new ArgumentNullException(nameof(errorKey));
            }

            if (errorMessage == null)
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            _errors = new[] { KeyValuePair.Create(errorKey, errorMessage) };
        }

        public ObjectValidationException(string message) : base(message)
        {
        }

        public ObjectValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ObjectValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
