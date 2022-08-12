using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSystem.model
{
    public class Assignement : LSystemAST
    {
        public Variable Var { get; set; }
        public Expression Value { get; set; }

        public Assignement(Variable var, Expression value)
        {
            Var = var;
            Value = value;
        }

        public bool CheckContext(List<string> globalCtx, List<string> ctx)
        {
            return true;
        }

        public string Dump(string tab)
        {
            StringBuilder dmp = new StringBuilder();

            dmp.AppendLine(tab + "ASSIGNEMENT:");
            dmp.AppendLine(Var.Dump(tab + "\t"));
            dmp.AppendLine(tab + "\tVALUE:");
            dmp.AppendLine(Value.Dump(tab + "\t\t"));

            return dmp.ToString();
        }
    }
}