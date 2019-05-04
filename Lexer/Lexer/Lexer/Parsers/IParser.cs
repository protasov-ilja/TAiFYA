using Lexer.Lexer.Enums;

namespace Lexer.Lexer.Parsers
{
    public interface IParser
    {
		TokenType GetTokenType(int startPos, ref int currPos);
		void ResetState();
	}
}
