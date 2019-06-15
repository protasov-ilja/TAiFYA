using SLRTableOfFirstsGenerator.Creator;
using SLRTableOfFirstsGenerator.Utils;
using System;
using System.IO;
using System.Text;

namespace SLRTableOfFirstsGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			using (StreamReader streamReader = new StreamReader(args[0], Encoding.Default))
			{
				var reader = new SentencesReader(streamReader);
				var creator = new FirstCreator(reader.Sentences);
				creator.WriteResultToStream(Console.Out);
			}
		}
	}
}
