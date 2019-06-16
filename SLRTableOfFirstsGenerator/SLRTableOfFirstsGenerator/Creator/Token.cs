namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class Token
	{
		const string END_TOKEN = "[END]";
		const string START_TOKEN = "[START]";
		const char START_LINK = '<';
		const char RULE_LINK = '[';

		/// <summary>
		/// token string value
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// token position in sentence 
		/// </summary>
		public int ColIndex { get; set; }

		/// <summary>
		/// token position in row of sentences
		/// </summary>
		public int RowIndex { get; set; }

		/// <summary>
		/// token type : rule [], non-terminal <>, teminal without braces
		/// </summary>
		public TokenType Type { get; set; }

		public Token(string value, int colIndex, int rowIndex)
		{
			if (value.StartsWith(START_LINK))
			{
				Type = TokenType.NonTerminal;
			}
			else if (value.StartsWith(RULE_LINK))
			{
				if (value == END_TOKEN)
				{
					Type = TokenType.End;
				}
				else if (value == START_TOKEN)
				{
					Type = TokenType.Start;
				}
				else
				{
					Type = TokenType.Rule;
				}
			}
			else
			{
				Type = TokenType.Terminal;
			}

			Value = value;
			ColIndex = colIndex;
			RowIndex = rowIndex;
		}
	}
}
