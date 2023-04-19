using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Luqmit3ish.Exceptions
{
    class ServerException : Exception
    {
        public ServerException()
        {
        }

        public ServerException(string message) : base(message)
        {
        }

        public ServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
