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
        class Exceptions
        {
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
        }
    }
}
