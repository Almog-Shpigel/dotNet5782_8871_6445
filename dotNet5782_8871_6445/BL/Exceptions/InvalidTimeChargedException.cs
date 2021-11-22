using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    internal class InvalidTimeChargedException : Exception
    {
        public InvalidTimeChargedException()
        {
        }

        public InvalidTimeChargedException(string message) : base(message)
        {
        }

        public InvalidTimeChargedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTimeChargedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}