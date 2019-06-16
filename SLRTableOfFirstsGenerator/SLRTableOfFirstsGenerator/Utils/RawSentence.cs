using System.Collections.Generic;

namespace SLRTableOfFirstsGenerator.Utils
{
	public sealed class RawSentence
	{
		public string MainToken { get; set; }
		public List<string> Tokens { get; set; }

		public RawSentence(string main, List<string> tokens)
		{
			Tokens = tokens;
			MainToken = main;
		}

		public RawSentence(string main, string[] tokens)
		{
			Tokens = new List<string>(tokens);
			MainToken = main;
		}
	}
}
