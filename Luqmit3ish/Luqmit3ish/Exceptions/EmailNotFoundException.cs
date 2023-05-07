using System;
using System.Runtime.Serialization;

namespace Luqmit3ish.Exceptions
{
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException()
        {
        }

        public EmailNotFoundException(string message) : base(message)
        {
        }

        public EmailNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmailNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

