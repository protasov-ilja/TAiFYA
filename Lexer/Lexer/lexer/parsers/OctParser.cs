using Lexer.Lexer.Enums;
using Lexer.Lexer.Utils;

namespace Lexer.Lexer.Parsers
{
	public sealed class OctParser : Parser
    {
		public OctParser(StringsBuffer strBuffer, string fileName)
			: base(strBuffer, fileName)
		{
		}

		protected override TokenType ChooseToken()
		{
			return TokenType.OCT_NUMBER;
		}
	}
}
