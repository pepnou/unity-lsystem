using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

namespace LSystem.model
{
    public class Module : RuleCall
    {
        public string Name { get; set; }
        public Vector2Int Position { get; set; }
        public List<Expression> Parametters { get; set; }
        public ProductionRule Definition { get; set; }

        public Module(string name, Vector2Int position, List<Expression> parametters)
        {
            Name = name;
            Position = position;
            Parametters = parametters;
        }
        public Module(string name, Vector2Int position)
        {
            Name = name;
            Position = position;
            Parametters = null;
        }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "MODULE:");
            dmp.AppendLine(tab + "\tNAME: " + Name);
            if (Parametters != null && Parametters.Count > 0)
            {
                dmp.AppendLine(tab + "\tPARAMETTERS: ");
                foreach(Expression e in Parametters)
                {
                    dmp.AppendLine(e.Dump(tab + "\t\t"));
                }
            }
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            bool res = true;
            foreach(Expression e in Parametters)
            {
                res = res && e.CheckContext(globalCtx, ctx);
            }
            return res;
        }

        public bool MatchRuleDefinition(LSystemContainer rules)
        {
            Definition = rules.Rules.Find(x => x.Name == Name);

            if (Definition == null)
            {
                //Debug.LogError($"In module {Name} line: {Position.x}, column: {Position.y}, rule \"{Name}\" isn't defined.");

                List<Variable> parametters = new List<Variable>();
                for(int i = 0; i < Parametters.Count; i++)
                {
                    parametters.Add(new Variable($"p{i}"));
                }

                ProductionRule newRule = new ProductionRule(Name, parametters);
                rules.Rules.Add(newRule);
                Definition = newRule;
            }

            return Definition != null;
        }

        public bool CheckRuleParametters()
        {
            bool res = Parametters.Count == Definition.Parametters.Count;
            if (!res)
                Debug.LogError($"Module \"{Name}\" line: {Position.x}, column: {Position.y} defined line: {Definition.Position.x}, column: {Definition.Position.y} have different parametter count than definition.");
            return res;
        }

        public void Evaluate(ref List<(ProductionRule rule, Dictionary<string, double> ctx)>  newIteration, in Dictionary<string, double> globalCtx, Dictionary<string, double> ctx)
        {
            Dictionary<string, double> newCtx = new Dictionary<string, double>();
            for (int i = 0; i < Parametters.Count; i++)
            {
                newCtx.Add(Definition.Parametters[i].Name, Parametters[i].Evaluate(globalCtx, ctx));
            }

            newIteration.Add((Definition, newCtx));
        }
    }
}

