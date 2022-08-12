using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;

using LSystem.model;
using LSystem.parser;

public class LSystemHandler
{
    lsystemParser parser;
    IParseTree tree;

    ASTGenerationVisitor astGenerationVisitor;
    LSystemContainer ast;

    List<(ProductionRule rule, Dictionary<string, double> ctx)> Iteration = null;
    Dictionary<string, double> globalCtx;

    public LSystemHandler(string input)
    {
        ITokenSource lexer = new lsystemLexer(new AntlrInputStream(input));
        ITokenStream tokens = new CommonTokenStream(lexer);
        parser = new lsystemParser(tokens);

        parser.AddErrorListener(new ThrowingErrorListener());
        parser.BuildParseTree = true;
            
        astGenerationVisitor = new ASTGenerationVisitor();
    }
    public void Parse()
    {
        try
        {
            tree = parser.lSystem();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            //Debug.LogError(ex.StackTrace);
        }
    }

    public void GenerateAST()
    {
        ast = astGenerationVisitor.Visit(tree) as LSystemContainer;
    }

    public void DumpAST()
    {
        if (ast == null)
        {
            Debug.Log("null");
        }
        else
        {
            Debug.Log(ast.Dump(""));
        }
    }

    public void VariableCheck()
    {
        ast.CheckContext(null, null);
        ast.CollapseRuleByProbability();
        ast.MatchRuleDefinition(null);
        ast.CheckRuleParametters();
    }

    public void BuildGlobalCtx()
    {
        ast.EvaluateGlobalCtx(ref globalCtx);
    }

    public void Iterate()
    {
        ast.Evaluate(ref Iteration, globalCtx);
    }

    public void Clear()
    {
        Iteration = null;
    }

    public string IterationToString()
    {
        StringBuilder str = new StringBuilder();
        foreach((ProductionRule rule, Dictionary<string, double> ctx) x in Iteration)
        {
            str.Append(x.rule.Name);
        }
        return str.ToString();
    }

    public void Draw(string drawer)
    {
        var drawerType = Type.GetType(drawer);
        var drawerInstance = Activator.CreateInstance(drawerType);
        if (drawerInstance == null)
        {
            Debug.LogError($"Class [{drawer}] does not exist.");
        }

        foreach((ProductionRule rule, Dictionary<string, double> ctx) x in Iteration)
        {
            var drawMethod = drawerType.GetMethod($"Draw_{x.rule.Name}");

            if (drawMethod == null)
                continue;

            drawMethod.Invoke(drawerInstance, new[] { x.ctx, globalCtx });
        }
    }

    public void Draw(string drawer, object[] args)
    {
        var drawerType = Type.GetType(drawer);
        var drawerInstance = Activator.CreateInstance(drawerType, args);
        if (drawerInstance == null)
        {
            Debug.LogError($"Class [{drawer}] does not exist.");
        }

        foreach ((ProductionRule rule, Dictionary<string, double> ctx) x in Iteration)
        {
            var drawMethod = drawerType.GetMethod($"Draw_{x.rule.Name}");

            if (drawMethod == null)
                continue;

            drawMethod.Invoke(drawerInstance, new[] { x.ctx, globalCtx });
        }
    }
    public class ThrowingErrorListener : BaseErrorListener
    {
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new ParseCanceledException("line " + line + ":" + charPositionInLine + " " + msg);
        }
    }
}
