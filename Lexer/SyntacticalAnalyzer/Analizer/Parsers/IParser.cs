using Lexer.Lexer.Tokens;

using System.Collections.Generic;

namespace SyntacticalAnalyzer.Analizer.Parsers
{
	public interface IParser
	{
		bool Parse(ref int currentPosition, List<Token> tokensList);
	}
}
