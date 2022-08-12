using System.Collections.Generic;
using System.Text;

using UnityEngine;

using System;

namespace LSystem.model
{
    public class BinaryOperation : Expression
    {
        public Expression Left { get; set; }
        public string OperatorStr { get; set; }
        public Expression Right { get; set; }

        public BinaryOperation(Expression left, string oper, Expression right)
        {
            Left = left;
            OperatorStr = oper;
            Right = right;
        }
        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "Binary OPERATION: [" + OperatorStr + "]");
            dmp.AppendLine(Left?.Dump(tab + "\t"));
            dmp.AppendLine(Right?.Dump(tab + "\t"));
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            return Left.CheckContext(globalCtx, ctx) && Right.CheckContext(globalCtx, ctx);
        }

        public double Evaluate(in Dictionary<string, double> globalCtx, in Dictionary<string, double> ctx)
        {
            switch (OperatorStr)
            {
                case "*":
                    return Left.Evaluate(globalCtx, ctx) * Right.Evaluate(globalCtx, ctx);
                case "/":
                    return Left.Evaluate(globalCtx, ctx) / Right.Evaluate(globalCtx, ctx);
                case "%":
                    return Left.Evaluate(globalCtx, ctx) % Right.Evaluate(globalCtx, ctx);
                case "**":
                    return Math.Pow(Left.Evaluate(globalCtx, ctx), Right.Evaluate(globalCtx, ctx));
                case "+":
                    return Left.Evaluate(globalCtx, ctx) + Right.Evaluate(globalCtx, ctx);
                case "-":
                    return Left.Evaluate(globalCtx, ctx) - Right.Evaluate(globalCtx, ctx);
                /*case "<<":
                    return Left.Evaluate(globalCtx, ctx) << Right.Evaluate(globalCtx, ctx);
                case ">>":
                    return Left.Evaluate(globalCtx, ctx) >> Right.Evaluate(globalCtx, ctx);*/
                case "<=":
                    return (Left.Evaluate(globalCtx, ctx) <= Right.Evaluate(globalCtx, ctx)) ? 1 : 0;
                case ">=":
                    return (Left.Evaluate(globalCtx, ctx) >= Right.Evaluate(globalCtx, ctx)) ? 1 : 0;
                case "<":
                    return (Left.Evaluate(globalCtx, ctx) < Right.Evaluate(globalCtx, ctx)) ? 1 : 0;
                case ">":
                    return (Left.Evaluate(globalCtx, ctx) > Right.Evaluate(globalCtx, ctx)) ? 1 : 0;
                case "==":
                    return (Left.Evaluate(globalCtx, ctx) == Right.Evaluate(globalCtx, ctx)) ? 1 : 0;
                case "!=":
                    return (Left.Evaluate(globalCtx, ctx) != Right.Evaluate(globalCtx, ctx)) ? 1 : 0;
                /*case "&":
                    return Left.Evaluate(globalCtx, ctx) & Right.Evaluate(globalCtx, ctx);
                case "|":
                    return Left.Evaluate(globalCtx, ctx) | Right.Evaluate(globalCtx, ctx);
                case "^":
                    return Left.Evaluate(globalCtx, ctx) ^ Right.Evaluate(globalCtx, ctx);*/
                case "&&":
                    return (Left.Evaluate(globalCtx, ctx) != 0 && Right.Evaluate(globalCtx, ctx) != 0) ? 1 : 0;
                case "||":
                    return (Left.Evaluate(globalCtx, ctx) != 0 || Right.Evaluate(globalCtx, ctx) != 0) ? 1 : 0;
                default:
                    Debug.LogError($"Unknown binary operator[{OperatorStr}]");
                    return 0;
            }
        }
    }
}

