using System.Collections.Generic;

namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class Sentence
	{
		public string MainToken { get; set; }
		public List<string> Tokens { get; set; }

		public Sentence(string main, List<string> tokens)
		{
			Tokens = tokens;
			MainToken = main;
		}

		public Sentence(string main, string[] tokens)
		{
			Tokens = new List<string>(tokens);
			MainToken = main;
		}
	}
}
