using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    internal class VarNotDefinedException : Exception
    {
        public VarNotDefinedException()
        {
        }

        public VarNotDefinedException(string message) : base(message)
        {
        }

        public VarNotDefinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VarNotDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
