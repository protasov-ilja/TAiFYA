using Lexer.lexer.enums;
using Lexer.lexer.parsers;
using Lexer.lexer.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lexer.lexer
{
    class LexerManager
    {
		private Dictionary<char, TokenType> m_delimeters = new Dictionary<char, TokenType>()
		{
			{ ' ' , TokenType.ERROR },
			{ '.' , TokenType.ERROR }
		};

		private List<KeyValuePair<TokenType, string>> _tokens = new List<KeyValuePair<TokenType, string>>();

		private StringsBuffer m_stringBuffer;

		private IdentParser m_identParser;
		private HexParser m_hexParser;
		//private List<List<int>> m_octTable;
		//private List<List<int>> m_binTable;
		//private List<List<int>> m_dexTable;

		private int m_startTokenPos;
		private int m_currPos;

		public LexerManager(String fileName)
		{
			try
			{
				using (StreamReader streamReader = new StreamReader(fileName, Encoding.Default))
				{
					m_stringBuffer = new StringsBuffer(streamReader);
					m_identParser = new IdentParser(m_stringBuffer, "var.txt");
					m_hexParser = new HexParser(m_stringBuffer, "hex.txt");
					TokenType t = TokenType.LOOP;
					while (t != TokenType.ERROR)
					{
						t = GetNextToken();
						Console.WriteLine(t);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private TokenType GetNextToken()
		{
			while (!m_stringBuffer.IsFileEnded)
			{
				for (; m_currPos < m_stringBuffer.CurrStringCount; ++m_currPos)
				{
					m_startTokenPos = m_currPos;
					//var token = m_identParser.GetToken(ref m_startTokenPos, ref m_currPos);
					//m_identParser.ResetState();
					//if (token != TokenType.ERROR)
					//{
					//	Console.WriteLine(token + " : " + m_currPos);

					//	if (token == TokenType.IDENTIFIER)
					//	{
					//		//Console.WriteLine(m_startTokenPos + " c: " + m_currPos);
					//		//var strToken = GetRawString(m_startTokenPos, m_currPos);
					//		//Console.WriteLine(strToken);
					//		//Console.WriteLine("++");
					//		//if (strToken)
					//		//{

					//		//}

					//	}

					//	return token;
					//}

					var token = m_hexParser.GetToken(ref m_startTokenPos, ref m_currPos);
					m_hexParser.ResetState();
					if (token != TokenType.ERROR)
					{
						return token;
					}
				
					m_startTokenPos++;
				}
				Console.WriteLine();
				Console.WriteLine(m_startTokenPos);
				m_currPos = 0;
				if (m_stringBuffer.IsBufferSwitched)
				{
					m_stringBuffer.ReadNewStr();
				}
				else
				{
					m_stringBuffer.SwitchStr(true);
				}
			}

			return TokenType.ERROR;
		}

		private string GetRawString(int startPos, int currPos)
		{
			if (startPos < currPos)
			{
				//Console.WriteLine("++");
				return m_stringBuffer.CurrSring.Substring(startPos, currPos);
			}
			else if (startPos > currPos)
			{
				var str = m_stringBuffer.CurrSring.Substring(0, currPos);
				m_stringBuffer.SwitchStr(false);
				str += m_stringBuffer.CurrSring.Substring(startPos, m_stringBuffer.CurrStringCount);
				m_stringBuffer.SwitchStr(false);

				return str;
			}

			return m_stringBuffer.CurrSring[startPos].ToString();
		}

		public bool IsVarDelimeter(char simbol)
		{
			return m_delimeters.ContainsKey(simbol);
		}
	}
}
