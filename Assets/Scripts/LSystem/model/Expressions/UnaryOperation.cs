using System.Collections.Generic;
using System.Text;

using UnityEngine;

namespace LSystem.model
{
    public class UnaryOperation : Expression
    {
        public string OperatorStr { get; set; }
        public Expression Right { get; set; }

        public UnaryOperation(string oper, Expression right)
        {
            OperatorStr = oper;
            Right = right;
        }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "UNARY OPERATION: [" + OperatorStr + "]");
            dmp.AppendLine(Right?.Dump(tab + "\t"));
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            return Right.CheckContext(globalCtx, ctx);
        }

        public double Evaluate(in Dictionary<string, double> globalCtx, in Dictionary<string, double> ctx)
        {
            switch(OperatorStr)
            {
                /*case "!":
                    return !Right.Evaluate(globalCtx, ctx);
                case "~":
                    return ~Right.Evaluate(globalCtx, ctx);*/
                case "-":
                    return -Right.Evaluate(globalCtx, ctx);
                default:
                    Debug.LogError($"Unknown unary operator[{OperatorStr}]");
                    return 0;
            }
        }
    }
}

