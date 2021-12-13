using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    public class StationExistException : Exception
    {
        public StationExistException()
        {
        }

        public StationExistException(string message) : base(message)
        {
        }

        public StationExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StationExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}