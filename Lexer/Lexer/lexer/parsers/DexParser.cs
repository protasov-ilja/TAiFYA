using Lexer.Lexer.Enums;
using Lexer.Lexer.Utils;

namespace Lexer.Lexer.Parsers
{
	public sealed class DexParser : Parser
    {
		public DexParser(StringsBuffer strBuffer, string fileName)
			: base(strBuffer, fileName)
		{
		}

		protected override TokenType ChooseToken()
		{
			switch (_currState)
			{
				case 1:
					return TokenType.INT_NUMBER;
				case 4:
					return TokenType.FLOAT_NUMBER;
				default:
					return TokenType.E_NUMBER;
			}
		}
	}
}
