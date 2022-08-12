using System;
using System.Collections;
using System.Collections.Generic;


namespace LSystem.model
{
    public interface Statement : LSystemAST
    {
        public bool MatchRuleDefinition(LSystemContainer rules);
        public bool CheckRuleParametters();
        public void Evaluate(ref List<(ProductionRule rule, Dictionary<string, double> ctx)> newIteration, in Dictionary<string, double> globalCtx, Dictionary<string, double> ctx);
    }
}