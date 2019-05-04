using Lexer.Lexer.Enums;
using Lexer.Lexer.Utils;

namespace Lexer.Lexer.Parsers
{
    public sealed class BinParser : Parser
    {
		public BinParser(StringsBuffer strBuffer, string fileName)
			: base(strBuffer, fileName)
		{
		}

		protected override TokenType ChooseToken()
		{
			return TokenType.BIN_NUMBER;
		}
	}
}
