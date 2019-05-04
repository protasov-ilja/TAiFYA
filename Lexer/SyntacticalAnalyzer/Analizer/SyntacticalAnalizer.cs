using Lexer.Lexer;
using Lexer.Lexer.Tokens;

using SyntacticalAnalyzer.Analizer.Parsers;

using System;
using System.Collections.Generic;

namespace SyntacticalAnalyzer.Analizer
{
	public sealed class SyntacticalAnalizer
	{
		private ILexer _lexer;
		private List<IParser> _parsers = new List<IParser>();
		private List<Token> _tokensList = new List<Token>();
		private int _currIndex;

		public SyntacticalAnalizer(ILexer lexer)
		{
			_lexer = lexer;
			ReadFromLexer();
			InitilizeParsers();
		}

		private void ReadFromLexer()
		{
			var token = _lexer.GetToken();
			while (token != null)
			{
				_tokensList.Add(token);
				token = _lexer.GetToken();
			}
		}

		private void InitilizeParsers()
		{
			_parsers.Add(new VarParser());
		}

		public void Run()
		{
			while (_currIndex < _tokensList.Count)
			{
				bool isRecognized = false;
				foreach (var parser in _parsers)
				{
					isRecognized = parser.Parse(ref _currIndex, _tokensList);
					if (isRecognized)
					{
						break;
					}
				}

				if (!isRecognized)
				{
					_currIndex++;
				}

				Console.WriteLine(isRecognized);
			}
		}
	}
}
