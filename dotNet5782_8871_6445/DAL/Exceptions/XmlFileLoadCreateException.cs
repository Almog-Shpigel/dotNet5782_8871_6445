using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    internal class XmlFileLoadCreateException : Exception
    {
        private string filePath;
        private string v;
        private Exception ex;

        public XmlFileLoadCreateException()
        {
        }

        public XmlFileLoadCreateException(string message) : base(message)
        {
        }

        public XmlFileLoadCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public XmlFileLoadCreateException(string filePath, string v, Exception ex)
        {
            this.filePath = filePath;
            this.v = v;
            this.ex = ex;
        }

        protected XmlFileLoadCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}