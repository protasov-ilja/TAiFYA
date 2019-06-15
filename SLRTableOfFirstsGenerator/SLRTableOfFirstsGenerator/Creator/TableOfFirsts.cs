using System.Collections.Generic;

namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class TableOfFirsts
	{
		public List<string> Column = new List<string>();
		public List<Token> Row = new List<Token>();
		public string StartToken => Column.Count > 0 ? Column[0] : "[END]";

		public List<List<Token>> Table { get; } = new List<List<Token>>();
		private int _currentIndex = 0;

		public void ExpandTable(Token token, bool firstCall = false)
		{
			Row.Add(token);
			var row = new List<Token>(Column.Count);
			for (int i = 0; i < Column.Count; i++) row.Add(null);
			Table.Add(row);
			if (!firstCall)
			{
				_currentIndex++;
			}
		}

		public void AddInTable(Token token)
		{
			var columnIndex = Column.IndexOf(token.Value);
			Table[_currentIndex][columnIndex] = token;
		}
	} 
}
