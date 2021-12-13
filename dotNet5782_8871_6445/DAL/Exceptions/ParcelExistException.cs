using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    public class ParcelExistException : Exception
    {
        public ParcelExistException()
        {
        }

        public ParcelExistException(string message) : base(message)
        {
        }

        public ParcelExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ParcelExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}