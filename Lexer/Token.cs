using System.Collections.Generic;

namespace Lexer
{
    public class Token
    {
        public enum Name
        {
            I,
            CONSTANT,
            COLON,
            SEMICOLON,
            DOT,
            COMMA,
            ASSIGNMENTCHAR,
            OPENINGBRACKET,
            CLOSINGBRACKET,
            SPACE,
            BEGIN,
            VAR,
            END,
            LOGICAL,
            NOT,
            AND,
            OR,
            EQU,
            READ,
            WRITE,
            IF,
            THEN,
            ELSE,
            ENDIF,
            IDENT,
            PROGRAM,
            VARDEFINITION,
            CALCDEFINITION,
            ASSIGNMENTLIST,
            VARLIST,
            ASSIGNMENT,
            OPERATOR,
            EXPRESSION,
            EQUEXPRESSION,
            OREXPRESSION,
            ANDEXPRESSION,
            ATOMEXPRESSION,
            OPERAND,
            UNARYOP,
            BINARYEQU,
            BINARYOR,
            BINARYAND,
            LETTER,
            READEX,
            WRITEEX,
            CONDITION
        }

        public Token(Name type, string value, IEnumerable<Token> children = null)
        {
            Type = type;
            Value = value;
            Children = (children == null) ? null : new List<Token>(children);
        }

        public Name Type { get; }
        public string Value { get; }
        public List<Token> Children { get; }
    }
}
