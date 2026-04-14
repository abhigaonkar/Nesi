using System;
using System.Runtime.Serialization;

namespace NESI.Common.Exceptions
{
    /// <summary>
    /// Base class for all Nesi exceptions
    /// </summary>
    public class NesiException : Exception
    {
        public NesiException()
        {}

        public NesiException(string message) : base(message)
        {}

        public NesiException(string message, Exception innerException) : base(message, innerException)
        {}

        protected NesiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}
