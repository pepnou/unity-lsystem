using System.Collections.Generic;
using System.Linq;
using System.IO;

using sly.lexer;
using sly.parser.generator;
using sly.parser.parser;

using LSystem.model;

using UnityEngine;


namespace LSystem.parser
{

    public class LSystemParser
    {
        public LSystemParser()
        {

        }

        /*[Operation((int) LSystemToken.PLUS          , Affix.InFix, Associativity.Left, 4)]
        [Operation((int) LSystemToken.MINUS         , Affix.InFix, Associativity.Left, 4)]
        [Operation((int) LSystemToken.TIMES         , Affix.InFix, Associativity.Left, 3)]
        [Operation((int) LSystemToken.DIVIDE        , Affix.InFix, Associativity.Left, 3)]
        [Operation((int) LSystemToken.MODULUS       , Affix.InFix, Associativity.Left, 3)]
        [Operation((int) LSystemToken.POWER         , Affix.InFix, Associativity.Left, 3)]

        [Operation((int) LSystemToken.EQUALS        , Affix.InFix, Associativity.Left, 7)]
        [Operation((int) LSystemToken.DIFFERENT     , Affix.InFix, Associativity.Left, 7)]
        [Operation((int) LSystemToken.LESSER_EQUAL  , Affix.InFix, Associativity.Left, 6)]
        [Operation((int) LSystemToken.GREATER_EQUAL , Affix.InFix, Associativity.Left, 6)]
        [Operation((int) LSystemToken.LESSER        , Affix.InFix, Associativity.Left, 6)]
        [Operation((int) LSystemToken.GREATER       , Affix.InFix, Associativity.Left, 6)]

        [Operation((int)LSystemToken.LOGICAL_AND    , Affix.InFix, Associativity.Left, 11)]
        [Operation((int)LSystemToken.LOGICAL_OR     , Affix.InFix, Associativity.Left, 12)]

        [Operation((int)LSystemToken.RIGHT_SHIFT    , Affix.InFix, Associativity.Left, 5)]
        [Operation((int)LSystemToken.LEFT_SHIFT     , Affix.InFix, Associativity.Left, 5)]
        [Operation((int)LSystemToken.AND            , Affix.InFix, Associativity.Left, 8)]
        [Operation((int)LSystemToken.XOR            , Affix.InFix, Associativity.Left, 9)]
        [Operation((int)LSystemToken.OR             , Affix.InFix, Associativity.Left, 10)]*/

        [Operation((int) LSystemToken.TIMES         , Affix.InFix, Associativity.Left, 10)]
        [Operation((int) LSystemToken.DIVIDE        , Affix.InFix, Associativity.Left, 10)]
        [Operation((int) LSystemToken.MODULUS       , Affix.InFix, Associativity.Left, 10)]
        [Operation((int) LSystemToken.POWER         , Affix.InFix, Associativity.Left, 10)]
        [Operation((int) LSystemToken.PLUS          , Affix.InFix, Associativity.Left, 9)]
        [Operation((int) LSystemToken.MINUS         , Affix.InFix, Associativity.Left, 9)]
        
        [Operation((int)LSystemToken.RIGHT_SHIFT    , Affix.InFix, Associativity.Left, 8)]
        [Operation((int)LSystemToken.LEFT_SHIFT     , Affix.InFix, Associativity.Left, 8)]

        [Operation((int) LSystemToken.LESSER_EQUAL  , Affix.InFix, Associativity.Left, 7)]
        [Operation((int) LSystemToken.GREATER_EQUAL , Affix.InFix, Associativity.Left, 7)]
        [Operation((int) LSystemToken.LESSER        , Affix.InFix, Associativity.Left, 7)]
        [Operation((int) LSystemToken.GREATER       , Affix.InFix, Associativity.Left, 7)]
        
        [Operation((int) LSystemToken.EQUALS        , Affix.InFix, Associativity.Left, 6)]
        [Operation((int) LSystemToken.DIFFERENT     , Affix.InFix, Associativity.Left, 6)]

        [Operation((int)LSystemToken.AND            , Affix.InFix, Associativity.Left, 5)]
        [Operation((int)LSystemToken.XOR            , Affix.InFix, Associativity.Left, 4)]
        [Operation((int)LSystemToken.OR             , Affix.InFix, Associativity.Left, 3)]

        [Operation((int)LSystemToken.LOGICAL_AND    , Affix.InFix, Associativity.Left, 2)]
        [Operation((int)LSystemToken.LOGICAL_OR     , Affix.InFix, Associativity.Left, 1)]

        public LSystemAST binaryExpression(LSystemAST left, Token<LSystemToken> operation, LSystemAST right)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new BinaryOperation(left as Expression, (BinaryOperator)operation.TokenID, right as Expression);
        }

