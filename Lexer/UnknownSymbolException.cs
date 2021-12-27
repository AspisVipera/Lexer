using System;
using System.Runtime.Serialization;

namespace Lexer
{
    class UnknownSymbolException : Exception
    {
        public UnknownSymbolException()
        {
        }

        public UnknownSymbolException(string message) : base(message)
        {
        }

        public UnknownSymbolException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownSymbolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
