using System.Collections.Generic;

namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class Sentence
	{
		public string MainToken { get; set; }
		public List<Token> Tokens { get; set; }

		public Sentence(string main, List<Token> tokens)
		{
			Tokens = tokens;
			MainToken = main;
		}

		public Sentence(string main, Token[] tokens)
		{
			Tokens = new List<Token>(tokens);
			MainToken = main;
		}
	}
}
