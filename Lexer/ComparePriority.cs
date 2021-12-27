using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    internal class ComparePriority : EqualityComparer<Token.Name>
    {
        public override bool Equals(Token.Name first, Token.Name second)
        {
            switch (second)
            {
                case Token.Name.EQUEXPRESSION:
                    return (first == Token.Name.EQUEXPRESSION ||
                        first == Token.Name.OREXPRESSION ||
                        first == Token.Name.ANDEXPRESSION ||
                        first == Token.Name.ATOMEXPRESSION);
                case Token.Name.OREXPRESSION:
                    return (first == Token.Name.OREXPRESSION ||
                        first == Token.Name.ANDEXPRESSION ||
                        first == Token.Name.ATOMEXPRESSION);
                case Token.Name.ANDEXPRESSION:
                    return (first == Token.Name.ANDEXPRESSION ||
                        first == Token.Name.ATOMEXPRESSION);
                default:
                    return second == first;
            }
        }

        public override int GetHashCode([DisallowNull] Token.Name obj)
        {
            throw new NotImplementedException();
        }
    }
}
