using System;
using System.Linq;

namespace Lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawTree(Parser.Run(Lexer.Run(Console.ReadLine())));

        }

        static void DrawTree(Token tree, int level = 0)
        {
            Console.WriteLine(new String(' ', level * 4) + tree.Type.ToString() + ' ' + tree.Value);

            foreach (Token child in tree.Childs != null ? tree.Childs.Reverse<Token>() : Enumerable.Empty<Token>())
            {
                DrawTree(child, level + 1);
            }
        }
    }
}
