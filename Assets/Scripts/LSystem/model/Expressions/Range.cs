using System.Collections.Generic;
using System.Text;

using LSystem.utility;

namespace LSystem.model
{
    public class Range : Expression
    {
        public Expression From { get; set; }
        public Expression To { get; set; }
        public Range(Expression from, Expression to)
        {
            From = from;
            To = to;
        }
        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "RANDOM RANGE:");
            dmp.AppendLine(tab + "\tFROM:");
            dmp.AppendLine(From?.Dump("\t\t"));
            dmp.AppendLine(tab + "\tTO:");
            dmp.AppendLine(To?.Dump("\t\t"));
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            return From.CheckContext(globalCtx, ctx) && To.CheckContext(globalCtx, ctx);
        }

        public double Evaluate(in Dictionary<string, double> globalCtx, in Dictionary<string, double> ctx)
        {
            return RandomNumberGenerator.GenerateDouble(From.Evaluate(globalCtx, ctx), To.Evaluate(globalCtx, ctx));
        }
    }
}

