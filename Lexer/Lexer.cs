using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexer
{
    static class Lexer
    {
        public static List<Token> Run(string input)
        {
            var tokens = new List<Token>();
            var stack = new Stack<char>();

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    if (stack.Count != 0)
                    {
                        tokens.Add(ReduceLetterToken(string.Join("", stack.Reverse())));
                        stack.Clear();
                    }

                    tokens.Add(ReduceCharToken(c));
                }
                else
                {
                    stack.Push(c);
                }
            }

            if (stack.Count != 0)
            {
                tokens.Add(ReduceLetterToken(string.Join("", stack.Reverse())));
            }

            return tokens;
        }

        private static Token ReduceLetterToken(string value)
        {
            Token.Name name = value switch
            {
                "begin" => Token.Name.BEGIN,
                "var" => Token.Name.VAR,
                "end" => Token.Name.END,
                "logical" => Token.Name.LOGICAL,
                "not" => Token.Name.UNARYOP,
                "and" => Token.Name.BINARYOP,
                "or" => Token.Name.BINARYOP,
                "equ" => Token.Name.BINARYOP,
                "read" => Token.Name.READEX,
                "write" => Token.Name.WRITEEX,
                "if" => Token.Name.IF,
                "then" => Token.Name.THEN,
                "else" => Token.Name.ELSE,
                "endif" => Token.Name.ENDIF,
                _ => Token.Name.IDENT
            };

            return new Token(
                name, 
                ((name == Token.Name.IDENT |
                (name == Token.Name.UNARYOP) |
                (name == Token.Name.BINARYOP)) ? value : null));
        }

        private static Token ReduceCharToken(char c)
        {
            Token.Name name = c switch
            {
                '0' => Token.Name.CONSTANT,
                '1' => Token.Name.CONSTANT,
                ':' => Token.Name.COLON,
                ';' => Token.Name.SEMICOLON,
                '.' => Token.Name.DOT,
                ',' => Token.Name.COMMA,
                '=' => Token.Name.ASSIGNMENTCHAR,
                '(' => Token.Name.OPENINGBRACKET,
                ')' => Token.Name.CLOSINGBRACKET,
                ' ' => Token.Name.SPACE,
                _ => throw new UnknownSymbolException()
            };

            return new Token(name, name == Token.Name.CONSTANT ? c.ToString() : null);
        }
    }
}
