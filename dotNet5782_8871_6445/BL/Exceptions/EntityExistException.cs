using System;
using System.Runtime.Serialization;

namespace BlApi
{
    [Serializable]
    internal class EntityExistException : Exception
    {
        public EntityExistException()
        {
        }

        public EntityExistException(string message) : base(message)
        {
        }

        public EntityExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}