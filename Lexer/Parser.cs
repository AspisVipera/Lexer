using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexer
{
    static class Parser
    {
        private readonly struct Rule
        {
            public Rule(Token.Name[] stack, Token.Name[] input, Token.Name output)
            {
                this.stack  = stack;
                this.input  = input;
                this.output = output;
            }

            public Token.Name[] stack  { get; }
            public Token.Name[] input  { get; }
            public Token.Name   output { get; }
        }

        private static readonly Rule[] rules =
        {
            new Rule(
                new Token.Name[]
                { 
                    Token.Name.CALCDEFINITION,
                    Token.Name.SPACE,
                    Token.Name.VARDEFINITION
                },
                null,
                Token.Name.PROGRAM),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.END,
                    Token.Name.SPACE,
                    Token.Name.ASSIGNMENTLIST,
                    Token.Name.SPACE,
                    Token.Name.BEGIN
                },
                null,
                Token.Name.CALCDEFINITION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SEMICOLON,
                    Token.Name.SPACE,
                    Token.Name.LOGICAL,
                    Token.Name.SPACE,
                    Token.Name.COLON,
                    Token.Name.SPACE,
                    Token.Name.VARLIST,
                    Token.Name.SPACE,
                    Token.Name.VAR
                },
                null,
                Token.Name.VARDEFINITION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.IDENT
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.COLON
                },
                Token.Name.VARLIST),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.IDENT
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET,
                    Token.Name.SEMICOLON
                },
                Token.Name.VARLIST),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.VARLIST,
                    Token.Name.SPACE,
                    Token.Name.COMMA,
                    Token.Name.IDENT
                },
                null,
                Token.Name.VARLIST),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ASSIGNMENT
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.ELSE
                },
                Token.Name.ASSIGNMENTLIST),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ASSIGNMENT
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.ENDIF
                },
                Token.Name.ASSIGNMENTLIST),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ASSIGNMENT
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.END
                },
                Token.Name.ASSIGNMENTLIST),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ASSIGNMENTLIST,
                    Token.Name.SPACE,
                    Token.Name.ASSIGNMENT
                },
                null,
                Token.Name.ASSIGNMENTLIST),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SEMICOLON,
                    Token.Name.SPACE,
                    Token.Name.LOGICAL,
                    Token.Name.SPACE,
                    Token.Name.COLON,
                    Token.Name.SPACE,
                    Token.Name.VARLIST,
                    Token.Name.SPACE,
                    Token.Name.VAR
                },
                null,
                Token.Name.VARDEFINITION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.OPERATOR
                },
                null,
                Token.Name.ASSIGNMENT),
            
            new Rule(
                new Token.Name[]
                {
                    Token.Name.SEMICOLON,
                    Token.Name.EXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.ASSIGNMENTCHAR,
                    Token.Name.SPACE,
                    Token.Name.IDENT
                },
                null,
                Token.Name.ASSIGNMENT),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.READ
                },
                null,
                Token.Name.OPERATOR),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.WRITE
                },
                null,
                Token.Name.OPERATOR),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.CONDITION
                },
                null,
                Token.Name.OPERATOR),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET,
                    Token.Name.EXPRESSION,
                    Token.Name.OPENINGBRACKET
                },
                null,
                Token.Name.SUBEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.OPERAND
                },
                null,
                Token.Name.SUBEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SUBEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYOP,
                    Token.Name.SPACE,
                    Token.Name.SUBEXPRESSION
                },
                null,
                Token.Name.SUBEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SUBEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.UNARYOP
                },
                new Token.Name[]
                {
                    Token.Name.SEMICOLON
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SUBEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.UNARYOP
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SUBEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.UNARYOP
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.THEN
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SUBEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SEMICOLON
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SUBEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SUBEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.THEN
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.IDENT
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET
                },
                Token.Name.OPERAND),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.IDENT
                },
                new Token.Name[]
                {
                    Token.Name.SEMICOLON
                },
                Token.Name.OPERAND),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.IDENT
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.BINARYOP
                },
                Token.Name.OPERAND),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.IDENT
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.THEN
                },
                Token.Name.OPERAND),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.CONSTANT
                },
                null,
                Token.Name.OPERAND),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SEMICOLON,
                    Token.Name.CLOSINGBRACKET,
                    Token.Name.VARLIST,
                    Token.Name.OPENINGBRACKET,
                    Token.Name.READEX
                },
                null,
                Token.Name.READ),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SEMICOLON,
                    Token.Name.CLOSINGBRACKET,
                    Token.Name.VARLIST,
                    Token.Name.OPENINGBRACKET,
                    Token.Name.WRITEEX
                },
                null,
                Token.Name.WRITE),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.SEMICOLON,
                    Token.Name.ENDIF,
                    Token.Name.SPACE,
                    Token.Name.ASSIGNMENTLIST,
                    Token.Name.SPACE,
                    Token.Name.ELSE,
                    Token.Name.SPACE,
                    Token.Name.ASSIGNMENTLIST,
                    Token.Name.SPACE,
                    Token.Name.THEN,
                    Token.Name.SPACE,
                    Token.Name.EXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.IF
                },
                null,
                Token.Name.CONDITION)
        };

        static public Token Run(IEnumerable<Token> input)
        {
            var stack = new Stack<Token>();
            var queue = new Queue<Token>(input);

            for (;;)
            {
                var rule = rules
                    .Cast<Rule?>()
                    .Where(rule =>
                        stack
                            .Take(rule.Value.stack.Length)
                            .Select(token => token.Type)
                            .SequenceEqual(rule.Value.stack) &&
                        (rule.Value.input == null || queue
                            .Take(rule.Value.input.Length)
                            .Select(token => token.Type)
                            .SequenceEqual(rule.Value.input)))
                    .FirstOrDefault();

                //shift
                if (rule.Equals(default))
                {
                    if (queue.Count == 0)
                    {
                        return stack.Single();
                    }

                    DebugWriteShift(queue.Peek());
                    DebugWriteState(stack, queue);

                    stack.Push(queue.Dequeue());
                }
                //reduce
                else
                {
                    DebugWriteRule(rule.Value);
                    DebugWriteState(stack, queue);

                    var token = new Token(
                        rule.Value.output,
                        null,
                        new List<Token>(stack.Take(rule.Value.stack.Length)));

                    for (int i = 0; i < rule.Value.stack.Length; ++i)
                    {
                        stack.Pop();
                    }

                    stack.Push(token);
                }
            }
        }

        static private void DebugWriteState(IEnumerable<Token> stack, IEnumerable<Token> queue)
        {
            Console.Write("Stack: ");
            foreach (Token.Name name in stack.Select(token => token.Type))
            {
                Console.Write(name.ToString() + ' ');
            }
            Console.WriteLine();

            Console.Write("Input: ");
            foreach (Token.Name name in queue.Select(token => token.Type))
            {
                Console.Write(name.ToString() + ' ');
            }
            Console.WriteLine('\n');
        }

        static private void DebugWriteRule(Rule rule)
        {
            foreach (Token.Name name in rule.stack)
            {
                Console.Write(name.ToString() + ' ');
            }

            Console.WriteLine("=> " + rule.output.ToString());
        }

        static private void DebugWriteShift(Token token) =>
            Console.WriteLine("Shift << " + token.Type.ToString());
    }
}
