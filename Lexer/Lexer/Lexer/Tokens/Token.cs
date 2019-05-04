using Lexer.Lexer.Enums;

namespace Lexer.Lexer.Tokens
{
	public sealed class Token
    {
		public string Str { get; set; }
		public TokenType Type { get; set; }
		public int RowPosition { get; set; }
		public int RowNumber { get; set; }

		public Token(string str, TokenType type, int rowPosition, int rowNumber)
		{
			Str = str;
			Type = type;
			RowPosition = rowPosition;
			RowNumber = rowNumber;
		}

		public override string ToString()
		{
			return $"str: { Str } type: { Type } pos: { RowPosition } row: { RowNumber }";
		}
    }
}
