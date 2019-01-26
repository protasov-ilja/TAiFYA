using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer.lexer.enums
{
    public enum TokenType
    {
		SQUARE_BRACKETS, // []
		ROUND_BRACKETS, // ()
		BRACES, // {}
		COMMENT, // /* */ // 
		ACCESS_MODIFICATOR, // public private protected
		LOOP, // while for do
		IDENTIFIER, // var int float bool double char
		VARIABLE, // __asdasdasdasd
		OPERATOR, // = -= += == <= >= !=
		CONDITIONS, // if else ifelse
		DOT, // .
		NUMBER, // 1 2 3 123.3 12.3+E23
		BOOL_OPERATOR, // false true
		SEMICOLON, // ;
		ERROR,
		HEX_NUMBER, // 0h123FF
		BIN_NUMBER // 0b010100
	};
}
