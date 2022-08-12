using System;
using System.Collections.Generic;
using System.Text;

namespace LSystem.model
{
    public class FloatConstant : Expression
    {
        public double Value { get; set; }
        public FloatConstant(double value)
        {
            Value = value;
        }
        public FloatConstant(string value)
        {
            Value = Convert.ToDouble(value);
        }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "FLOAT CONSTANT: " + Value);
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            return true;
        }

        public double Evaluate(in Dictionary<string, double> globalCtx, in Dictionary<string, double> ctx)
        {
            return Value;
        }
    }
}

