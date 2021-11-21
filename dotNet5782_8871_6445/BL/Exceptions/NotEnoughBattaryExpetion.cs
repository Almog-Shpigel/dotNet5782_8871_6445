using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    internal class NotEnoughBattaryExpetion : Exception
    {
        public NotEnoughBattaryExpetion()
        {
        }

        public NotEnoughBattaryExpetion(string message) : base(message)
        {
        }

        public NotEnoughBattaryExpetion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughBattaryExpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}