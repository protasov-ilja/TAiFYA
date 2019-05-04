using Lexer.Lexer.Enums;
using Lexer.Lexer.Utils;

namespace Lexer.Lexer.Parsers
{
	public sealed class HexParser : Parser
	{
		public HexParser(StringsBuffer strBuffer, string fileName)
			: base(strBuffer, fileName)
		{
		}

		protected override TokenType ChooseToken()
		{
			return TokenType.HEX_NUMBER;
		}
	}
}
