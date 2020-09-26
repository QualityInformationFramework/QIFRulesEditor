using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Re.Grammar;

namespace Re.Completion
{
    /// <summary>
    /// Provides methods for checking expressions for completeness
    /// 
    /// TODO: There is some duplicated code
    /// get it off and consider Visitor usage
    /// </summary>
    static class Completeness
    {
        public static bool IsComplete(rulesParser.If_statementContext ctx)
        {
            var expr = ctx.boolean_expr();
            if (IsNullOrEmpty(expr) || expr.GetType() == typeof(rulesParser.Boolean_exprContext))
                return false;

            return IsComplete((dynamic)expr);
        }

        private static bool IsComplete(rulesParser.Boolean_expr_notContext ctx)
        {
            if (IsNullOrEmpty(ctx.boolean_expr()))
                return false;

            return IsComplete((dynamic)ctx.boolean_expr());
        }

        private static bool IsComplete(rulesParser.Boolean_expr_tokenContext ctx)
        {
            if (ctx.token_var().Length < 2 || IsNullOrEmpty(ctx.token_var(0)) || IsNullOrEmpty(ctx.token_var(0)))
                return false;

            return IsComplete(ctx.token_var(0)) && IsComplete(ctx.token_var(1));
        }

        private static bool IsComplete(rulesParser.Boolean_expr_andContext ctx)
        {
            if (ctx.boolean_expr().Length < 2 || IsNullOrEmpty(ctx.boolean_expr(0)) || IsNullOrEmpty(ctx.boolean_expr(0)))
                return false;

            return IsComplete((dynamic)ctx.boolean_expr(0)) && IsComplete((dynamic)ctx.boolean_expr(1));
        }

        private static bool IsComplete(rulesParser.Boolean_expr_orContext ctx)
        {
            if (ctx.boolean_expr().Length < 2 || IsNullOrEmpty(ctx.boolean_expr(0)) || IsNullOrEmpty(ctx.boolean_expr(0)))
                return false;

            // check boolean expressions if they are valid
            if (ctx.boolean_expr(0).GetType() == typeof(rulesParser.Boolean_exprContext) && ctx.boolean_expr(1).GetType() == typeof(rulesParser.Boolean_exprContext))
                return IsComplete((dynamic)ctx.boolean_expr(0)) && IsComplete((dynamic)ctx.boolean_expr(1));

            return false;
        }

        private static bool IsComplete(rulesParser.Boolean_expr_arithmetic_comparisonContext ctx)
        {
            if (ctx.arithmetic_expr().Length < 2
                || IsNullOrEmpty(ctx.arithmetic_comp_literal())
                || IsNullOrEmpty(ctx.arithmetic_expr(0))
                || IsNullOrEmpty(ctx.arithmetic_expr(1)))
                return false;

            return IsComplete((dynamic)ctx.arithmetic_expr(0)) && IsComplete((dynamic)ctx.arithmetic_expr(1));
        }

        private static bool IsComplete(rulesParser.Boolean_expr_inContext ctx)
        {
            if (IsNullOrEmpty(ctx.arithmetic_expr()) || IsNullOrEmpty(ctx.interval_fn()) || ctx.IN_KEYWORD() == null)
                return false;

            return IsComplete(ctx.interval_fn());
        }

        private static bool IsComplete(rulesParser.Interval_fnContext ctx)
        {
            var interval = ctx.closed_interval();
            return !IsNullOrEmpty(interval) && IsComplete(interval);
        }

        private static bool IsComplete(rulesParser.Closed_intervalContext ctx)
        {
            var exprs = ctx.arithmetic_expr();
            var expr1 = exprs.FirstOrDefault();
            var expr2 = ctx.arithmetic_expr().Length > 1 ? exprs[1] : null;
            return ctx.CLOSED_INTERVAL_KEYWORD() != null
                && !IsNullOrEmpty(expr1) && IsComplete((dynamic)expr1)
                && !IsNullOrEmpty(expr2) && IsComplete((dynamic)expr2);
        }

        private static bool IsComplete(rulesParser.Boolean_expr_literalContext ctx)
        {
            return true;
        }

        private static bool IsComplete(rulesParser.Boolean_expr_fnContext ctx)
        {
            var fn = ctx.boolean_fn();
            return !IsNullOrEmpty(fn) && IsComplete(fn);
        }

        private static bool IsComplete(rulesParser.Boolean_fnContext ctx)
        {
            var characteristicIs = ctx.characteristic_is();
            if (!IsNullOrEmpty(characteristicIs) && IsComplete(characteristicIs))
                return true;

            var featureIsDatum = ctx.feature_is_datum();
            if (!IsNullOrEmpty(featureIsDatum) && IsComplete(featureIsDatum))
                return true;

            var featureIsInternal = ctx.feature_is_internal();
            if (!IsNullOrEmpty(featureIsInternal) && IsComplete(featureIsInternal))
                return true;

            var featureTypeIs = ctx.feature_type_is();
            if (!IsNullOrEmpty(featureTypeIs) && IsComplete(featureTypeIs))
                return true;

            var samplingCategoryIs = ctx.sampling_category_is();
            if (!IsNullOrEmpty(samplingCategoryIs) && IsComplete(samplingCategoryIs))
                return true;

            var shapeClassIs = ctx.shape_class_is();
            if (!IsNullOrEmpty(shapeClassIs) && IsComplete(shapeClassIs))
                return true;

            return false;
        }

