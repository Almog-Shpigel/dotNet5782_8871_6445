using System;
using System.Runtime.Serialization;

namespace BlApi
{
    [Serializable]
    public class NotEnoughBatteryExpetion : Exception
    {
        public NotEnoughBatteryExpetion()
        {
        }

        public NotEnoughBatteryExpetion(string message) : base(message)
        {
        }

        public NotEnoughBatteryExpetion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughBatteryExpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}