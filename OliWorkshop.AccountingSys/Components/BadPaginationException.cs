using System;
using System.Runtime.Serialization;

namespace OliWorkshop.AccountingSys.Components
{
    /// <summary>
    /// Indicates that the pagination parameters is bad
    /// </summary>
    [Serializable]
    public class BadPaginationException : Exception
    {
        public BadPaginationException()
        {
        }

        public BadPaginationException(string message) : base(message)
        {
        }

        public BadPaginationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadPaginationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}