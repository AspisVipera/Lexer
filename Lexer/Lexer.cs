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

            tokens.Add(new Token(Token.Name.I, ""));

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    if (stack.Count != 0)
                    {
                        if (stack.Count > 7)
                        {
                            Console.WriteLine(string.Concat(stack) + " переменная не может иметь длину более 8 символов.");
                            throw new Exception();
                        }

                        tokens.Add(ReduceLetterToken(string.Join("", stack.Reverse())));
                        stack.Clear();
                    }

                    Token nextToken = ReduceCharToken(c);
                    if (nextToken.Type != Token.Name.SPACE || tokens.Last().Type != Token.Name.SPACE)
                    {
                        tokens.Add(nextToken);
                    }
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
                "and" => Token.Name.BINARYAND,
                "or" => Token.Name.BINARYOR,
                "equ" => Token.Name.BINARYEQU,
                "read" => Token.Name.READEX,
                "write" => Token.Name.WRITEEX,
                "if" => Token.Name.IF,
                "then" => Token.Name.THEN,
                "else" => Token.Name.ELSE,
                "endif" => Token.Name.ENDIF,
                _ => Token.Name.IDENT
            };

            return new Token(name, value);
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
                '\t' => Token.Name.SPACE,
                '\n' => Token.Name.SPACE,
                _ => throw new UnknownSymbolException(c.ToString())
            };

            return new Token(name, c.ToString());
        }
    }
}
