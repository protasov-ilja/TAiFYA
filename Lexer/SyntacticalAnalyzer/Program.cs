using Lexer.Lexer;
using System;

namespace SyntacticalAnalyzer
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

			ILexer lexer = new LexerManager(args[0]);
			var s = new Analizer.SyntacticalAnalizer(lexer);
			s.Run();

			return 0;
		}
	}
}
