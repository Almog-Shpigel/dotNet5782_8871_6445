using System;
using System.Runtime.Serialization;

namespace BlApi
{
    [Serializable]
    public class InvalidSlotsException : Exception
    {
        public InvalidSlotsException()
        {
        }

        public InvalidSlotsException(string message) : base(message)
        {
        }

        public InvalidSlotsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSlotsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}