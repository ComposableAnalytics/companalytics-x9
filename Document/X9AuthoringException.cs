using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.Document
{
    [Serializable]
    public class X9AuthoringException : Exception
    {
        public X9AuthoringException() : base() { }
        public X9AuthoringException(string message) : base(message) { }
        public X9AuthoringException(string message, Exception innerException) : base(message, innerException) { }
        protected X9AuthoringException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
