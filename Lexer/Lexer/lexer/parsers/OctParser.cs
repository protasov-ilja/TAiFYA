using Lexer.lexer.enums;
using Lexer.lexer.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer.lexer.parsers
{
    class OctParser : Parser
    {
		private List<char> m_delimeters = new List<char>
		{
			' ',
			'=',
			'(',
			')',
			';'
		};

		public OctParser(StringsBuffer strBuffer, string fileName)
			: base(strBuffer, fileName)
		{
			Console.WriteLine("HexParser");
		}

		protected override StateMachineStatus CheckState(char simbol)
		{
			switch (currState)
			{
				case 0:
					{
						var isHex = simbol == '0';
						if (!isHex)
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
						var isHex = simbol == 'h';
						if (!isHex)
						{
							return StateMachineStatus.ERROR;
						}
						else
						{
							currState = 2;
						}

						break;
					}
				case 2:
					{

						var isHex = Util.IsHexNumber(simbol);
						if (!isHex)
						{
							return StateMachineStatus.ERROR;
						}
						else
						{
							currState = 3;
						}

						break;
					}
				case 3:
					{
						var isHex = Util.IsHexNumber(simbol);
						if (!isHex)
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
			return TokenType.HEX_NUMBER;
		}

		protected override bool IsTerminalState()
		{
			return terminateStates.Contains(currState);
		}
	}
}
