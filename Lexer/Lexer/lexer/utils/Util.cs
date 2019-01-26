using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer.lexer.utils
{
    public class Util
    {
		public static bool IsSimbol(char simbol)
		{
			return ((simbol >= 'a') && (simbol <= 'z')) || ((simbol >= 'A') && (simbol <= 'Z')) || (simbol == '_');
		}

		public static bool IsDexNumber(char simbol)
		{
			return (simbol <= '9') && (simbol >= '0');
		}

		public static bool IsHexNumber(char simbol)
		{
			return IsDexNumber(simbol) || ((simbol <= 'F') && (simbol >= 'A')) || ((simbol <= 'f') && (simbol >= 'a'));
		}

		public static bool IsOctNumber(char simbol)
		{
			return (simbol <= '7') && (simbol >= '0');
		}

		public static bool IsBinNumber(char simbol)
		{
			return (simbol == '1') || (simbol == '0');
		}
	}
}
