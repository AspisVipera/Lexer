using System;
using System.Linq;
using System.Collections.Generic;

namespace Lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.In.ReadToEnd().Replace("\r", "").TrimEnd();
            List<Token> tokens = Lexer.Run(input);
            Token syntaxTree = Parser.Run(tokens);
            DrawTree(syntaxTree);
        }

        static void DrawTree(Token tree, int level = 0)
        {
            Console.WriteLine(new string(' ', level * 4) + tree.Type.ToString() + ' ' + tree.Value);

            foreach (Token child in tree.Children?.Reverse<Token>() ?? Enumerable.Empty<Token>())
            {
                DrawTree(child, level + 1);
            }
        }
    }
}
