using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/*using LSystem.model;
using LSystem.parser;
using sly.parser;
using sly.parser.generator;
using sly.lexer;
using sly.lexer.fsm;



using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;*/
using Antlr4.Runtime;
//using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;

using System.IO;

public class ThrowingErrorListener : BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        throw new ParseCanceledException("line " + line + ":" + charPositionInLine + " " + msg);
    }
}
public class LSystemTest : MonoBehaviour
{
    [SerializeField] [TextArea(10,20)] string lsystemRules;
    [SerializeField] string drawer;

    [SerializeField] List<Transform> transforms;

    // [SerializeField] string startRule;
    // 
    // static void AddExtention(LSystemToken token, LexemeAttribute lexem, GenericLexer<LSystemToken> lexer)
    // {
    //     if (token == LSystemToken.VARIABLE_)
    //     {
    //         Debug.Log("VARIABLE_");
    //         TransitionPrecondition tmp1 = (ReadOnlyMemory<char> value) =>
    //         {
    //             bool ok = char.IsLower(value.At(0)) || char.IsDigit(value.At(0)) || value.At(0) == '_';
    //             return ok;
    //         };
    // 
    //         TransitionPrecondition tmp2 = (ReadOnlyMemory<char> value) =>
    //         {
    //             bool ok = value.Length == 0 || !(char.IsLower(value.At(1)) || char.IsDigit(value.At(1)) || value.At(1) == '_');
    //             return ok;
    //         };
    // 
    // 
    //         // callback on end_date node 
    //         NodeCallback<GenericToken> callback = (FSMMatch<GenericToken> match) =>
    //         {
    //             // this store the token id the the FSMMatch object to be later returned by GenericLexer.Tokenize 
    //             match.Properties[GenericLexer<LSystemToken>.DerivedToken] = LSystemToken.VARIABLE_;
    //             return match;
    //         };
    // 
    //         var fsmBuilder = lexer.FSMBuilder;
    // 
    //         fsmBuilder.GoTo(GenericLexer<LSystemToken>.start)
    //             .RangeTransition('a', 'z')
    //             .Mark("v1")
    //             .AnyTransitionTo("v1",tmp1)
    //             .AnyTransition(tmp2)
    //             .Mark("v2")
    //             .End(GenericToken.Extension) // mark as ending node 
    //             .CallBack(callback); // set the ending callback
    //     } 
    //     else if(token == LSystemToken.RULENAME_)
    //     {
    //         Debug.Log("RULENAME_");
    //         TransitionPrecondition tmp1 = (ReadOnlyMemory<char> value) =>
    //         {
    //             Debug.Log("tmp1: " + value.ToString());
    //             bool ok = char.IsLower(value.At(0)) || char.IsDigit(value.At(0)) || value.At(0) == '_';
    //             return ok;
    //         };
    // 
    //         TransitionPrecondition tmp2 = (ReadOnlyMemory<char> value) =>
    //         {
    //             Debug.Log("tmp2: " + value.ToString());
    //             bool ok = value.Length == 0 || !(char.IsLower(value.At(1)) || char.IsDigit(value.At(1)) || value.At(1) == '_');
    //             return ok;
    //         };
    // 
    // 
    //         // callback on end_date node 
    //         NodeCallback<GenericToken> callback = (FSMMatch<GenericToken> match) =>
    //         {
    //             // this store the token id the the FSMMatch object to be later returned by GenericLexer.Tokenize 
    //             match.Properties[GenericLexer<LSystemToken>.DerivedToken] = LSystemToken.RULENAME_;
    //             return match;
    //         };
    // 
    //         var fsmBuilder = lexer.FSMBuilder;
    // 
    //         fsmBuilder.GoTo(GenericLexer<LSystemToken>.start)
    //             .Transition('A')
    //             //.RangeTransition('A', 'Z')
    //             .Mark("r1")
    //             /*.AnyTransitionTo("r1",tmp1)
    //             .AnyTransition(tmp2)
    //             .Mark("r2")*/
    //             .End(GenericToken.Extension) // mark as ending node 
    //             .CallBack(callback); // set the ending callback
    //     }
    // }
    // 
    // public void Parse()
    // {
    //     BuildExtension<LSystemToken> extensionBuilder = (LSystemToken token, LexemeAttribute lexem, GenericLexer<LSystemToken> lexer) =>
    //  {
    //      /*if (token == LSystemToken.VARIABLE_)
    //      {
    //          Debug.Log("VARIABLE_");
    //          TransitionPrecondition tmp1 = (ReadOnlyMemory<char> value) =>
    //          {
    //              bool ok = char.IsLower(value.At(0)) || char.IsDigit(value.At(0)) || value.At(0) == '_';
    //              return ok;
    //          };
    // 
    //          TransitionPrecondition tmp2 = (ReadOnlyMemory<char> value) =>
    //          {
    //              bool ok = value.Length == 0 || !(char.IsLower(value.At(1)) || char.IsDigit(value.At(1)) || value.At(1) == '_');
    //              return ok;
    //          };
    // 
    // 
    //          // callback on end_date node 
    //          NodeCallback<GenericToken> callback = (FSMMatch<GenericToken> match) =>
    //          {
    //              // this store the token id the the FSMMatch object to be later returned by GenericLexer.Tokenize 
    //              match.Properties[GenericLexer<LSystemToken>.DerivedToken] = LSystemToken.VARIABLE_;
    //              return match;
    //          };
    // 
    //          var fsmBuilder = lexer.FSMBuilder;
    // 
    //          fsmBuilder.GoTo(GenericLexer<LSystemToken>.start)
    //              .RangeTransition('a', 'z')
    //              .Mark("v1")
    //              .AnyTransitionTo("v1", tmp1)
    //              .AnyTransition(tmp2)
    //              .Mark("v2")
    //              .End(GenericToken.Extension) // mark as ending node 
    //              .CallBack(callback); // set the ending callback
    //      }
    //      else */if (token == LSystemToken.RULENAME_)
    //      {
    //          Debug.Log("RULENAME_");
    //          TransitionPrecondition tmp1 = (ReadOnlyMemory<char> value) =>
    //          {
    //              Debug.Log("tmp1: " + value.ToString());
    //              bool ok = char.IsLower(value.At(0)) || char.IsDigit(value.At(0)) || value.At(0) == '_';
    //              return ok;
    //          };
    // 
    //          TransitionPrecondition tmp2 = (ReadOnlyMemory<char> value) =>
    //          {
    //              Debug.Log("tmp2: " + value.ToString());
    //              bool ok = value.Length == 0 || !(char.IsLower(value.At(1)) || char.IsDigit(value.At(1)) || value.At(1) == '_');
    //              return ok;
    //          };
    // 
    //          TransitionPrecondition tmp3 = (ReadOnlyMemory<char> value) =>
    //          {
    //              return true;
    //              Debug.Log("tmp2: " + value.ToString());
    //              bool ok = char.IsUpper(value.At(0));
    //              return ok;
    //          };
    // 
    // 
    //          // callback on end_date node 
    //          NodeCallback<GenericToken> callback = (FSMMatch<GenericToken> match) =>
    //          {
    //              // this store the token id the the FSMMatch object to be later returned by GenericLexer.Tokenize 
    //              match.Properties[GenericLexer<LSystemToken>.DerivedToken] = LSystemToken.RULENAME_;
    //              return match;
    //          };
    // 
    //          var fsmBuilder = lexer.FSMBuilder;
    // 
    //          fsmBuilder.GoTo(GenericLexer<LSystemToken>.start)
    //              //.Transition('A')
    //              .Transition(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' })
    //              //.AnyTransition(tmp3)
    //              //.RangeTransition('A', 'Z')
    //              .Mark("r1")
    //              /*.AnyTransitionTo("r1",tmp1)
    //              .AnyTransition(tmp2)
    //              .Mark("r2")*/
    //              .End(GenericToken.Extension) // mark as ending node 
    //              .CallBack(callback); // set the ending callback
    //      }
    //  };
    // 
    // 
    // 
    // 
    // 
    //     var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //     var type = assembly.GetType("UnityEditor.LogEntries");
    //     var method = type.GetMethod("Clear");
    //     method.Invoke(new object(), null);
    // 
    //     var parserInstance = new LSystemParser();
    //     var builder = new ParserBuilder<LSystemToken, LSystemAST>();
    //     //var parserResult = builder.BuildParser(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT, startRule, AddExtention);
    //     var parserResult = builder.BuildParser(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT, startRule, extensionBuilder);
    // 
    //     if (!parserResult.IsOk || parserResult.IsError || parserResult.Errors != null)
    //     {
    //         bool ret = false;
    //         foreach (var error in parserResult.Errors)
    //         {
    //             Debug.LogError($"{error.Code} : {error.Message}");
    //             ret |= (error.Code != sly.buildresult.ErrorCodes.NOT_AN_ERROR);
    //         }
    //         if (ret)
    //         {
    //             return;
    //         }
    //     }
    // 
    //     Debug.Log("Parser Built");
    // 
    //     var parser = parserResult.Result;
    //     var r = parser.Parse(lsystemRules);
    // 
    //     Debug.Log("Text Parsed");
    // 
    //     if (r.IsError)
    //     {
    //         if (r.Errors != null)
    //         {
    //             r.Errors.ForEach(error => Debug.LogError(error.ErrorMessage));
    //         }
    //         return;
    //     }
    // 
    //     Debug.Log(r.Result.Dump(""));
    // }

    LSystemHandler lsystem = null;
    public void Parse()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);


        lsystem = new LSystemHandler(lsystemRules);
        lsystem.Parse();
        lsystem.GenerateAST();
        lsystem.VariableCheck();
        lsystem.BuildGlobalCtx();
        lsystem.DumpAST();
    }

    public void Iterate()
    {
        lsystem.Iterate();
        Debug.Log(lsystem.IterationToString());
    }

    public void Clear()
    {
        lsystem.Clear();
    }

    public void Draw()
    {
        lsystem.Draw(drawer, transforms.ToArray());
    }
}
