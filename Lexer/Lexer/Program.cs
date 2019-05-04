using Lexer.Lexer;
using System;

namespace Lexer
{
	class Program
	{
        static int Main(string[] args)
        {
			if (args.Length == 0)
			{
				Console.WriteLine("Please enter a numeric argument.");
				return 1;
			}

			var lexer = new LexerManager(args[0]);

			return 0;
        }
    }
}
