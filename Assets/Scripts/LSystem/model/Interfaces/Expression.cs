using System.Collections;
using System.Collections.Generic;

namespace LSystem.model
{
    public interface Expression : LSystemAST
    {
        public double Evaluate(in Dictionary<string, double> globalCtx, in Dictionary<string, double> ctx);
    }
}