using sly.lexer;

namespace LSystem.parser
{
    [Lexer(IgnoreWS = true, WhiteSpace = new char[] { ' ', '\t' , '\r' })]
    public enum LSystemToken
    {
#if (false)
        #region litterals 10 -> 19
        [Lexeme("[0-9]+(\\.[0-9]+)?")] FLOAT = 10,
        [Lexeme("[a-z_][a-zA-Z0-9_]*")] VARIABLE = 11,
        [Lexeme("[A-Z][a-zA-Z0-9_]*")] RULENAME = 12,
        #endregion


        #region arithmeticOperators 20 -> 29
        [Lexeme("\\+")] PLUS = 20,
        [Lexeme("\\-")] MINUS = 21,
        [Lexeme("\\*")] TIMES = 22,
        [Lexeme("\\/")] DIVIDE = 23,
        [Lexeme("\\*\\*")] POWER = 24,
        [Lexeme("%")] MODULUS = 25,
        #endregion


        #region conditionOperators 30 -> 39
        [Lexeme("==")] EQUALS = 30,
        [Lexeme("!=")] DIFFERENT = 31,
        [Lexeme(">=")] GREATER_EQUAL = 32,
        [Lexeme("<=")] LESSER_EQUAL = 33,
        [Lexeme(">")] GREATER = 34,
        [Lexeme("<")] LESSER = 35,
        #endregion


        #region logicalOperators 40 -> 49
        [Lexeme("&&")] LOGICAL_AND = 40,
        [Lexeme("\\|\\|")] LOGICAL_OR = 41,
        [Lexeme("!")] LOGICAL_NOT = 42,
        #endregion


        #region binaryOperators 50 -> 59
        [Lexeme("~")] NOT = 50,
        [Lexeme(">>")] RIGHT_SHIFT = 51,
        [Lexeme("<<")] LEFT_SHIFT = 52,
        [Lexeme("&")] AND = 53,
        [Lexeme("^")] XOR = 54,
        [Lexeme("|")] OR = 55,
        #endregion


        #region other 0 -> 9
        [Lexeme("[ \\t]+", isSkippable: true)] WS = 0,
        [Lexeme("[\\r?\\n]", isSkippable: false, isLineEnding: true)] EOL = 1,
        #endregion

        #region sugar 60 -> 69
        [Lexeme(",")] COMMA = 60,
        [Lexeme(":")] COLON = 61,
        [Lexeme("\\(")] LPAREN = 62,
        [Lexeme("\\)")] RPAREN = 63,
        [Lexeme("{")] LBRACE = 64,
        [Lexeme("}")] RBRACE = 65,
        #endregion
        
#else
        

        #region litterals 10 -> 19
        [Lexeme(GenericToken.Int)] INT = 10,
        [Lexeme(GenericToken.Double)] DOUBLE = 11,
        //[Lexeme(GenericToken.Identifier, IdentifierType.Custom, "a-z", "_0-9a-z")] VARIABLE = 12,
        [Lexeme(GenericToken.KeyWord, "YYY")] VARIABLE = 12,
        //[Lexeme(GenericToken.Identifier, IdentifierType.Custom, "A-Z", "_0-9a-z")] RULENAME = 13,
        [Lexeme(GenericToken.KeyWord, "ZZZ")] RULENAME = 13,

        [Lexeme(GenericToken.Extension)] VARIABLE_ = 14,
        [Lexeme(GenericToken.Extension)] RULENAME_ = 15,
        #endregion


        #region arithmeticOperators 20 -> 29
        [Lexeme(GenericToken.SugarToken, "+")] PLUS = 20,
        [Lexeme(GenericToken.SugarToken, "-")] MINUS = 21,
        [Lexeme(GenericToken.SugarToken, "*")] TIMES = 22,
        [Lexeme(GenericToken.SugarToken, "/")] DIVIDE = 23,
        [Lexeme(GenericToken.SugarToken, "**")] POWER = 24,
        [Lexeme(GenericToken.SugarToken, "%")] MODULUS = 25,
        #endregion


        #region conditionOperators 30 -> 39
        [Lexeme(GenericToken.SugarToken, "==")] EQUALS = 30,
        [Lexeme(GenericToken.SugarToken, "!=")] DIFFERENT = 31,
        [Lexeme(GenericToken.SugarToken, ">=")] GREATER_EQUAL = 32,
        [Lexeme(GenericToken.SugarToken, "<=")] LESSER_EQUAL = 33,
        [Lexeme(GenericToken.SugarToken, ">")] GREATER = 34,
        [Lexeme(GenericToken.SugarToken, "<")] LESSER = 35,
        #endregion


        #region logicalOperators 40 -> 49
        [Lexeme(GenericToken.SugarToken, "&&")] LOGICAL_AND = 40,
        [Lexeme(GenericToken.SugarToken, "||")] LOGICAL_OR = 41,
        [Lexeme(GenericToken.SugarToken, "!")] LOGICAL_NOT = 42,
        #endregion


        #region binaryOperators 50 -> 59
        [Lexeme(GenericToken.SugarToken, "~")] NOT = 50,
        [Lexeme(GenericToken.SugarToken, ">>")] RIGHT_SHIFT = 51,
        [Lexeme(GenericToken.SugarToken, "<<")] LEFT_SHIFT = 52,
        [Lexeme(GenericToken.SugarToken, "&")] AND = 53,
        [Lexeme(GenericToken.SugarToken, "^")] XOR = 54,
        [Lexeme(GenericToken.SugarToken, "|")] OR = 55,
        #endregion


        #region other 0 -> 9
        [Lexeme(GenericToken.SugarToken, "\n")] EOL = 0,
        #endregion

        #region sugar 60 -> 69
        [Lexeme(GenericToken.SugarToken, ",")] COMMA = 60,
        [Lexeme(GenericToken.SugarToken, ":")] COLON = 61,
        [Lexeme(GenericToken.SugarToken, "(")] LPAREN = 62,
        [Lexeme(GenericToken.SugarToken, ")")] RPAREN = 63,
        [Lexeme(GenericToken.SugarToken, "{")] LBRACE = 64,
        [Lexeme(GenericToken.SugarToken, "}")] RBRACE = 65,
        #endregion

#endif
    }
}