using System;
using System.Runtime.Serialization;

namespace NESI.Common.Exceptions
{
    /// <summary>
    /// Base class for all validation related exceptions
    /// </summary>
    public class NesiValidationException : NesiException
    {
        public NesiValidationException()
        {
        }

        public NesiValidationException(string message) : base(message)
        {
        }

        public NesiValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NesiValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}