using System.Collections.Generic;
using System.Text;

namespace LSystem.model
{
    public class Neg : Expression
    {
        public Expression Value { get; set; }
        public Neg(Expression value)
        {
            Value = value;
        }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "NEG:");
            dmp.AppendLine(Value?.Dump(tab + "\t"));
            return dmp.ToString();
        }

        public bool CheckContext(List<string> ctx)
        {
            return Value.CheckContext(ctx);
        }
    }
}

