using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Luqmit3ish.Exceptions
{
    class NotAuthorizedException : Exception
    {
        public NotAuthorizedException()
        {
        }

        public NotAuthorizedException(string message) : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAuthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