        private static bool IsComplete(rulesParser.Shape_class_isContext ctx)
        {
            return ctx.SHAPE_CLASS_IS_KEYWORD() != null && ctx.ENUM_SHAPE_CLASS() != null;
        }

        private static bool IsComplete(rulesParser.Sampling_category_isContext ctx)
        {
            return ctx.SAMPLING_CATEGORY_IS_KEYWORD() != null && ctx.INTEGER() != null;
        }

        private static bool IsComplete(rulesParser.Feature_type_isContext ctx)
        {
            return ctx.FEATURE_TYPE_IS_KEYWORD() != null && ctx.ENUM_FEATURE() != null;
        }

        private static bool IsComplete(rulesParser.Feature_is_internalContext ctx)
        {
            return ctx.FEATURE_IS_INTERNAL_KEYWORD() != null;
        }

        private static bool IsComplete(rulesParser.Feature_is_datumContext ctx)
        {
            return ctx.FEATURE_IS_DATUM_KEYWORD() != null;
        }

        private static bool IsComplete(rulesParser.Characteristic_isContext ctx)
        {
            return ctx.CHARACTERISTIC_IS_KEYWORD() != null && ctx.ENUM_CHARACTERISTIC() != null;
        }

        private static bool IsComplete(rulesParser.Boolean_expr_bracesContext ctx)
        {
            var expr = ctx.boolean_expr();
            return !IsNullOrEmpty(expr) && expr.GetType() == typeof(rulesParser.Boolean_exprContext) && IsComplete((dynamic)expr);
        }

        private static bool IsComplete(rulesParser.Arithmetic_expr_mult_divContext ctx)
        {
            var exprs = ctx.arithmetic_expr();
            var expr1 = exprs.FirstOrDefault();
            var expr2 = exprs.Length > 1 ? exprs[1] : null;
            return !IsNullOrEmpty(expr1) && IsComplete((dynamic)expr1)
                && !IsNullOrEmpty(expr2) && IsComplete((dynamic)expr2);
        }

        private static bool IsComplete(rulesParser.Arithmetic_expr_plus_minusContext ctx)
        {
            var exprs = ctx.arithmetic_expr();
            var expr1 = exprs.FirstOrDefault();
            var expr2 = exprs.Length > 1 ? exprs[1] : null;
            return !IsNullOrEmpty(expr1) && IsComplete((dynamic)expr1)
                && !IsNullOrEmpty(expr2) && IsComplete((dynamic)expr2);
        }

        private static bool IsComplete(rulesParser.Arithmetic_expr_constContext ctx)
        {
            var c = ctx.arithmetic_const();
            return !IsNullOrEmpty(c) && IsComplete(c);
        }

        private static bool IsComplete(rulesParser.Arithmetic_expr_fnContext ctx)
        {
            var f = ctx.arithmetic_fn();
            return !IsNullOrEmpty(f) && IsComplete(f);
        }

        private static bool IsComplete(rulesParser.Arithmetic_expr_bracesContext ctx)
        {
            var e = ctx.arithmetic_expr();
            return !IsNullOrEmpty(e) && IsComplete((dynamic)e);
        }

        private static bool IsComplete(rulesParser.Arithmetic_fnContext f)
        {
            var v = f.variable();
            if (!IsNullOrEmpty(v) && IsComplete(v))
                return true;

            var fp = f.feature_parameter();
            if (!IsNullOrEmpty(fp) && IsComplete(fp))
                return true;

            var cp = f.characteristic_parameter();
            if (!IsNullOrEmpty(cp) && IsComplete(cp))
                return true;

            var dp = f.dme_parameter();
            if (!IsNullOrEmpty(dp) && IsComplete(dp))
                return true;

            var pp = f.part_parameter();
            if (!IsNullOrEmpty(pp) && IsComplete(pp))
                return true;

            var fl = f.feature_length();
            if (!IsNullOrEmpty(fl) && IsComplete(fl))
                return true;

            var fa = f.feature_area();
            if (!IsNullOrEmpty(fa) && IsComplete(fa))
                return true;

            var fs = f.feature_size();
            if (!IsNullOrEmpty(fs) && IsComplete(fs))
                return true;

            var fMax = f.max();
            if (!IsNullOrEmpty(fMax) && IsComplete(fMax))
                return true;

            var fMin = f.min();
            if (!IsNullOrEmpty(fMin) && IsComplete(fMin))
                return true;

            var ct = f.characteristic_tolerance();
            if (!IsNullOrEmpty(ct) && IsComplete(ct))
                return true;

            var op = f.object_parameter();
            if (!IsNullOrEmpty(op) && IsComplete(op))
                return true;

            return false;
        }

        private static bool IsComplete(rulesParser.Object_parameterContext ctx)
        {
            return ctx.OBJECT_PARAMETER_KEYWORD() != null && !IsNullOrEmpty(ctx.xpath());
        }

