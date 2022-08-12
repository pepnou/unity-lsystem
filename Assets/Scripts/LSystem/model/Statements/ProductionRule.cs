using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using UnityEngine;

using LSystem.utility;

namespace LSystem.model
{
    public class ProductionRule : Statement
    {
        public struct RuleContainer
        {
            public Expression Probability;
            public List<RuleCall> Predecessors;
            public Expression Condition;
            public List<RuleCall> Successors;
        };

        public string Name { get; set; }
        public Vector2Int Position { get; set; }
        public List<Variable> Parametters { get; set; }
        public List<RuleContainer> Rules { get; set; }
        public bool isTerminal { get; set; }


        public ProductionRule(string name, Vector2Int position, List<Variable> parametters, Expression probability, List<RuleCall> predecessors, Expression condition, List<RuleCall> sucessors)
        {
            Name = name;
            Position = position;
            Parametters = parametters;
            Rules = new List<RuleContainer>();
            isTerminal = false;

            RuleContainer firstRule;
            firstRule.Probability = probability;
            firstRule.Predecessors = predecessors;
            firstRule.Condition = condition;
            firstRule.Successors = sucessors;

            Rules.Add(firstRule);
        }

        public ProductionRule(string name, List<Variable> parametters)
        {
            Name = name;
            Parametters = parametters;
            Rules = new List<RuleContainer>();
            isTerminal = true;
        }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "PRODUCTION RULE:");
            dmp.AppendLine(tab + "\tNAME: " + Name);
            if (Parametters != null && Parametters.Count > 0)
            {
                dmp.AppendLine(tab + "\tPARAMETTERS: ");
                foreach (Expression e in Parametters)
                {
                    dmp.AppendLine(e.Dump(tab + "\t\t"));
                }
            }
            int index = 0;
            foreach (RuleContainer r in Rules)
            {
                dmp.AppendLine(tab + "\tRULE n°" + index++ + ": ");
                if (r.Probability != null)
                {
                    dmp.AppendLine(tab + "\t\tPROBABILITY: ");
                    dmp.AppendLine(r.Probability.Dump(tab + "\t\t\t"));
                }
                if (r.Predecessors != null && r.Predecessors.Count > 0)
                {
                    dmp.AppendLine(tab + "\t\tPREDECESSORS: ");
                    foreach (RuleCall m in r.Predecessors)
                    {
                        dmp.AppendLine(m.Dump(tab + "\t\t\t"));
                    }
                }
                if (r.Condition != null)
                {
                    dmp.AppendLine(tab + "\t\tCONDITION: ");
                    dmp.AppendLine(r.Condition.Dump(tab + "\t\t\t"));
                }
                dmp.AppendLine(tab + "\t\tSUCCESSORS: ");
                foreach (RuleCall m in r.Successors)
                {
                    dmp.AppendLine(m.Dump(tab + "\t\t\t"));
                }
            }
            return dmp.ToString();
        }
        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            bool res = true;
            List<string> newCtx = new List<string>();
            foreach (Variable v in Parametters)
            {
                if (!newCtx.Contains(v.Name))
                {
                    newCtx.Add(v.Name);
                }
                else
                {
                    Debug.LogError($"In rule {Name} defined line: {Position.x}, column: {Position.y}, Variable \"{v.Name}\" already exist.");
                    return false;
                }
            }

            foreach (RuleContainer r in Rules)
            {
                if (r.Condition != null)
                    res = res && r.Condition.CheckContext(globalCtx, newCtx);

                foreach (RuleCall m in r.Successors)
                {
                    res = res && m.CheckContext(globalCtx, newCtx);
                }
            }
            return res;
        }
        bool CheckProbabilityCollapseParametters(ProductionRule productionRule)
        {
            if (Parametters.Count != productionRule.Parametters.Count)
            {
                Debug.LogError($"Rule {Name} is defined twice: (line: {Position.x}, column: {Position.y}) and (line: {productionRule.Position.x}, column: {productionRule.Position.y}) but Parametters lists don't have the same length.");
                return false;
            }

            for (int i = 0; i < Parametters.Count; i++)
            {
                if (Parametters[i].Name != productionRule.Parametters[i].Name)
                {
                    Debug.LogError($"Rule {Name} is defined twice: line: {Position.x}, column: {Position.y} and line: {productionRule.Position.x}, column: {productionRule.Position.y} but Parametters aren't the same, or in the same order.");
                    return false;
                }
            }

            return true;
        }
        public bool CollapseRuleByProbability(ProductionRule productionRule)
        {
            if(!CheckProbabilityCollapseParametters(productionRule))
                return false;

            Rules.Add(productionRule.Rules[0]);
            return true;
        }

        public bool MatchRuleDefinition(LSystemContainer rules)
        {
            bool res = true;
            foreach(RuleContainer r in Rules)
            {
                foreach(RuleCall m in r.Successors)
                {
                    res = res && m.MatchRuleDefinition(rules);
                }
            }
            return res;
        }

        public bool CheckRuleParametters()
        {
            bool res = true;
            foreach (RuleContainer r in Rules)
            {
                foreach (RuleCall m in r.Successors)
                {
                    res = res && m.CheckRuleParametters();
                }
            }
            return res;
        }

        private RuleContainer? ProbabilityPick(Dictionary<string, double> globalCtx, Dictionary<string, double> ctx)
        {
            (double probability, RuleContainer rule)[] probabilities = Rules.Select(x => (x.Probability == null) ? (Double.NaN, x) : (x.Probability.Evaluate(globalCtx, ctx), x)).ToArray();

            double totalProbability = 0;
            int undefinedProbabilityCount = 0;
            foreach ((double probability, RuleContainer rule) x in probabilities)
            {
                if (Double.IsNaN(x.probability))
                {
                    undefinedProbabilityCount++;
                    continue;
                }

                if (x.probability > 100 || x.probability < 0)
                {
                    Debug.LogError($"In production rule {Name}, the rule probability is evaluated to [{x.probability}]. This value shouldn't be lower than 0 or higher than 100.");
                    return null;
                }

                totalProbability += x.probability;
            }

            if (totalProbability > 100)
            {
                Debug.LogError($"In production rule {Name}, the sum of the  sub-rule probabilities is evaluated to [{totalProbability}]. This value higher than 100.");
                return null;
            }

            for (int i = 0; i < probabilities.Length; i++)
            {
                (double probability, RuleContainer rule) x = probabilities[i];

                if (Double.IsNaN(x.probability))
                {
                    probabilities[i] = ((100d - totalProbability) / undefinedProbabilityCount, x.rule);
                }
            }

            double probabilitySum = 0;
            double randomDouble = RandomNumberGenerator.GenerateDouble(0, 100);

            foreach ((double probability, RuleContainer rule) x in probabilities)
            {
                probabilitySum += x.probability;
                if (randomDouble <= probabilitySum)
                {
                    return x.rule;
                }
            }

            return null;
        }
        public void Evaluate(ref List<(ProductionRule rule, Dictionary<string, double> ctx)> newIteration, in Dictionary<string, double> globalCtx, Dictionary<string, double> ctx)
        {
            if(isTerminal)
            {
                newIteration.Add((this, ctx));
            }

            RuleContainer? choosenRule = ProbabilityPick(globalCtx, ctx);

            if (!choosenRule.HasValue)
                return;

            if (choosenRule.Value.Condition != null && choosenRule.Value.Condition.Evaluate(globalCtx, ctx) == 0)
            {
                newIteration.Add((this, ctx));
                return;
            }

            foreach(RuleCall m in choosenRule.Value.Successors)
            {
                m.Evaluate(ref newIteration, globalCtx, ctx);
            }
        }
    }
}

