namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class Token
	{
		public string Value { get; set; }
		public int ColIndex { get; set; }
		public int RowIndex { get; set; }

		public Token(string value, int colIndex, int rowIndex)
		{
			Value = value;
			ColIndex = colIndex;
			RowIndex = rowIndex;
		}
	}
}
