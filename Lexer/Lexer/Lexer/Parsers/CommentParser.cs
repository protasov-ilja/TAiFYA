using Lexer.Lexer.Enums;
using Lexer.Lexer.Parsers;
using Lexer.Lexer.Utils;

namespace Lexer.Lexer.Parsers
{
	public sealed class CommentParser : Parser
	{
		public CommentParser(StringsBuffer strBuffer, string fileName)
				  : base(strBuffer, fileName)
		{
		}

		protected override TokenType ChooseToken()
		{
			return TokenType.COMMENT;
		}
	}
}
