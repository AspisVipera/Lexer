using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Lexer
{
    static class Parser
    {
        private readonly struct Rule
        {
            public Rule(Token.Name[] stack, Token.Name[] input, Token.Name output)
            {
                this.Stack  = stack;
                this.Input  = input;
                this.Output = output;
            }

            public Token.Name[] Stack  { get; }
            public Token.Name[] Input  { get; }
            public Token.Name Output { get; }
        } 

        private static readonly Rule[] rules =
        {
            new Rule(
                new Token.Name[]
                { 
                    Token.Name.CALCDEFINITION,
                    Token.Name.SPACE,
                    Token.Name.VARDEFINITION,
                    Token.Name.I
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
                Token.Name.ATOMEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.OPERAND
                },
                null,
                Token.Name.ATOMEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ATOMEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYAND,
                    Token.Name.SPACE,
                    Token.Name.ANDEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.BINARYOR
                },
                Token.Name.ANDEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ANDEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYOR,
                    Token.Name.SPACE,
                    Token.Name.OREXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.BINARYEQU
                },
                Token.Name.OREXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.OREXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYEQU,
                    Token.Name.SPACE,
                    Token.Name.EQUEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.BINARYEQU
                },
                Token.Name.EQUEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ATOMEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYAND,
                    Token.Name.SPACE,
                    Token.Name.ANDEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SEMICOLON
                },
                Token.Name.ANDEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ANDEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYOR,
                    Token.Name.SPACE,
                    Token.Name.OREXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SEMICOLON
                },
                Token.Name.OREXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.OREXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYEQU,
                    Token.Name.SPACE,
                    Token.Name.EQUEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SEMICOLON
                },
                Token.Name.EQUEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ATOMEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYAND,
                    Token.Name.SPACE,
                    Token.Name.ANDEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET
                },
                Token.Name.ANDEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ANDEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYOR,
                    Token.Name.SPACE,
                    Token.Name.OREXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET
                },
                Token.Name.OREXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.OREXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYEQU,
                    Token.Name.SPACE,
                    Token.Name.EQUEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET
                },
                Token.Name.EQUEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ATOMEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYAND,
                    Token.Name.SPACE,
                    Token.Name.ANDEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.THEN
                },
                Token.Name.ANDEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.ANDEXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYOR,
                    Token.Name.SPACE,
                    Token.Name.OREXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.THEN
                },
                Token.Name.OREXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.OREXPRESSION,
                    Token.Name.SPACE,
                    Token.Name.BINARYEQU,
                    Token.Name.SPACE,
                    Token.Name.EQUEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SPACE,
                    Token.Name.THEN
                },
                Token.Name.EQUEXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.EQUEXPRESSION,
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
                    Token.Name.EQUEXPRESSION,
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
                    Token.Name.EQUEXPRESSION,
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
                    Token.Name.EQUEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.SEMICOLON
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.EQUEXPRESSION
                },
                new Token.Name[]
                {
                    Token.Name.CLOSINGBRACKET
                },
                Token.Name.EXPRESSION),

            new Rule(
                new Token.Name[]
                {
                    Token.Name.EQUEXPRESSION
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
                    Token.Name.BINARYEQU
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
                    Token.Name.BINARYOR
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
                    Token.Name.BINARYAND
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

        private static readonly Token.Name[][] ShiftRules =
        {
            new Token.Name[]
            {
                Token.Name.I,
                Token.Name.VAR
            },

            new Token.Name[]
            {
                Token.Name.LOGICAL,
                Token.Name.SPACE,
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.BEGIN
            },

            new Token.Name[]
            {
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.BEGIN
            },

            new Token.Name[]
            { 
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.READEX
            },

            new Token.Name[]
            {
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.WRITEEX
            },

            new Token.Name[]
            {
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.IF
            },

            new Token.Name[]
            {
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.ELSE
            },

            new Token.Name[]
            {
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.END
            },

            new Token.Name[]
            {
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.ENDIF
            },

            new Token.Name[]
            {
                Token.Name.SEMICOLON,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.COLON,
                Token.Name.SPACE,
                Token.Name.LOGICAL
            },

            new Token.Name[]
            {
                Token.Name.COMMA,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.ASSIGNMENTCHAR,
                Token.Name.SPACE,
                Token.Name.UNARYOP
            },

            new Token.Name[]
            {
                Token.Name.ASSIGNMENTCHAR,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.ASSIGNMENTCHAR,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.ASSIGNMENTCHAR,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.OPENINGBRACKET,
                Token.Name.UNARYOP
            },

            new Token.Name[]
            {
                Token.Name.OPENINGBRACKET,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.OPENINGBRACKET,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.OPENINGBRACKET,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.CLOSINGBRACKET,
                Token.Name.SEMICOLON
            },

            new Token.Name[]
            {
                Token.Name.CLOSINGBRACKET,
                Token.Name.CLOSINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.CLOSINGBRACKET,
                Token.Name.SPACE,
                Token.Name.BINARYEQU
            },

            new Token.Name[]
            {
                Token.Name.CLOSINGBRACKET,
                Token.Name.SPACE,
                Token.Name.BINARYOR
            },

            new Token.Name[]
            {
                Token.Name.CLOSINGBRACKET,
                Token.Name.SPACE,
                Token.Name.BINARYAND
            },

            new Token.Name[]
            {
                Token.Name.CLOSINGBRACKET,
                Token.Name.SPACE,
                Token.Name.THEN
            },

            new Token.Name[]
            {
                Token.Name.BEGIN,
                Token.Name.SPACE,
                Token.Name.READEX
            },

            new Token.Name[]
            {
                Token.Name.BEGIN,
                Token.Name.SPACE,
                Token.Name.WRITEEX
            },

            new Token.Name[]
            {
                Token.Name.BEGIN,
                Token.Name.SPACE,
                Token.Name.IF
            },

            new Token.Name[]
            {
                Token.Name.BEGIN,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.VAR,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.END
            },

            new Token.Name[]
            {
                Token.Name.UNARYOP,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.UNARYOP,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.UNARYOP,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.BINARYEQU,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.BINARYEQU,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.BINARYEQU,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.BINARYEQU,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.BINARYOR,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.BINARYOR,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.BINARYOR,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.BINARYOR,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.BINARYAND,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.BINARYAND,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.BINARYAND,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.BINARYAND,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.READEX,
                Token.Name.OPENINGBRACKET,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.WRITEEX,
                Token.Name.OPENINGBRACKET,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.IF,
                Token.Name.SPACE,
                Token.Name.UNARYOP
            },

            new Token.Name[]
            {
                Token.Name.IF,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.IF,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.IF,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.THEN,
                Token.Name.SPACE,
                Token.Name.IF
            },

            new Token.Name[]
            {
                Token.Name.THEN,
                Token.Name.SPACE,
                Token.Name.UNARYOP
            },

            new Token.Name[]
            {
                Token.Name.THEN,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.THEN,
                Token.Name.SPACE,
                Token.Name.WRITEEX
            },

            new Token.Name[]
            {
                Token.Name.THEN,
                Token.Name.SPACE,
                Token.Name.READEX
            },

            new Token.Name[]
            {
                Token.Name.THEN,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.THEN,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.ELSE,
                Token.Name.SPACE,
                Token.Name.UNARYOP
            },

            new Token.Name[]
            {
                Token.Name.ELSE,
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.ELSE,
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.ELSE,
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.ELSE,
                Token.Name.SPACE,
                Token.Name.IF
            },

            new Token.Name[]
            {
                Token.Name.ELSE,
                Token.Name.SPACE,
                Token.Name.WRITEEX
            },

            new Token.Name[]
            {
                Token.Name.ELSE,
                Token.Name.SPACE,
                Token.Name.READEX
            },

            new Token.Name[]
            {
                Token.Name.ENDIF,
                Token.Name.SEMICOLON
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.COMMA
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.SPACE,
                Token.Name.ASSIGNMENTCHAR
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.SPACE,
                Token.Name.BINARYEQU
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.SPACE,
                Token.Name.BINARYOR
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.SPACE,
                Token.Name.BINARYAND
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.SPACE,
                Token.Name.COLON
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.SEMICOLON
            },

            new Token.Name[]
            {
                Token.Name.IDENT,
                Token.Name.CLOSINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.CONSTANT,
                Token.Name.SPACE,
                Token.Name.BINARYEQU
            },

            new Token.Name[]
            {
                Token.Name.CONSTANT,
                Token.Name.SPACE,
                Token.Name.BINARYOR
            },

            new Token.Name[]
            {
                Token.Name.CONSTANT,
                Token.Name.SPACE,
                Token.Name.BINARYAND
            },

            new Token.Name[]
            {
                Token.Name.CONSTANT,
                Token.Name.SEMICOLON
            },

            new Token.Name[]
            {
                Token.Name.CONSTANT,
                Token.Name.CLOSINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.BEGIN
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.READEX
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.WRITEEX
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.IF
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.IDENT
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.END
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.SEMICOLON
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.LOGICAL
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.COLON
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.UNARYOP
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.OPENINGBRACKET
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.CONSTANT
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.BINARYEQU
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.BINARYOR
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.BINARYAND
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.THEN
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.ELSE
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.ENDIF
            },

            new Token.Name[]
            {
                Token.Name.SPACE,
                Token.Name.ASSIGNMENTCHAR
            }
        };

        public static Token Run(IEnumerable<Token> input)
        {
            var stack = new Stack<Token>();
            var queue = new Queue<Token>(input);

            for (;;)
            {
                var rule = rules
                    .Where(rule =>
                        stack
                            .Take(rule.Stack.Length)
                            .Select(token => token.Type)
                            .SequenceEqual(rule.Stack, comparePriority) &&
                        (rule.Input == null || queue
                            .Take(rule.Input.Length)
                            .Select(token => token.Type)
                            .SequenceEqual(rule.Input)))
                    .Cast<Rule?>()
                    .FirstOrDefault();

                //shift
                if (rule == null)
                {
                    if (queue.Count == 0)
                    {
                        return stack.Single();
                    }

                    var shiftrule = ShiftRules
                        .Where(rule =>
                            queue
                                .Take(rule.Length)
                                .Select(token => token.Type)
                                .SequenceEqual(rule))
                        .FirstOrDefault();

                    if (shiftrule != null)
                    {
                        DebugWriteShift(queue.Peek());
                        DebugWriteState(stack, queue);
                        stack.Push(queue.Dequeue());
                    }
                    else {
                        DebugWriteState(stack, queue);

                        string stackstr = stack.Any() ? stack
                            .Select(token => token.Value)
                            .Reverse()
                            .Aggregate((p, c) => p + c) : "";

                        string skippedstr = string.Concat(stackstr
                            .Reverse()
                            .SkipWhile(c => c != '\n')
                            .Reverse());

                        string lastLine = skippedstr.Any() ? skippedstr : stackstr;

                        int max = ShiftRules.Select(rulearr =>
                        {
                            IEnumerable<Token.Name> ruleenum = rulearr;
                            IEnumerable<Token> queueenum = queue;

                            int count = 0;

                            while (queueenum.Any() && queueenum.Any() && ruleenum.First() == queueenum.First().Type)
                            {
                                ++count;
                                ruleenum = ruleenum.Skip(1);
                                queueenum = queueenum.Skip(1);
                            }
                            

                            return count;

                        }).Max();


                        string correctqueue = queue.Any() ? string.Concat(queue.Take(max).Select(token => token.Value)) : "";
                        string firstincorrect = queue.Skip(max).Any() ? queue.Skip(max).First().Value : "";

                        throw new NoRuleException(
                            lastLine + correctqueue + firstincorrect + " <\n" +
                            "Символ до стрелки не ожидался в этой позиции");
                    }
                }
                //reduce
                else
                {
                    DebugWriteRule(rule.Value);
                    DebugWriteState(stack, queue);

                    var token = new Token(
                        rule.Value.Output,
                        stack.
                            Take(rule.Value.Stack.Length)
                            .Select(token => token.Value)
                            .Reverse()
                            .Aggregate((accumulator, next) => accumulator + next),
                        new List<Token>(stack.Take(rule.Value.Stack.Length)));

                    for (int i = 0; i < rule.Value.Stack.Length; ++i)
                    {
                        stack.Pop();
                    }

                    stack.Push(token);
                }
            }
        }

        private static readonly ComparePriority comparePriority = new();
            
        private static void DebugWriteState(IEnumerable<Token> stack, IEnumerable<Token> queue)
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

        private static void DebugWriteRule(Rule rule)
        {
            foreach (Token.Name name in rule.Stack)
            {
                Console.Write(name.ToString() + ' ');
            }

            Console.WriteLine("=> " + rule.Output.ToString());
        }

        private static void DebugWriteShift(Token token) =>
            Console.WriteLine("Shift << " + token.Type.ToString());
    }
}
