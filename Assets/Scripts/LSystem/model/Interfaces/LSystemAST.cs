using System.Collections;
using System.Collections.Generic;

namespace LSystem.model
{
    public interface LSystemAST
    {
        public string Dump(string tab);
        public bool CheckContext(List<string> globalCtx, List<string> ctx);
    }
}

