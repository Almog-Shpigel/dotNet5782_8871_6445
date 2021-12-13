using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class StationExistExceptionBL : Exception
    {
        public StationExistExceptionBL()
        {
        }

        public StationExistExceptionBL(string message) : base(message)
        {
        }

        public StationExistExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StationExistExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}