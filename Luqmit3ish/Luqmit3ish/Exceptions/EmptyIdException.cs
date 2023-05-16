using System;
using System.Runtime.Serialization;

namespace Luqmit3ish.Exceptions
{
    public class EmptyIdException : Exception
    {
        public EmptyIdException()
        {
        }

        public EmptyIdException(string message) : base(message)
        {
        }

        public EmptyIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

