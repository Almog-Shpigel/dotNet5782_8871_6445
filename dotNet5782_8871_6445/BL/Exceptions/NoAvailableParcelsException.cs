using System;
using System.Runtime.Serialization;

namespace BlApi
{
    [Serializable]
    public class NoAvailableParcelsException : Exception
    {
        public NoAvailableParcelsException()
        {
        }

        public NoAvailableParcelsException(string message) : base(message)
        {
        }

        public NoAvailableParcelsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoAvailableParcelsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}