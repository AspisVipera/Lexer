using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    internal class VarAlreadyDefinedException : Exception
    {
        public VarAlreadyDefinedException()
        {
        }

        public VarAlreadyDefinedException(string message) : base(message)
        {
        }

        public VarAlreadyDefinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VarAlreadyDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
