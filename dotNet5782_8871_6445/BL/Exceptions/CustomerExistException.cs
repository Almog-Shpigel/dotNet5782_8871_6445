using System;
using System.Runtime.Serialization;

namespace IBL
{
    namespace BO
    {
        [Serializable]
        internal class CustomerExistException : Exception
        {
            public CustomerExistException()
            {
            }

            public CustomerExistException(string message) : base(message)
            {
            }

            public CustomerExistException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected CustomerExistException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
