using Lexer.lexer.enums;
using Lexer.lexer.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer.lexer.parsers
{
    public class IdentParser : Parser
    {
		private List<char> m_delimeters = new List<char>
		{
			' ',
			'.',
			'=',
			'(',
			')',
			';'
		};

		public IdentParser(StringsBuffer strBuffer, string fileName)
			: base(strBuffer, fileName)
		{
			Console.WriteLine("IdentParser");
		}

		protected override StateMachineStatus CheckState(char simbol)
		{
			switch (currState)
			{
				case 0:
					{
						var isVar = Util.IsSimbol(simbol);
						if (!isVar)
						{
							return StateMachineStatus.ERROR;
						}
						else
						{
							currState = 1;
						}
						break;
					}
				case 1:
					{
						var isVar = Util.IsSimbol(simbol) || Util.IsDexNumber(simbol);
						if (!isVar)
						{
							var isDelimeter = m_delimeters.Contains(simbol);
							if (isDelimeter)
							{
								return StateMachineStatus.END;
							}
							else
							{
								return StateMachineStatus.ERROR;
							}
						}

						break;
					}
			}

			return StateMachineStatus.CONTINUE;
		}

		protected override TokenType ChooseToken()
		{
			return TokenType.IDENTIFIER;
		}

		protected override bool IsTerminalState()
		{
			return terminateStates.Contains(currState);
		}
	}
}
