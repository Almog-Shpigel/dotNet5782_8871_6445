using System;
using System.Runtime.Serialization;

namespace BlApi
{
    [Serializable]
    internal class InvalidDeleteException : Exception
    {
        public InvalidDeleteException()
        {
        }

        public InvalidDeleteException(string message) : base(message)
        {
        }

        public InvalidDeleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}