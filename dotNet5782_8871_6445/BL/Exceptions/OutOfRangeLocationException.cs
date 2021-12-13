using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class OutOfRangeLocationException : Exception
    {
        public OutOfRangeLocationException()
        {
        }

        public OutOfRangeLocationException(string message) : base(message)
        {
        }

        public OutOfRangeLocationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutOfRangeLocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}