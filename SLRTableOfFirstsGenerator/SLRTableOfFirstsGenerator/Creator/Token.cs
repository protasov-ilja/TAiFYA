using System;

namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class Token : IEquatable<Token>
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

		public Token(string value)
		{
			Value = value;
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
		}

		public Token(string value, int colIndex, int rowIndex)
			: this(value)
		{
			ColIndex = colIndex;
			RowIndex = rowIndex;
		}

		public bool Equals(Token other)
		{
			//Check whether the compared object is null. 
			if (ReferenceEquals(other, null)) return false;

			//Check whether the compared object references the same data. 
			if (ReferenceEquals(this, other)) return true;

			//Check whether the products' properties are equal. 
			return Value.Equals(other.Value) && ColIndex.Equals(other.ColIndex) && RowIndex.Equals(other.RowIndex) && Type.Equals(other.Type);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, ColIndex, RowIndex, Type);
		}
	}
}
