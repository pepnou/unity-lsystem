//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from .\lsystem.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="lsystemParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.CLSCompliant(false)]
public interface IlsystemVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.lSystem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLSystem([NotNull] lsystemParser.LSystemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.assignement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignement([NotNull] lsystemParser.AssignementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.productionRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProductionRule([NotNull] lsystemParser.ProductionRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.module"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitModule([NotNull] lsystemParser.ModuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.moduleLoop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitModuleLoop([NotNull] lsystemParser.ModuleLoopContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] lsystemParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.range"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRange([NotNull] lsystemParser.RangeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.simpleExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSimpleExpression([NotNull] lsystemParser.SimpleExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="lsystemParser.simpleRange"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSimpleRange([NotNull] lsystemParser.SimpleRangeContext context);
}
