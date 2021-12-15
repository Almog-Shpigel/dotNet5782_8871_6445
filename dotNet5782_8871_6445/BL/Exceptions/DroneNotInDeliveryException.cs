﻿using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class DroneNotInDeliveryException : Exception
    {
        public DroneNotInDeliveryException()
        {
        }

        public DroneNotInDeliveryException(string message) : base(message)
        {
        }

        public DroneNotInDeliveryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneNotInDeliveryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}