﻿using System;
using System.Runtime.Serialization;

namespace BlApi
{
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
}