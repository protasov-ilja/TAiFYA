using Lexer.Lexer.Enums;
using Lexer.Lexer.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lexer.Lexer.Parsers
{
    public abstract class Parser : IParser
    {
		public const int ErrorState = -1;
		protected List<List<int>> _stateMachineTable;
		protected StringsBuffer _strBuffer;
		protected int _currState;
		protected List<int> _terminateStates;
		protected List<List<char>> _eventsList;

		public Parser(StringsBuffer strBuffer, string fileName)
		{
			_strBuffer = strBuffer;
			_stateMachineTable = GetStateMacineTableFromFile(fileName);
		}

		private List<List<int>> GetStateMacineTableFromFile(string fileName)
		{
			List<List<int>> table = new List<List<int>>();
			try
			{
				using (StreamReader streamReader = new StreamReader(fileName, Encoding.Default))
				{
					var line = streamReader.ReadLine();
					var eventsNumber = int.Parse(line);
					_eventsList = new List<List<char>>();
					for (var i = 0; i < eventsNumber; ++i)
					{
						line = streamReader.ReadLine();
						_eventsList.Add(Array.ConvertAll(line.Split(' '), char.Parse).ToList());
					}

					line = streamReader.ReadLine();
					_terminateStates = Array.ConvertAll(line.Split(' '), int.Parse).ToList();
					while ((line = streamReader.ReadLine()) != null)
					{
						List<int> item = Array.ConvertAll(line.Split(' '), int.Parse).ToList();
						table.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return table;
		}

		public TokenType GetTokenType(int startPos, ref int currPos)
		{
			var startIndex = startPos;
			var pos = startPos;
			for (var i = startIndex; i < _strBuffer.CurrStringCount; ++i)
			{
				pos = i;
				var status = CheckState(_strBuffer.CurrString[pos]);
				if (status == StateMachineStatus.END)
				{
					currPos = pos;

					return ChooseToken();
				}
				else if (status == StateMachineStatus.ERROR)
				{
					return TokenType.UNCKNOWN;
				}
			}

			if (IsTerminalState())
			{
				currPos = pos;

				return ChooseToken();
			}
			else
			{
				return TokenType.UNCKNOWN;
			}
		}

		public void ResetState()
		{
			_currState = 0;
		}

		private StateMachineStatus CheckState(char simbol)
		{
			for (var i = 0; i < _stateMachineTable[_currState].Count; ++i)
			{
				var nextState = _stateMachineTable[_currState][i];
				if (nextState != ErrorState && _eventsList[i].Contains(simbol))
				{
					_currState = nextState;

					return StateMachineStatus.CONTINUE;
				}
			}

			if (IsTerminalState())
			{
				if (Config.Delimeters.ContainsKey(simbol))
				{
					return StateMachineStatus.END;
				}
			}

			return StateMachineStatus.ERROR;
		}

		private bool IsTerminalState()
		{
			return _terminateStates.Contains(_currState);
		}

		protected abstract TokenType ChooseToken();
	}
}
