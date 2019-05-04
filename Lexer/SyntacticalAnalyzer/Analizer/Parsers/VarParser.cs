using Lexer.Lexer.Enums;

using System.Collections.Generic;

namespace SyntacticalAnalyzer.Analizer.Parsers
{
	public sealed class VarParser : Parser
	{
		private readonly HashSet<TokenType> _validTokens = new HashSet<TokenType>
		{
			
		};

		protected override bool IsValide()
		{
			return IsToken(TokenType.VAR_IDENTIFIER)
				&& IsToken(TokenType.IDENTIFIER)
				&& IsToken(TokenType.COLON)
				&& IsIdList()
				&& IsToken(TokenType.SEMICOLON);
		}
	}
}
