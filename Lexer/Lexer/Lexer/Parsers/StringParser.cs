using Lexer.Lexer.Enums;
using Lexer.Lexer.Utils;

namespace Lexer.Lexer.Parsers
{
	public sealed class StringParser : Parser
	{
		public StringParser(StringsBuffer strBuffer, string fileName)
			   : base(strBuffer, fileName)
		{
		}

		protected override TokenType ChooseToken()
		{
			return TokenType.STRING;
		}
	}
}
