using SLRTableOfFirstsGenerator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SLRTableOfFirstsGenerator.Creator
{
	public sealed class FirstCreator
	{
		const string END_TOKEN = "[END]";
		const char START_LINK = '<';

		public List<Sentence> Sentences { get; private set; } = new List<Sentence>();
		private Stack<Token> _stackOfEmpties = new Stack<Token>();

		private Queue<Token> _tokensQueue = new Queue<Token>();
		private ISet<Token> _setOfVisited = new HashSet<Token>();

		private string _startToken = "";

		private string _TempToken = "";

		private TableOfFirsts _tableOfFirsts = new TableOfFirsts();

		public FirstCreator(List<RawSentence> sentenses)
		{
			Sentences = SentenceConverter.ConvertRawSentences(sentenses);
			Create();
		}

		public void WriteResultToStream(TextWriter writer)
		{
			//for (var i = 0; i < )
			//foreach (var s in Sentences)
			//{
			//	writer.WriteLine($"{ s.MainToken } -> { TokensToString(s.Tokens, ' ') } / { TokensToString(s.ForwardSet, ',') }");
			//}
		}

		private string TokensToString(List<string> list, char delimeter)
		{
			var str = "";
			for (var i = 0; i < list.Count; ++i)
			{
				str += list[i];
				if (i < list.Count - 1)
				{
					str += delimeter;
				}
			}

			return str;
		}

		private void CreateHeaderRowOfTable()
		{
			foreach (var sentence in Sentences)
			{
				if (!_tableOfFirsts.Column.Contains(sentence.MainToken))
				{
					_tableOfFirsts.Column.Add(sentence.MainToken);
				}
				
				foreach (var token in sentence.Tokens)
				{
					if (!_tableOfFirsts.Column.Contains(token))
					{
						_tableOfFirsts.Column.Add(token);
					}
				}
			}

			_tableOfFirsts.Column.Add(END_TOKEN);
		}

		private void Create()
		{
			CreateHeaderRowOfTable();
			
			var index = 0;
			var sentence = Sentences[index];
			var tokens = sentence.Tokens;
			if (tokens.Count == 0)
			{
				throw new ArgumentException($"found empty sentence in row: { index }");
			}

			for (var i = 0; i < tokens.Count; ++i)
			{
				if (tokens[i].Type == TokenType.End)
				{
					continue;
				}

				CreateFirstRowOfTable(new Token(tokens[i], i, 0));
			}


			for (var i = 0; i < Sentences.Count; ++i)
			{
				

				var sentence = Sentences[i];
				for (var j = 0; j < sentence.Tokens.Count; ++j)
				{
					var tokens = sentence.Tokens;
					

					
				}

				var token = new Token(sentence.Tokens[0], 0, index);
				if (!_setOfVisited.Contains(token))
				{
					_setOfVisited.Add(token);
					_tokensQueue.Enqueue(token);
				}

				_setOfVisited.Add(token);
				if (sentence.Tokens[0].StartsWith(START_LINK))
				{
					_tableOfFirsts.AddInTable(token);
					CountingInDepth(Sentences[i].Tokens[0]);
				}
				else
				{
					_tableOfFirsts.AddInTable(token);
				}

				index++;
			}


			var indeXXX = 0;






			//for (var i = 0; i < Sentences.Count; ++i)
			//{
			//	_TempToken = Sentences[i].MainToken;

			//	if (Sentences[i].Tokens[0].StartsWith(START_LINK))
			//	{
			//		Sentences[i].AddInSet(CalculateCurrent(Sentences[i].Tokens[0]));
			//	}
			//	else if (Sentences[i].Tokens[0] == EMPTY_LINK)
			//	{
			//		_stackOfEmpties.Push(i);
			//		Sentences[i].AddInSet(CalculateEmptyCurrent(Sentences[i].MainToken));
			//		_stackOfEmpties.Pop();
			//	}
			//	else
			//	{
			//		Sentences[i].AddInSet(Sentences[i].Tokens[0]);
			//	}
			//}
			//if (Sentences.Count != 0)
			//{
			//	Sentences.RemoveAt(Sentences.Count - 1);
			//}
		}

		private void CreateFirstRowOfTable(Token token)
		{
			if (token.Value.StartsWith('<'))
			{
				_tableOfFirsts.ExpandTable();
			}
			else
			{
				throw new ArgumentException("first token must be non terminal");
			}
		}

		private bool StackContainToken(string str, int column, int row)
		{
			var otherToken = new Token(str, column, row);
			IEnumerable<Token> result = _stackOfEmpties.Where(token => token == otherToken);
			return result.Count() != 0;
		}

		private bool StackContainValue(string str, int column, int row)
		{
			IEnumerable<Token> result = _stackOfEmpties.Where(token => token.Value == str);
			return result.Count() != 0;
		}

		private List<string> CountingInDepth(Token token)
		{
			for (var i = 0; i < Sentences.Count; ++i)
			{
				if (Sentences[i].MainToken != token.Value)
				{
					continue;
				}

				if (Sentences[i].Tokens[0].StartsWith(START_LINK))
				{
					_tableOfFirsts.AddInTable(token);
					CountingInDepth(Sentences[i].Tokens[0]);
				}
				else
				{
					_tableOfFirsts.AddInTable(token);
				}
			}

			var generatedSet = new List<string>();
			for (var i = 0; i < Sentences.Count; ++i)
			{
				if (Sentences[i].MainToken == token)
				{
					if (Sentences[i].ForwardSet.Count != 0) // has forward
					{
						AddInLocalSet(generatedSet, Sentences[i].ForwardSet);
					}
					else if (Sentences[i].Tokens[0].StartsWith(START_LINK))
					{
						var set = CalculateCurrent(Sentences[i].Tokens[0]);
						AddInLocalSet(generatedSet, set);
						Sentences[i].AddInSet(set);
					}
					else if (Sentences[i].Tokens[0] == EMPTY_LINK)
					{
						_stackOfEmpties.Push(i);
						var set = CalculateEmptyCurrent(Sentences[i].MainToken);
						_stackOfEmpties.Pop();
						AddInLocalSet(generatedSet, set);
						Sentences[i].AddInSet(set);
					}
					else
					{
						Sentences[i].AddInSet(Sentences[i].Tokens[0]);
						AddInLocalSet(generatedSet, Sentences[i].Tokens[0]);
					}
				}
			}

			return generatedSet;
		}

		private List<string> ReverseCountInDepth(Token token)
		{

		}


		private List<string> CalculateEmptyCurrent(string token)
		{
			var generatedSet = new List<string>();
			for (var i = 0; i < Sentences.Count; ++i) // search token existence
			{
				if (!_stackOfEmpties.Contains(i) && Sentences[i].Tokens.Contains(token)) // if contains
				{
					var currentIndex = 0;
					while (currentIndex < Sentences[i].Tokens.Count) // search token in tokens
					{
						if (Sentences[i].Tokens[currentIndex] == token) // token found
						{
							var seachedIndex = currentIndex + 1;
							if (seachedIndex < Sentences[i].Tokens.Count) // token not last
							{
								if (Sentences[i].Tokens[seachedIndex].StartsWith(START_LINK)) // is link
								{
									AddInLocalSet(generatedSet, CalculateCurrent(Sentences[i].Tokens[seachedIndex]));
								}
								else // is determinant
								{
									AddInLocalSet(generatedSet, Sentences[i].Tokens[seachedIndex]);
								}
							}
							else // token is last
							{
								_stackOfEmpties.Push(i);
								AddInLocalSet(generatedSet, CalculateEmptyCurrent(Sentences[i].MainToken));
								_stackOfEmpties.Pop();
								if (Sentences[i].MainToken == _startToken)
								{
									AddInLocalSet(generatedSet, END_TOKEN);
								}
							}
						}

						++currentIndex;
					}
				}
			}

			return generatedSet;
		}

		//private void AddInLocalSet(List<string> set, List<string> items)
		//{
		//	foreach (var item in items)
		//	{
		//		AddInLocalSet(set, item);
		//	}
		//}

		//private void AddInLocalSet(List<string> set, string item)
		//{
		//	if (!set.Contains(item))
		//	{
		//		set.Add(item);
		//	}
		//}
	}
}
