using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using LSystem.model;

using UnityEngine;

namespace LSystem.parser
{
    public class ASTGenerationVisitor : lsystemBaseVisitor<LSystemAST>
    {
        public override LSystemAST VisitLSystem([NotNull] lsystemParser.LSystemContext context)
        {
            return new LSystemContainer(
                context.assignement().Select(Visit).Cast<Assignement>().ToList(),
                context.module().Select(Visit).Cast<RuleCall>().ToList(),
                context.productionRule().Select(Visit).Cast<ProductionRule>().ToList());
        }

        public override LSystemAST VisitAssignement([NotNull] lsystemParser.AssignementContext context) 
        {
            return new Assignement(
                new Variable(
                    context.VARIABLE().GetText(),
                    new Vector2Int(context.VARIABLE().Symbol.Line, context.VARIABLE().Symbol.Column)),
                Visit(context.simpleExpression()) as Expression);
        }

        public override LSystemAST VisitProductionRule([NotNull] lsystemParser.ProductionRuleContext context) 
        {
            var ruleNameCtx = context.RULENAME();
            string rulename =ruleNameCtx .GetText();

            List<Variable> parametters = null;
            var paramettersCtx = context.VARIABLE();
            if(paramettersCtx != null)
            {
                parametters = paramettersCtx.Select(x => new Variable(x.GetText(), new Vector2Int(x.Symbol.Line, x.Symbol.Column))).ToList();
            }

            Expression probability = null;
            var probabilityCtx = context.probability_;
            if(probabilityCtx != null)
            {
                probability = Visit(probabilityCtx) as Expression;
            }

            List<RuleCall> predecessors = null;
            var predecessorsCtx = context._predecessors_;
            if(predecessorsCtx != null)
            {
                predecessors = predecessorsCtx.Select(Visit).Cast<RuleCall>().ToList();
            }

            Expression condition = null;
            var conditionCtx = context.condition_;
            if (conditionCtx != null)
            {
                condition = Visit(conditionCtx) as Expression;
            }

            List<RuleCall> successors = null;
            var successorsCtx = context._successors_;
            if (successorsCtx != null)
            {
                successors = successorsCtx.Select(Visit).Cast<RuleCall>().ToList();
            }

            return new ProductionRule(rulename, new Vector2Int(ruleNameCtx.Symbol.Line, ruleNameCtx.Symbol.Column), parametters, probability, predecessors, condition, successors);
        }

        public override LSystemAST VisitModule([NotNull] lsystemParser.ModuleContext context)
        {
            var ruleNameCtx = context.RULENAME();
            if (ruleNameCtx != null)
            {
                string rulename = ruleNameCtx.GetText();

                List<Expression> parametters = null;
                var expressionCtx = context.expression();
                if (expressionCtx != null)
                {
                    parametters = expressionCtx.Select(Visit).Cast<Expression>().ToList();
                }

                return new Module(rulename, new Vector2Int(ruleNameCtx.Symbol.Line, ruleNameCtx.Symbol.Column), parametters);
            } else
            {
                return Visit(context.moduleLoop());
            }
        }

        public override LSystemAST VisitModuleLoop([NotNull] lsystemParser.ModuleLoopContext context) 
        {
            return new ModuleLoop(
                context.module().Select(Visit).Cast<RuleCall>().ToList(),
                Visit(context.expression()) as Expression,
                new Vector2Int(context.Start.Line, context.Start.Column));
        }

        public override LSystemAST VisitExpression([NotNull] lsystemParser.ExpressionContext context)
        {
            var varCtx = context.VARIABLE();
            var floatCtx = context.FLOAT();
            var paranCtx = context.expr_;
            var unopCtx = context.unop_;
            var binopCtx = context.binop_;
            var leftCtx = context.left_;
            var rightCtx = context.right_;

            if(varCtx != null)
            {
                return new Variable(varCtx.GetText(), new Vector2Int(varCtx.Symbol.Line, varCtx.Symbol.Column));
            } else if(floatCtx != null)
            {
                return new FloatConstant(floatCtx.GetText());
            } else if(paranCtx != null)
            {
                return Visit(paranCtx);
            } else if(unopCtx != null)
            {
                return new UnaryOperation(unopCtx.Text, Visit(rightCtx) as Expression);
            } else
            {
                return new BinaryOperation(Visit(leftCtx) as Expression, binopCtx.Text, Visit(rightCtx) as Expression);
            }
        }

        public override LSystemAST VisitRange([NotNull] lsystemParser.RangeContext context)
        {
            Expression from = Visit(context.from_) as Expression;
            Expression to = Visit(context.to_) as Expression;

            return new Range(from, to);
        }

        public override LSystemAST VisitSimpleExpression([NotNull] lsystemParser.SimpleExpressionContext context) 
        {
            var floatCtx = context.FLOAT();
            var paranCtx = context.expr_;
            var unopCtx = context.unop_;
            var binopCtx = context.binop_;
            var leftCtx = context.left_;
            var rightCtx = context.right_;

            if (floatCtx != null)
            {
                return new FloatConstant(floatCtx.GetText());
            }
            else if (paranCtx != null)
            {
                return Visit(paranCtx);
            }
            else if (unopCtx != null)
            {
                return new UnaryOperation(unopCtx.Text, Visit(rightCtx) as Expression);
            }
            else
            {
                return new BinaryOperation(Visit(leftCtx) as Expression, binopCtx.Text, Visit(rightCtx) as Expression);
            }
        }
        
        public override LSystemAST VisitSimpleRange([NotNull] lsystemParser.SimpleRangeContext context)
        {
            Expression from = Visit(context.from_) as Expression;
            Expression to = Visit(context.to_) as Expression;

            return new Range(from, to);
        }
    }
}