using System;

namespace Lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Run(Lexer.Run(Console.ReadLine()));
        }
    }
}