        private static bool IsComplete(rulesParser.Characteristic_toleranceContext ctx)
        {
            return ctx.CHARACTERISTIC_TOLERANCE_KEYWORD() != null;
        }

        private static bool IsComplete(rulesParser.MinContext ctx)
        {
            var expressions = ctx.arithmetic_expr();
            foreach (var e in expressions)
            {
                if (!IsNullOrEmpty(e) && !IsComplete((dynamic)e))
                    return false;
            }
            return true;
        }

        private static bool IsComplete(rulesParser.MaxContext ctx)
        {
            var expressions = ctx.arithmetic_expr();
            foreach (var e in expressions)
            {
                if (!IsNullOrEmpty(e) && !IsComplete((dynamic)e))
                    return false;
            }
            return true;
        }

        private static bool IsComplete(rulesParser.Feature_sizeContext ctx)
        {
            return ctx.FEATURE_SIZE_KEYWORD() != null;
        }

        private static bool IsComplete(rulesParser.Feature_areaContext ctx)
        {
            return ctx.FEATURE_AREA_KEYWORD() != null;
        }

        private static bool IsComplete(rulesParser.Feature_lengthContext ctx)
        {
            return ctx.FEATURE_LENGTH_KEYWORD() != null;
        }

        private static bool IsComplete(rulesParser.Part_parameterContext ctx)
        {
            return ctx.PART_PARAMETER_KEYWORD() != null && !IsNullOrEmpty(ctx.xpath());
        }

        private static bool IsComplete(rulesParser.Dme_parameterContext ctx)
        {
            return ctx.DME_PARAMETER_KEYWORD() != null && !IsNullOrEmpty(ctx.xpath());
        }

        private static bool IsComplete(rulesParser.Characteristic_parameterContext ctx)
        {
            return ctx.CHARACTERISTIC_PARAMETER_KEYWORD() != null
                && !IsNullOrEmpty(ctx.xpath())
                && ctx.ENUM_CHARACTERISTIC() != null;
        }

        private static bool IsComplete(rulesParser.Feature_parameterContext ctx)
        {
            return ctx.FEATURE_PARAMETER_KEYWORD() != null && !IsNullOrEmpty(ctx.xpath());
        }

        private static bool IsComplete(rulesParser.VariableContext ctx)
        {
            return ctx.VARIABLE_KEYWORD() != null && !IsNullOrEmpty(ctx.identifier());
        }

        private static bool IsComplete(rulesParser.Arithmetic_constContext c)
        {
            return c.INTEGER() != null || c.DOUBLE() != null;
        }

        private static bool IsComplete(rulesParser.Token_varContext ctx)
        {
            return (!IsNullOrEmpty(ctx.token_fn()) && IsComplete(ctx)) || ctx.STRING_LITERAL() != null;
        }

        public static bool IsComplete(rulesParser.Dme_then_statementContext ctx)
        {
            if (ctx.dme_then_decision_class() != null && IsComplete(ctx.dme_then_decision_class()))
                return true;

            if (ctx.dme_then_id() != null && IsComplete(ctx.dme_then_id()))
                return true;

            if (ctx.dme_then_make_model() != null && IsComplete(ctx.dme_then_make_model()))
                return true;

            return false;
        }

        private static bool IsComplete(rulesParser.Dme_then_make_modelContext ctx)
        {
            var a = ctx.dme_applicability();
            return !IsNullOrEmpty(a) && IsComplete(a)
                && ctx.MANUFACTURER_KEYWORD() != null && !IsNullOrEmpty(ctx.manufacturer())
                && ctx.MODEL_NUMBER_KEYWORD() != null && !IsNullOrEmpty(ctx.model_number());
        }

        private static bool IsComplete(rulesParser.Dme_then_idContext ctx)
        {
            var a = ctx.dme_applicability();
            return !IsNullOrEmpty(a) && IsComplete(a) && ctx.INTEGER() != null;
        }

        private static bool IsComplete(rulesParser.Dme_then_decision_classContext ctx)
        {
            var applicability = ctx.dme_applicability();
            if (applicability == null || !IsComplete(applicability) || ctx.ENUM_DME_CLASS() == null)
                return false;

            foreach (var with in ctx.with_statement())
            {
                if (!IsComplete(with))
                    return false;
            }

            return true;
        }

        private static bool IsComplete(rulesParser.Dme_applicabilityContext ctx)
        {
            return ctx.dme_applicability_may() != null || ctx.dme_applicability_must() != null || ctx.dme_applicability_must_not() != null;
        }

        public static bool IsComplete(rulesParser.With_statementContext ctx)
        {
            var arithmeticExpr = ctx.arithmetic_expr();
            bool hasAriphmetic = !IsNullOrEmpty(arithmeticExpr) && IsComplete((dynamic)arithmeticExpr);
            return !IsNullOrEmpty(ctx.xpath()) && (hasAriphmetic || !IsNullOrEmpty(ctx.interval_fn()));
        }

        public static bool IsNullOrEmpty(ParserRuleContext context)
        {
            return context == null || context.SourceInterval.Length == 0;
        }
    }
}
