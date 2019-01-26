using Lexer.lexer.enums;
using Lexer.lexer.utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lexer.lexer.parsers
{
    public abstract class Parser
    {
		protected List<List<int>> stateMachineTable;
		protected StringsBuffer strBuffer;
		protected int currState;
		protected List<int> terminateStates;

		public Parser(StringsBuffer strBuffer, string fileName)
		{
			this.strBuffer = strBuffer;
			stateMachineTable = GetStateMacineTableFromFile(fileName);
			Console.WriteLine("Parser");
		}

		private List<List<int>> GetStateMacineTableFromFile(string fileName)
		{
			List<List<int>> table = new List<List<int>>();
			try
			{
				using (StreamReader streamReader = new StreamReader(fileName, Encoding.Default))
				{
					var line = streamReader.ReadLine();
					terminateStates = Array.ConvertAll(line.Split(' '), int.Parse).ToList();
					while ((line = streamReader.ReadLine()) != null)
					{
						List<int> item = Array.ConvertAll(line.Split(' '), int.Parse).ToList();
						table.Add(item);
					}

					Console.WriteLine("GetStateMacineTableFromFile");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return table;
		}

		public TokenType GetToken(ref int startPos, ref int currPos)
		{
			var startIndex = startPos;
			while (true)
			{
				for (var i = startIndex; i < strBuffer.CurrStringCount; ++i)
				{
					Console.Write(strBuffer.CurrSring[i]);
					var status = CheckState(strBuffer.CurrSring[i]);
					//Console.Write(status);
					if (status == StateMachineStatus.END)
					{
						if (IsTerminalState())
						{
							currPos = i;
							Console.Write("ChooseToken: ");
							return ChooseToken();
						}
						else
						{
							return TokenType.ERROR;
						}
					}
					else if (status == StateMachineStatus.ERROR)
					{
						return TokenType.ERROR;
					}
				}
				Console.WriteLine();

				startIndex = 0;
				if (strBuffer.IsBufferSwitched)
				{
					strBuffer.ReadNewStr();
					if (strBuffer.IsFileEnded)
					{
						if (IsTerminalState())
						{
							Console.Write("ChooseToken: ");
							currPos = startIndex;
							return ChooseToken();
						}
						else
						{
							return TokenType.ERROR;
						}
					}
				}
				else
				{
					strBuffer.SwitchStr(true);
					if (strBuffer.IsFileEnded)
					{
						if (IsTerminalState())
						{
							Console.Write("ChooseToken: ");
							currPos = startIndex;
							return ChooseToken();
						}
						else
						{
							return TokenType.ERROR;
						}
					}
				}
			}
		}

		public void ResetState()
		{
			currState = 0;
		}

		protected abstract StateMachineStatus CheckState(char simbol);

		protected abstract bool IsTerminalState();

		protected abstract TokenType ChooseToken();
	}
}
