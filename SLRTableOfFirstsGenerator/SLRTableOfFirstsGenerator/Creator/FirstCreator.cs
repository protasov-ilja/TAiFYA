﻿using SLRTableOfFirstsGenerator.Creator;
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

		private Stack<Token> _stackOfVisited = new Stack<Token>();
		private Queue<Token> _tokensQueue = new Queue<Token>();
		private ISet<Token> _setOfVisited = new HashSet<Token>();

		private string _startToken = "";

		private string _TempToken = "";

		private TableOfFirsts _tableOfFirsts = new TableOfFirsts();

		private List<Token> _tempTokensList;

		public FirstCreator(List<RawSentence> sentenses)
		{
			Sentences = SentenceConverter.ConvertRawSentences(sentenses);
			CreateHeaderRowOfTable();
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
				foreach (var token in sentence.Tokens)
				{
					if (!_tableOfFirsts.Column.Contains(token.Value))
					{
						_tableOfFirsts.Column.Add(token.Value);
					}
				}
			}
		}

		private void Create()
		{
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

				CreateFirstRowOfTable(tokens[i]);
			}

			//for (var i = 0; i < Sentences.Count; ++i)
			//{
				

			//	var sentence = Sentences[i];
			//	for (var j = 0; j < sentence.Tokens.Count; ++j)
			//	{
			//		var tokens = sentence.Tokens;
					

					
			//	}

			//	var token = new Token(sentence.Tokens[0], 0, index);
			//	if (!_setOfVisited.Contains(token))
			//	{
			//		_setOfVisited.Add(token);
			//		_tokensQueue.Enqueue(token);
			//	}

			//	_setOfVisited.Add(token);
			//	if (sentence.Tokens[0].StartsWith(START_LINK))
			//	{
			//		_tableOfFirsts.AddInTable(token);
			//		CountingInDepth(Sentences[i].Tokens[0]);
			//	}
			//	else
			//	{
			//		_tableOfFirsts.AddInTable(token);
			//	}

			//	index++;
			//}


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
			if (token.Type == TokenType.NonTerminal)
			{
				_stackOfVisited.Push(token);
				_tableOfFirsts.ExpandTable( new Cell(new Token("[START]", -1, -1)), true);
				_tempTokensList = new List<Token>();
				var tokens = CountingInDepth(token);
				_stackOfVisited.Pop();

				var cells = CreateListOfCells(tokens);
				// add tokens in table
			}
			else
			{
				throw new ArgumentException("first token must be non terminal");
			}
		}

		private List<Cell> CreateListOfCells(IEnumerable<Token> list)
		{
			var distinctTokens = list.Distinct();

			var tokensGoups = distinctTokens.GroupBy(token => token.Value);

			var cells = new List<Cell>();
			foreach (var item in tokensGoups)
			{
				var listTokens = item.ToList();
				cells.Add( new Cell(listTokens) );
			}

			return cells;
		}

		//private bool StackContainToken(string str, int column, int row)
		//{
		//	var otherToken = new Token(str, column, row);
		//	IEnumerable<Token> result = _stackOfEmpties.Where(token => token == otherToken);
		//	return result.Count() != 0;
		//}

		//private bool StackContainValue(string str, int column, int row)
		//{
		//	IEnumerable<Token> result = _stackOfEmpties.Where(token => token.Value == str);
		//	return result.Count() != 0;
		//}

		private List<Token> CountingInDepth(Token token)
		{
			var tokensList = new List<Token>();

			for (var i = 0; i < Sentences.Count; ++i)
			{
				if (Sentences[i].MainToken != token.Value)
				{
					continue;
				}

				if (Sentences[i].Tokens[0].Type == TokenType.NonTerminal)
				{
					tokensList.Add(Sentences[i].Tokens[0]);
					_stackOfVisited.Push(token);
					tokensList.AddRange(CountingInDepth(Sentences[i].Tokens[0]));
					_stackOfVisited.Pop();
				}
				else
				{
					tokensList.Add(Sentences[i].Tokens[0]);
				}
			}

			return tokensList;
		}

		private List<string> ReverseCountInDepth(Token token)
		{
			return null;
		}


		//private List<string> CalculateEmptyCurrent(string token)
		//{
		//	var generatedSet = new List<string>();
		//	for (var i = 0; i < Sentences.Count; ++i) // search token existence
		//	{
		//		if (!_stackOfEmpties.Contains(i) && Sentences[i].Tokens.Contains(token)) // if contains
		//		{
		//			var currentIndex = 0;
		//			while (currentIndex < Sentences[i].Tokens.Count) // search token in tokens
		//			{
		//				if (Sentences[i].Tokens[currentIndex] == token) // token found
		//				{
		//					var seachedIndex = currentIndex + 1;
		//					if (seachedIndex < Sentences[i].Tokens.Count) // token not last
		//					{
		//						if (Sentences[i].Tokens[seachedIndex].StartsWith(START_LINK)) // is link
		//						{
		//							AddInLocalSet(generatedSet, CalculateCurrent(Sentences[i].Tokens[seachedIndex]));
		//						}
		//						else // is determinant
		//						{
		//							AddInLocalSet(generatedSet, Sentences[i].Tokens[seachedIndex]);
		//						}
		//					}
		//					else // token is last
		//					{
		//						_stackOfEmpties.Push(i);
		//						AddInLocalSet(generatedSet, CalculateEmptyCurrent(Sentences[i].MainToken));
		//						_stackOfEmpties.Pop();
		//						if (Sentences[i].MainToken == _startToken)
		//						{
		//							AddInLocalSet(generatedSet, END_TOKEN);
		//						}
		//					}
		//				}

		//				++currentIndex;
		//			}
		//		}
		//	}

		//	return generatedSet;
		//}

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
