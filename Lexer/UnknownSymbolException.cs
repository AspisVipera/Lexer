using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    class UnknownSymbolException : Exception
    {
        public UnknownSymbolException()
        {
        }

        public UnknownSymbolException(char c) : base(((int) c).ToString())
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
