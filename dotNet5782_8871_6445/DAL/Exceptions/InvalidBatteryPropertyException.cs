using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class InvalidBatteryPropertyException : Exception
    {
        public InvalidBatteryPropertyException()
        {
        }

        public InvalidBatteryPropertyException(string message) : base(message)
        {
        }

        public InvalidBatteryPropertyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidBatteryPropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}