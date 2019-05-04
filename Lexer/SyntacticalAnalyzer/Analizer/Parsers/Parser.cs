using System.Collections.Generic;
using Lexer.Lexer.Enums;
using Lexer.Lexer.Tokens;

namespace SyntacticalAnalyzer.Analizer.Parsers
{
	public abstract class Parser : IParser
	{
		protected List<Token> _tokensList = new List<Token>();
		protected int _currIndex;

		public bool Parse(ref int currentPosition, List<Token> tokensList)
		{
			_tokensList = tokensList;
			_currIndex = currentPosition;

			if (IsValide())
			{
				currentPosition = _currIndex;
				return true;
			}

			return false;
		}

		protected abstract bool IsValide();

		protected bool IsTokenInSet(HashSet<TokenType> set)
		{
			if (_currIndex >= _tokensList.Count)
			{
				return false;
			}

			var token = _tokensList[_currIndex];

			if (set.Contains(token.Type))
			{
				_currIndex++;
				return true;
			}

			return false;
		}

		protected bool IsToken(TokenType type)
		{
			if (_currIndex >= _tokensList.Count)
			{
				return false;
			}

			var token = _tokensList[_currIndex];
			if (token.Type == type)
			{
				_currIndex++;

				return true;
			}

			return false;
		}

		protected bool IsIdList()
		{
			return IsToken(TokenType.VARIABLE) && IsIdListRightPart();
		}

		private bool IsIdListRightPart()
		{
			if (IsToken(TokenType.COMMA))
			{
				if (IsToken(TokenType.VARIABLE))
				{
					return IsIdListRightPart();
				}
			}
			else
			{
				return true;
			}

			return false;
		}
	}
}
