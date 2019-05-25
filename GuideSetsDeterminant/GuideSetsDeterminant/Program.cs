using GuideSetsDeterminant.Creator;
using GuideSetsDeterminant.Utils;
using System;
using System.IO;
using System.Text;

namespace GuideSetsDeterminant
{
	class Program
	{
		static void Main(string[] args)
		{
			using (StreamReader streamReader = new StreamReader(args[0], Encoding.Default))
			{
				var reader = new SentencesReader(streamReader);
				GuideSetCreator creator = new GuideSetCreator(reader.Sentences);
				creator.WriteResultToStream(Console.Out);
			}
		}
	}
}
