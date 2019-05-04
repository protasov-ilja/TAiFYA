using Lexer.Lexer.Enums;
using Lexer.Lexer.Parsers;
using Lexer.Lexer.Tokens;
using Lexer.Lexer.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lexer.Lexer
{
	public sealed class LexerManager : ILexer
    {
		private StringsBuffer _stringBuffer;
		private List<IParser> _stateMachines = new List<IParser>();
		private int _currPos;
		private int _currRow;
		private Queue<Token> _tokens = new Queue<Token>();

		public LexerManager(string fileName)
		{
			using (StreamReader streamReader = new StreamReader(fileName, Encoding.Default))
			{
				_stringBuffer = new StringsBuffer(streamReader);
				InitilizeStateMachines();
				_currPos = 0;
				_currRow = 0;
				while (!_stringBuffer.IsFileEnded)
				{
					var token = GetNextToken();
					if (token.Type != TokenType.DELIMETER)
					{
						Console.WriteLine(token.ToString());
						_tokens.Enqueue(token);
					}

					_currPos++;

					if (_currPos >= _stringBuffer.CurrStringCount)
					{
						_currRow++;
						_stringBuffer.GetNewString();
						_currPos = 0;
					}
				}
			}
		}

		private void InitilizeStateMachines()
		{
			_stateMachines.Add(new IdentParser(_stringBuffer, "var.txt"));
			_stateMachines.Add(new HexParser(_stringBuffer, "hex.txt"));
			_stateMachines.Add(new BinParser(_stringBuffer, "bin.txt"));
			_stateMachines.Add(new OctParser(_stringBuffer, "oct.txt"));
			_stateMachines.Add(new DexParser(_stringBuffer, "dex.txt"));
		}

		private Token GetNextToken()
		{
			if (!_stringBuffer.IsFileEnded)
			{
				var startPos = _currPos;
				var newPos = _currPos;
				var tokenType = TokenType.UNCKNOWN;
				foreach (var stateMachine in _stateMachines)
				{
					tokenType = stateMachine.GetTokenType(startPos, ref newPos);
					stateMachine.ResetState();

					if (tokenType != TokenType.UNCKNOWN)
					{
						break;	
					}
				}

				var strToken = GetStringToken(startPos, newPos);
				if (tokenType == TokenType.UNCKNOWN)
				{
					_currPos = newPos;
					if (Config.Delimeters.ContainsKey(_stringBuffer.CurrString[startPos]))
					{
						tokenType = Config.Delimeters[_stringBuffer.CurrString[startPos]];
					}
				}
				else
				{
					_currPos = newPos - 1;
					
					if (tokenType == TokenType.VARIABLE)
					{
						if (Config.Identifiers.ContainsKey(strToken))
						{
							tokenType = Config.Identifiers[strToken];
						}
					}
				}
				
				return new Token(strToken, tokenType, startPos, _currRow);
			}

			return new Token("END", TokenType.END, 0, _currRow);
		}

		private string GetStringToken(int startPos, int endPos)
		{
			var strLength = endPos - startPos;
			if (strLength == 0)
			{
				return _stringBuffer.CurrString[startPos].ToString();
			}

			return _stringBuffer.CurrString.Substring(startPos, strLength);
		}

		public bool IsVarDelimeter(char simbol)
		{
			return Config.Delimeters.ContainsKey(simbol);
		}

		public Token GetToken()
		{
			if (_tokens.Count != 0)
			{
				return _tokens.Dequeue();
			}

			return null;
		}
	}
}
