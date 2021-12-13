using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class ParcelTimesException : Exception
    {
        public ParcelTimesException()
        {
        }

        public ParcelTimesException(string message) : base(message)
        {
        }

        public ParcelTimesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParcelTimesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}