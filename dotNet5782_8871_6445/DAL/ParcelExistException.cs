using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class ParcelExistException : Exception
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

        protected ParcelExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}