﻿using System.Collections.Generic;

namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class Cell
	{
		private readonly List<Token> _values = new List<Token>();

		public Token this[int index]
		{
			get => _values[index];
			set => _values[index] = value;
		}

		public string Value => _values.Count != 0 ? _values[0].Value : ""; 

		public Cell(Token token)
		{
			_values.Add(token);
		}

		public Cell(List<Token> token)
		{

		}
	}
}