using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lexer.lexer.utils
{
    public class StringsBuffer
	{ 
		private List<string> m_strings = new List<string>();
		private int m_prevString = 1;
		private int m_currString = 0;
		private StreamReader m_strmR;
		private bool m_isFileEnded = false;
		private bool m_isFirstLoop = true;

		public string CurrSring
		{
			get {
				//Console.WriteLine("++");
				//Console.WriteLine(m_strings[m_currString]);
				return (m_isFileEnded) ? null : m_strings[m_currString]; }
		}

		public int CurrStringCount
		{
			get { return (m_isFileEnded) ? 0 : m_strings[m_currString].Length; }
		}

		public bool IsBufferSwitched { get; set; }

		public bool IsFileEnded
		{
			get { return m_isFileEnded; }
		}

		public StringsBuffer(StreamReader strmR, int stringsCount = 2)
		{
			m_strmR = strmR;
			for (var i = 0; i < stringsCount; ++i)
			{
				string line;
				if ((line = m_strmR.ReadLine()) != null)
				{
					m_strings.Add(line);
				}
			}
		}

		public void SwitchStr(bool isNeedToReadNew)
		{
			if (m_isFirstLoop)
			{
				var tempIndex = m_currString;
				m_currString = m_prevString;
				m_prevString = tempIndex;
				IsBufferSwitched = (m_currString != 0);
				if (isNeedToReadNew)
				{
					m_isFirstLoop = false;
				}
			}
			else if (isNeedToReadNew && !m_isFirstLoop)
			{
				ReadNewStr();
			}
		}

		public void ReadNewStr()
		{
			var tempIndex = m_currString;
			m_currString = m_prevString;
			m_prevString = tempIndex;
			IsBufferSwitched = (m_currString != 0);
			string line;
			if ((line = m_strmR.ReadLine()) != null)
			{
				m_strings[m_currString] = line;
				m_isFirstLoop = false;
			}
			else
			{
				m_isFileEnded = true;
			}	
		}
    }
}
