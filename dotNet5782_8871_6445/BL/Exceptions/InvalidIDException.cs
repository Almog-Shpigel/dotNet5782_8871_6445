using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class InvalidIDException : Exception
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