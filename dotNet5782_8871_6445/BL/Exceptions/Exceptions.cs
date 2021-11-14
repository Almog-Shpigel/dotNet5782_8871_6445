using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        [Serializable]
        internal class DroneExistException : Exception
        {
            public DroneExistException()
            {
            }

            public DroneExistException(string message) : base(message)
            {
            }

            public DroneExistException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected DroneExistException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        [Serializable]
        internal class StationExistException : Exception
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
        [Serializable]
        internal class WrongInputException : Exception
        {
            public WrongInputException()
            {
            }

            public WrongInputException(string message) : base(message)
            {
            }

            public WrongInputException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected WrongInputException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
