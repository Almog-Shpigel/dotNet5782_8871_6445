using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class NoAvailableStation : Exception
    {
        public NoAvailableStation()
        {
        }

        public NoAvailableStation(string message) : base(message)
        {
        }

        public NoAvailableStation(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoAvailableStation(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}