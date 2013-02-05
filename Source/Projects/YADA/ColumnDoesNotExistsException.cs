using System;
using System.Runtime.Serialization;

namespace Yada
{
    [Serializable]
    public class ColumnDoesNotExistsException : Exception
    {
        public ColumnDoesNotExistsException() {}

        public ColumnDoesNotExistsException(string message)
            : base(message) {}

        public ColumnDoesNotExistsException(string message, Exception inner)
            : base(message, inner) {}

        protected ColumnDoesNotExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}
    }
}