using System.Collections.Generic;
using System.IO;

namespace Lexer.Lexer.Utils
{
	public sealed class StringsBuffer
	{ 
		private List<string> _strings = new List<string>();
		private int _prevString = 1;
		private int _currString = 0;
		private StreamReader _strmR;
		private bool _isFileEnded = false;
		private bool _canRead = true;

		private bool _isFirstLoop = true;

		public string CurrString => (_isFileEnded) ? null : _strings[_currString];

		public int CurrStringCount => (_isFileEnded) ? 0 : _strings[_currString].Length;

		public bool IsFileEnded => _isFileEnded;

		public StringsBuffer(StreamReader strmR, int stringsCount = 2)
		{
			_strmR = strmR;
			for (var i = 0; i < stringsCount; ++i)
			{
				string line;
				if (_canRead && (line = _strmR.ReadLine()) != null)
				{
					_strings.Add(line + "\n");
				}
				else
				{
					_canRead = false;
				}
			}
		}

		public void GetNewString()
		{
			var tempIndex = _currString;
			_currString = _prevString;
			_prevString = tempIndex;
			string line;
			
			if (!_isFirstLoop)
			{
				if ((line = _strmR.ReadLine()) != null)
				{
					_strings[_currString] = line + "\n";
				}
				else
				{
					_isFileEnded = true;
				}
			}
			else
			{
				_isFirstLoop = false;
			}
		}
	}
}
