using Lexer.Lexer;
using Lexer.Lexer.Enums;
using Lexer.Lexer.Tokens;

using System.Collections.Generic;

namespace SyntacticalAnalyzer.Analizer.Parsers
{
	public sealed class VarParser : Parser
	{
		private HashSet<TokenType> _validTokens = new HashSet<TokenType>
		{
			TokenType.INT_NUMBER, TokenType.OCT_NUMBER, TokenType.HEX_NUMBER, TokenType.FLOAT_NUMBER, TokenType.E_NUMBER, TokenType.BIN_NUMBER
		};

		protected override bool IsValide()
		{
			return IsToken(TokenType.VAR_IDENTIFIER)
				&& IsIdList()
				&& IsToken(TokenType.EQUALITY_OPERATOR)
				&& IsTokenInSet(_validTokens)
				&& IsToken(TokenType.SEMICOLON);
		}
	}
}
