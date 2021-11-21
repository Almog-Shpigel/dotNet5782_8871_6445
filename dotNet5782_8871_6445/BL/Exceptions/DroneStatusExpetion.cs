﻿using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    internal class DroneStatusExpetion : Exception
    {
        public DroneStatusExpetion()
        {
        }

        public DroneStatusExpetion(string message) : base(message)
        {
        }

        public DroneStatusExpetion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneStatusExpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}