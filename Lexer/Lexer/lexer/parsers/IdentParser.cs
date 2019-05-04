using Lexer.Lexer.Enums;
using Lexer.Lexer.Utils;

namespace Lexer.Lexer.Parsers
{
	public sealed class IdentParser : Parser
    {
		public IdentParser(StringsBuffer strBuffer, string fileName)
			: base(strBuffer, fileName)
		{
		}

		protected override TokenType ChooseToken()
		{
			return TokenType.VARIABLE;
		}
	}
}
