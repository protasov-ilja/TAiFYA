using Lexer.lexer;
using Lexer.lexer.enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lexer
{
	class Program
	{
		public static Dictionary<string, TokenType> tokens = new Dictionary<string, TokenType>()
		{
			{ "(" , TokenType.ROUND_BRACKETS },
			{ ")" , TokenType.ROUND_BRACKETS },
			{ "[" , TokenType.SQUARE_BRACKETS },
			{ "]" , TokenType.SQUARE_BRACKETS },
			{ "while" , TokenType.LOOP },
			{ "for" , TokenType.LOOP },
			{ "do" , TokenType.LOOP },
			{ "public" , TokenType.ACCESS_MODIFICATOR },
			{ "private" , TokenType.ACCESS_MODIFICATOR },
			{ "protected" , TokenType.ACCESS_MODIFICATOR },
			{ "var" , TokenType.IDENTIFIER },
			{ "int" , TokenType.IDENTIFIER },
			{ "false" , TokenType.BOOL_OPERATOR },
			{ "true" , TokenType.BOOL_OPERATOR },
			{ ";", TokenType.SEMICOLON }
		};

        static int Main(string[] args)
        {
			if (args.Length == 0)
			{
				System.Console.WriteLine("Please enter a numeric argument.");
				return 1;
			}
			var lexer = new LexerManager(args[0]);
			try
			{
				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return 0;
        }
    }
}
