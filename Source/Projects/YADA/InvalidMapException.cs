using System;
using System.Runtime.Serialization;

namespace Yada
{
    [Serializable]
    public class InvalidMapException : Exception
    {
        public InvalidMapException() {}

        public InvalidMapException(string message)
            : base(message) {}

        public InvalidMapException(string message, Exception inner)
            : base(message, inner) {}

        protected InvalidMapException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}
    }
}