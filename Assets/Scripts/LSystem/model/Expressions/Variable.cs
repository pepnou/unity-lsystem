using System.Collections.Generic;
using System.Text;

using UnityEngine;

namespace LSystem.model
{
    public class Variable : Expression
    {
        public string Name { get; set; }
        public Vector2Int Position { get; set; }

        public Variable(string name, Vector2Int position)
        {
            Name = name;
            Position = position;
        }

        public Variable(string name)
        {
            Name = name;
        }

        public string Dump(string tab)
        {
            var dmp = new StringBuilder();
            dmp.AppendLine(tab + "Variable: " + Name);
            return dmp.ToString();
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            bool res = ctx.Contains(Name) || globalCtx.Contains(Name);
            if(!res)
            {
                Debug.LogError($"Variable \"{Name}\" line: {Position.x}, column: {Position.y} isn't defined");
            }
            return res;
        }

        public double Evaluate(in Dictionary<string, double> globalCtx, in Dictionary<string, double> ctx)
        {
            if(ctx.ContainsKey(Name))
                return ctx[Name];
            return globalCtx[Name];
        }
    }
}

