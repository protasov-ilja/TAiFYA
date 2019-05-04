namespace Lexer.Lexer.Enums
{
    public enum TokenType
    {
		SQUARE_BRACKET_OPEN, // [
		SQUARE_BRACKET_CLOSE, // ]
		ROUND_BRACKET_OPEN, // (
		ROUND_BRACKET_CLOSE, // )
		BRACE_OPEN, // {
		BRACE_CLOSE, // }
		COMMENT, // /* */ // 
		ACCESS_MODIFICATOR, // public private protected
		WHILE_LOOP, // while  
		FOR_LOOP, //for
		DO_LOOP, // do
		STRING, // "sth"
		IDENTIFIER, // var int float bool double char
		VAR_IDENTIFIER, // var
		VARIABLE, // __asdasdasdasd
		OPERATOR, // -= += == <= >= !=
		EQUALITY_OPERATOR, // = 
		CONDITIONS, // if else ifelse
		DOT, // .
		COMMA, // ,
		INT_NUMBER, // 1 2 3 123.3 12.3+E23
		FLOAT_NUMBER, // 111.01
		E_NUMBER, // 12.12E+(-)32.2
		BOOL_OPERATOR, // false true
		SEMICOLON, // ;
		HEX_NUMBER, // 0h123FF
		BIN_NUMBER, // 0b010100
		OCT_NUMBER, // 0o1234567
		UNCKNOWN, // uncknown
		DELIMETER, // sth
		ERROR,
		END
	};
}
