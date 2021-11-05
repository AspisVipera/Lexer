using System.Collections.Generic;

namespace Lexer
{
    struct Token
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
            SUBEXPRESSION,
            OPERAND,
            UNARYOP,
            BINARYOP,
            LETTER,
            READEX,
            WRITEEX,
            CONDITION
        }

        public Name Type { get; }
        public string Value { get; }
        public List<Token> Childs { get; }

        public Token(Name type, string value, IEnumerable<Token> childs = null)
        {
            Type = type;
            Value = value;
            Childs = (childs == null) ? null : new List<Token>(childs);
        }
    }
}