        [Operation((int)LSystemToken.LOGICAL_NOT    , Affix.PreFix, Associativity.Right, 11)]
        [Operation((int)LSystemToken.NOT            , Affix.PreFix, Associativity.Right, 11)]
        public LSystemAST unaryExpression(Token<LSystemToken> operation, LSystemAST right)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new UnaryOperation((UnaryOperator)operation.TokenID, right as Expression);
        }

        [Operation((int)LSystemToken.MINUS, Affix.PreFix, Associativity.Right, 11)]
        public LSystemAST Negation(Token<LSystemToken> operation, LSystemAST right)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new Neg(right as Expression);
        }


        [Production("lSystem: productionRule*")]
        public LSystemAST productionRuleStmt(List<LSystemAST> productionRules)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new LSystemRules(productionRules.Cast<ProductionRule>().ToList());
        }


        [Production("range: LBRACE [d] expression COMMA [d] expression RBRACE [d]")]
        public LSystemAST rangeStmt(LSystemAST range_from, LSystemAST range_to)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new Range(range_from as Expression, range_to as Expression);
        }


        [Production("module: RULENAME")]
        public LSystemAST moduleStmt1(Token<LSystemToken> ruleName)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new Module(ruleName.StringWithoutQuotes);
        }

        [Production("module: RULENAME LPAREN [d] expression ( COMMA [d] expression )* RPAREN [d]")]
        public LSystemAST moduleStmt2(Token<LSystemToken> ruleName, LSystemAST firstParam, List<Group<LSystemToken, LSystemAST>> otherParameters)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            List<Expression> parametters = otherParameters.Select((Group<LSystemToken, LSystemAST> group) => group.Value(0)).Cast<Expression>().ToList();
            parametters.Insert(0, firstParam as Expression);

            return new Module(ruleName.StringWithoutQuotes, parametters);
        }



        /*[Production("primary: FLOAT")]
        public LSystemAST primary1(Token<LSystemToken> value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new FloatConstant(value.StringWithoutQuotes);
        }

        [Production("primary: VARIABLE")]
        public LSystemAST primary2(Token<LSystemToken> value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new Variable(value.StringWithoutQuotes);
        }

        [Production("primary: range")]
        public LSystemAST primary3(LSystemAST range)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return range;
        }

        [Production("primary: LPAREN [d] expression RPAREN [d]")]
        public LSystemAST primary4(LSystemAST expression)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return expression;
        }

        [Operand]
        [Production("operand: primary")]
        public LSystemAST operand(LSystemAST value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return value;
        }





        [Production("expression: operand")]
        public LSystemAST expressionStmt2(LSystemAST value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return value;
        }*/

        /*[Production("expression: range")]
        public LSystemAST expressionStmt3(LSystemAST range)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return range;
        }

        [Production("expression: LPAREN [d] expression RPAREN [d]")]
        public LSystemAST expressionStmt4(LSystemAST expression)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return expression;
        }*/

        /*[Production("expression: LSystemParser_expressions")]
        public LSystemAST expressionStmt5(LSystemAST expression)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return expression;
        }*/




        [Production("primary: INT")]
        public LSystemAST primary1(Token<LSystemToken> value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            Debug.Log(value.StringWithoutQuotes);
            //return new FloatConstant(value.StringWithoutQuotes);
            return new FloatConstant(value.DoubleValue);
        }

        [Production("primary: DOUBLE")]
        public LSystemAST primary2(Token<LSystemToken> value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            Debug.Log(value.StringWithoutQuotes);
            //return new FloatConstant(value.StringWithoutQuotes);
            return new FloatConstant(value.DoubleValue);
        }

        [Production("primary: VARIABLE")]
        public LSystemAST primary3(Token<LSystemToken> value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new Variable(value.StringWithoutQuotes);
        }

        [Production("primary: range")]
        public LSystemAST primary4(LSystemAST range)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return range;
        }

        [Production("primary: LPAREN [d] expression RPAREN [d]")]
        public LSystemAST primary5(LSystemAST expression)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return expression;
        }

        [Operand]
        [Production("operand: primary")]
        public LSystemAST operand(LSystemAST value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return value;
        }

        [Production("expression: LSystemParser_expressions")]
        [Production("expression: operand")]
        public LSystemAST expressionStmt5(LSystemAST expression)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return expression;
        }


        [Production("productionRule: RULENAME COLON [d] module* COLON [d] expression  COLON [d] modules* EOL [d]")]
        public LSystemAST productionRuleStmt(Token<LSystemToken> ruleName, List<LSystemAST> predecessors, LSystemAST condition, List<LSystemAST> successors)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return null;
            /*return new ProductionRule(
                ruleName.StringWithoutQuotes,
                predecessors.Cast<Module>().ToList(),
                condition as Expression,
                successors.Cast<Module>().ToList());*/
        }

        [Production("test: RULENAME_")]
        public LSystemAST test(Token<LSystemToken> value)
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
            return new Variable(value.StringWithoutQuotes);
        }
    }
}