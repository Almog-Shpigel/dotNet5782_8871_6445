using System;
using System.Runtime.Serialization;

namespace BlApi
{
    [Serializable]
    public class InvalidIDException : Exception
    {
        public InvalidIDException()
        {
        }

        public InvalidIDException(string message) : base(message)
        {
        }

        public InvalidIDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}