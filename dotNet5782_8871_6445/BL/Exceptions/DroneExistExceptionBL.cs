using System;
using System.Runtime.Serialization;

namespace BlApi
{
    [Serializable]
    public class DroneExistExceptionBL : Exception
    {
        public DroneExistExceptionBL()
        {
        }

        public DroneExistExceptionBL(string message) : base(message)
        {
        }

        public DroneExistExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneExistExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}