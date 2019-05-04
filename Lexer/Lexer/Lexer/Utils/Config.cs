using Lexer.Lexer.Enums;

using System.Collections.Generic;

namespace Lexer.Lexer.Utils
{
	public sealed class Config
	{
		public static readonly Dictionary<char, TokenType> Delimeters = new Dictionary<char, TokenType>()
		{
			{ ' ', TokenType.DELIMETER },
			{ '\n', TokenType.DELIMETER },
			{ '.', TokenType.DOT },
			{ ';', TokenType.SEMICOLON },
			{ ':', TokenType.COLON },
			{ ',', TokenType.COMMA },
			{ '/', TokenType.OPERATOR },
			{ '+', TokenType.OPERATOR },
			{ '-', TokenType.OPERATOR },
			{ '*', TokenType.OPERATOR },
			{ '=', TokenType.EQUALITY_OPERATOR },
			{ '<', TokenType.OPERATOR },
			{ '>', TokenType.OPERATOR },
			{ '%', TokenType.OPERATOR },
			{ '{', TokenType.BRACE_OPEN },
			{ '}', TokenType.BRACE_CLOSE },
			{ '[', TokenType.SQUARE_BRACKET_OPEN },
			{ ']', TokenType.SQUARE_BRACKET_CLOSE },
			{ '(', TokenType.ROUND_BRACKET_OPEN },
			{ ')', TokenType.ROUND_BRACKET_CLOSE },
		};

		public static readonly Dictionary<string, TokenType> Identifiers = new Dictionary<string, TokenType>()
		{
			{ "while" , TokenType.WHILE_LOOP },
			{ "for" , TokenType.FOR_LOOP },
			{ "do" , TokenType.DO_LOOP },
			{ "public" , TokenType.ACCESS_MODIFICATOR },
			{ "private" , TokenType.ACCESS_MODIFICATOR },
			{ "protected" , TokenType.ACCESS_MODIFICATOR },
			{ "var" , TokenType.VAR_IDENTIFIER },
			{ "int" , TokenType.IDENTIFIER },
			{ "string" , TokenType.IDENTIFIER },
			{ "double" , TokenType.IDENTIFIER },
			{ "flaot" , TokenType.IDENTIFIER },
			{ "false" , TokenType.BOOL_OPERATOR },
			{ "true" , TokenType.BOOL_OPERATOR },
			{ "if" , TokenType.CONDITIONS },
			{ "else" , TokenType.CONDITIONS }
		};
	}
}
