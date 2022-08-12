using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

namespace LSystem.model
{
    public class ModuleLoop : RuleCall
    {
        public Vector2Int Position { get; set; }
        public List<RuleCall> Calls { get; set; }
        public Expression LoopNumber { get; set; }

        public ModuleLoop(List<RuleCall> calls, Expression loopNumber, Vector2Int position)
        {
            Calls = calls;
            LoopNumber = loopNumber;
            Position = position;
        }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "MODULE LOOP:");
            dmp.AppendLine(tab + "\tLOOP NUMBER:");
            dmp.AppendLine(LoopNumber.Dump(tab + "\t\t"));
            dmp.AppendLine(tab + "\tCALLS:");
            foreach (RuleCall r in Calls)
            {
                dmp.AppendLine(r.Dump(tab + "\t\t"));
            }
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            bool res = true;
            foreach (RuleCall r in Calls)
            {
                res = res && r.CheckContext(globalCtx, ctx);
            }
            res = res && LoopNumber.CheckContext(globalCtx, ctx);
            return res;
        }

        public bool MatchRuleDefinition(LSystemContainer rules)
        {
            bool res = true;
            foreach (RuleCall r in Calls)
            {
                res = res && r.MatchRuleDefinition(rules);
            }
            return res;
        }

        public bool CheckRuleParametters()
        {
            bool res = true;
            foreach (RuleCall r in Calls)
            {
                res = res && r.CheckRuleParametters();
            }
            return res;
        }

        public void Evaluate(ref List<(ProductionRule rule, Dictionary<string, double> ctx)> newIteration, in Dictionary<string, double> globalCtx, Dictionary<string, double> ctx)
        {
            double loopNumber = LoopNumber.Evaluate(globalCtx, ctx);
            if(loopNumber < 0)
            {
                Debug.LogError($"In module loop line: {Position.x}, column: {Position.y}, loop number was evaluated to [{loopNumber}] but cannot be lower than 0.");
                return;
            }

            for(int i = 0; i < loopNumber; i++)
            {
                foreach (RuleCall r in Calls)
                {
                    r.Evaluate(ref newIteration, globalCtx, ctx);
                }
            }
        }
    }
}

