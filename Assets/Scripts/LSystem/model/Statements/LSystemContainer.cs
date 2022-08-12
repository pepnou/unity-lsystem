using System.Linq;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

namespace LSystem.model
{
    public class LSystemContainer : Statement
    {
        public List<Assignement> GlobalVariables { get; set; }
        public List<RuleCall> Axiom { get; set; }
        public List<ProductionRule> Rules { get; set; }
        public LSystemContainer(List<Assignement> globalVariables, List<RuleCall> axiom, List<ProductionRule> rules)
        {
            GlobalVariables = globalVariables;
            Axiom = axiom;
            Rules = rules;
        }
        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            if(GlobalVariables != null && GlobalVariables.Count > 0)
            {
                dmp.AppendLine(tab + "GLOBAL VARIABLE:");
                foreach(Assignement a in GlobalVariables)
                {
                    dmp.AppendLine(a.Dump(tab + "\t"));
                }
            }
            dmp.AppendLine(tab + "LSYSTEM:");
            foreach(ProductionRule p in Rules)
            {
                dmp.AppendLine(p.Dump(tab + "\t"));
            }
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            List<string> newGlobalCtx = new List<string>();
            foreach (Assignement a in GlobalVariables)
            {
                if (!newGlobalCtx.Contains(a.Var.Name))
                {
                    newGlobalCtx.Add(a.Var.Name);
                }
                else
                {
                    Debug.LogError($"Global variable [{a.Var.Name}] line: {a.Var.Position.x}, column: {a.Var.Position.y} already exist.");
                    return false;
                }
            }

            bool res = true;
            foreach(ProductionRule p in Rules)
            {
                res = res && p.CheckContext(globalCtx, ctx);
            }
            return res;
        }

        public bool CollapseRuleByProbability()
        {
            bool res = true;
            for (int i = 0; i < Rules.Count; i++)
            {
                for (int j = Rules.Count - 1; j > i; j--)
                {
                    if(Rules[i].Name == Rules[j].Name)
                    {
                        res = res && Rules[i].CollapseRuleByProbability(Rules[j]);
                        Rules.RemoveAt(j);
                    }
                }
            }
            return res;
        }

        public bool MatchRuleDefinition(LSystemContainer rules)
        {
            bool res = true;
            foreach (RuleCall m in Axiom)
            {
                res = res && m.MatchRuleDefinition(this);
            }
            /*foreach (ProductionRule p in Rules)
            {
                res = res && p.MatchRuleDefinition(this);
            }*/
            int length = Rules.Count;
            for(int i = 0; i < length; i++)
            {
                res = res && Rules[i].MatchRuleDefinition(this);
            }
            return res;
        }

        public bool CheckRuleParametters()
        {
            bool res = true;
            foreach (ProductionRule p in Rules)
            {
                res = res && p.CheckRuleParametters();
            }
            return res;
        }

        public void EvaluateGlobalCtx(ref Dictionary<string, double> globalCtx)
        {
            globalCtx = GlobalVariables.ToDictionary(x => x.Var.Name, x => x.Value.Evaluate(null, null));
        }

        public void Evaluate(ref List<(ProductionRule rule, Dictionary<string, double> ctx)> previousIteration, in Dictionary<string, double> globalCtx)
        {
            List<(ProductionRule rule, Dictionary<string, double> ctx)> newIteration = new List<(ProductionRule rule, Dictionary<string, double> ctx)>();
            Dictionary<string, double> ctx = new Dictionary<string, double>();

            if (previousIteration == null)
            {
                foreach(RuleCall m in Axiom)
                {
                    m.Evaluate(ref newIteration, globalCtx, ctx);
                }
            } else
            {
                foreach((ProductionRule rule, Dictionary<string, double> ctx) x in previousIteration)
                {
                    x.rule.Evaluate(ref newIteration, globalCtx, x.ctx);
                }
            }

            previousIteration = newIteration;
        }

        public void Evaluate(ref List<(ProductionRule rule, Dictionary<string, double> ctx)> newIteration, in Dictionary<string, double> globalCtx, Dictionary<string, double> ctx)
        {
            Evaluate(ref newIteration, globalCtx);
        }
    }
}

