using GuideSetsDeterminant;
using GuideSetsDeterminant.Creator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CreatorTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void CurrentIsDeterminate()
		{
			var R1 = new List<string> { "nu", "<R2>" };
			var A = new List<string> { "e" };
			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<A>", A)
			};

			GuideSetCreator creator = new GuideSetCreator(list);

			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R1[0]);
		}

		[TestMethod]
		public void NextIsDeterminate()
		{
			var R1 = new List<string> { "<R2>", "<A>" };
			var R2 = new List<string> { "nu" };
			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R2)
			};

			GuideSetCreator creator = new GuideSetCreator(list);

			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R2[0]);
			Assert.AreEqual(creator._sentences[1].ForwardSet[0], R2[0]);
		}

		[TestMethod]
		public void NextHasTwoVariantsAndAreDeterminate()
		{
			var R1 = new List<string> { "<R2>", "<A>" };
			var R21 = new List<string> { "nu" };
			var R22 = new List<string> { "ny" };

			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R21),
				new Sentence("<R2>", R22)
			};

			GuideSetCreator creator = new GuideSetCreator(list);
			//Console.WriteLine(creator.Sentences[1].ForwardSet[0]);
			//Console.WriteLine(creator.Sentences[1].ForwardSet[1]);
			Assert.AreEqual(creator._sentences[2].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[1].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[0].ForwardSet.Count, 2);
			Assert.AreEqual(creator._sentences[1].ForwardSet[0], R21[0]);
			Assert.AreEqual(creator._sentences[2].ForwardSet[0], R22[0]);
			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R21[0]);
			Assert.AreEqual(creator._sentences[0].ForwardSet[1], R22[0]);
		}

		[TestMethod]
		public void NextIsEmptyAndNextNextIsDeterminate()
		{
			var R1 = new List<string> { "e" };
			var R21 = new List<string> { "ха", "<R1>", "nu" };
			var R22 = new List<string> { "ny" };

			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R21),
				new Sentence("<R2>", R22)
			};

			GuideSetCreator creator = new GuideSetCreator(list);
			Assert.AreEqual(creator._sentences[0].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R21[2]);
		}

		[TestMethod]
		public void NextIsEmptyAndNextNextNextIsDeterminate()
		{
			var R1 = new List<string> { "e" };
			var R2 = new List<string> { "ха", "<R1>", "<R3>" };
			var R3 = new List<string> { "nu" };

			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R2),
				new Sentence("<R3>", R3)
			};

			GuideSetCreator creator = new GuideSetCreator(list);
			Assert.AreEqual(creator._sentences[0].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R3[0]);
		}

		[TestMethod]
		public void NextIsEmptyAndNextNextNextNextIsDeterminate()
		{
			var R1 = new List<string> { "e" };
			var R2 = new List<string> { "ха", "<R1>", "<R3>" };
			var R3 = new List<string> { "<R4>" };
			var R4 = new List<string> { "nu" };

			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R2),
				new Sentence("<R3>", R3),
				new Sentence("<R4>", R4)
			};

			GuideSetCreator creator = new GuideSetCreator(list);
			Assert.AreEqual(creator._sentences[0].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R4[0]);
		}

		[TestMethod]
		public void FinalTest()
		{
			var R1 = new List<string> { "e" };
			var R2 = new List<string> { "<R4>", "<R1>" };
			var R3 = new List<string> { "<R2>", "nu" };
			var R4 = new List<string> { "ne" };

			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R2),
				new Sentence("<R3>", R3),
				new Sentence("<R4>", R4)
			};

			GuideSetCreator creator = new GuideSetCreator(list);
			Assert.AreEqual(creator._sentences[0].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R3[1]);
		}

		[TestMethod]
		public void FinalTest2()
		{
			var R1 = new List<string> { "e" };
			var R2 = new List<string> { "<R4>", "<R1>", "<R3>" };
			var R3 = new List<string> { "e" };
			var R4 = new List<string> { "<R3>", "ne" };

			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R2),
				new Sentence("<R3>", R3),
				new Sentence("<R4>", R4)
			};

			GuideSetCreator creator = new GuideSetCreator(list);
			Assert.AreEqual(creator._sentences[0].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R4[1]);
		}


		[TestMethod]
		public void FinalTest3()
		{
			//var R1 = new List<string> { "<R2>", "<A>" };
			//var A1 = new List<string> { "e" };
			//var A2 = new List<string> { "ay", "<R3>", "<A>" };
			//var R2 = new List<string> { "<R3>", "<B>"/*, "<R5>"*/ };
			//var B1 = new List<string> { "ky", "<R3>", "<B>" };
			//var B2 = new List<string> { "e" };
			//var R31 = new List<string> { "yxti" };
			//var R32 = new List<string> { "ho", "<R3>" };
			//var R33 = new List<string> { "hy", "<R4>", "iny" };
			//var R41 = new List<string> { "e" };
			//var R42 = new List<string> { "oi", "<R1>", "kakoj" };
			////var R5 = new List<string> { "e" };

			//var list = new List<Sentence>
			//{
			//	new Sentence("<R1>", R1),
			//	new Sentence("<A>", A1),
			//	new Sentence("<A>", A2),
			//	new Sentence("<R2>", R2),
			//	new Sentence("<B>", B1),
			//	new Sentence("<B>", B2),
			//	new Sentence("<R3>", R31),
			//	new Sentence("<R3>", R32),
			//	new Sentence("<R3>", R33),
			//	new Sentence("<R4>", R41),
			//	new Sentence("<R4>", R42),
			//	//new Sentence("<R5>", R5),
			//};


			var R1 = new List<string> { "e" };
			var R2 = new List<string> { "<R4>", "<R1>", "<R3>" };
			var R3 = new List<string> { "e" };
			var R4 = new List<string> { "<R3>", "<R4>" };
			var R4 = new List<string> { "<R3>", "ne" };

			var list = new List<Sentence>
			{
				new Sentence("<R1>", R1),
				new Sentence("<R2>", R2),
				new Sentence("<R3>", R3),
				new Sentence("<R4>", R4)
			};

			GuideSetCreator creator = new GuideSetCreator(list);
			Assert.AreEqual(creator._sentences[0].ForwardSet.Count, 1);
			Assert.AreEqual(creator._sentences[0].ForwardSet[0], R4[1]);
		}
	}
}
