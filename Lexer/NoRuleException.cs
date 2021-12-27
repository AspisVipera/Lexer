using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    class NoRuleException : Exception
    {
        public NoRuleException()
        {
        }

        public NoRuleException(string message) : base(message)
        {
        }

        public NoRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoRuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
