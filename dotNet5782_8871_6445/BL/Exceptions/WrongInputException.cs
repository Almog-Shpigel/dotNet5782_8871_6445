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
