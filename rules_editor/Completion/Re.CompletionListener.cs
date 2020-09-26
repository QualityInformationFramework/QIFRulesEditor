﻿using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Re.Grammar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    /// <summary> Listens 'exit' events and tries to find continuation taking context into account. </summary>
    /// <seealso cref="Re.Grammar.IrulesListener" />
    public class CompletionListener : IrulesListener
    {
        public CompletionListener(IList<IToken> tokens)
        {
            Tokens = tokens;
        }

        public StreamWriter Logger { get; set; }

        public IEnumerable<string> Suggestions => Collected.Suggestions;

        #region Entries
        public void EnterArithmetic_comp_literal([NotNull] rulesParser.Arithmetic_comp_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_comp_literal_equal([NotNull] rulesParser.Arithmetic_comp_literal_equalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_comp_literal_greater([NotNull] rulesParser.Arithmetic_comp_literal_greaterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_comp_literal_greater_or_equal([NotNull] rulesParser.Arithmetic_comp_literal_greater_or_equalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_comp_literal_less([NotNull] rulesParser.Arithmetic_comp_literal_lessContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_comp_literal_less_or_equal([NotNull] rulesParser.Arithmetic_comp_literal_less_or_equalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_const([NotNull] rulesParser.Arithmetic_constContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_div_literal([NotNull] rulesParser.Arithmetic_div_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_expr([NotNull] rulesParser.Arithmetic_exprContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_expr_braces([NotNull] rulesParser.Arithmetic_expr_bracesContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_expr_const([NotNull] rulesParser.Arithmetic_expr_constContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_expr_fn([NotNull] rulesParser.Arithmetic_expr_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_expr_mult_div([NotNull] rulesParser.Arithmetic_expr_mult_divContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_expr_negate([NotNull] rulesParser.Arithmetic_expr_negateContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_expr_plus_minus([NotNull] rulesParser.Arithmetic_expr_plus_minusContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_fn([NotNull] rulesParser.Arithmetic_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_minus_literal([NotNull] rulesParser.Arithmetic_minus_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_mult_div_literal([NotNull] rulesParser.Arithmetic_mult_div_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_mult_literal([NotNull] rulesParser.Arithmetic_mult_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_plus_literal([NotNull] rulesParser.Arithmetic_plus_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterArithmetic_plus_minus_literal([NotNull] rulesParser.Arithmetic_plus_minus_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr([NotNull] rulesParser.Boolean_exprContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr_and([NotNull] rulesParser.Boolean_expr_andContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (IsOnEnd(context) && !IsNullOrEmpty(context.boolean_expr(0)) && IsNullOrEmpty(context.boolean_expr(1)))
                SuggestBoolean();
        }

        public void EnterBoolean_expr_arithmetic_comparison([NotNull] rulesParser.Boolean_expr_arithmetic_comparisonContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        }

        public void EnterBoolean_expr_braces([NotNull] rulesParser.Boolean_expr_bracesContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr_fn([NotNull] rulesParser.Boolean_expr_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr_in([NotNull] rulesParser.Boolean_expr_inContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr_literal([NotNull] rulesParser.Boolean_expr_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr_not([NotNull] rulesParser.Boolean_expr_notContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr_or([NotNull] rulesParser.Boolean_expr_orContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_expr_token([NotNull] rulesParser.Boolean_expr_tokenContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_fn([NotNull] rulesParser.Boolean_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterBoolean_literal([NotNull] rulesParser.Boolean_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterCharacteristic_is([NotNull] rulesParser.Characteristic_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterCharacteristic_parameter([NotNull] rulesParser.Characteristic_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterCharacteristic_tolerance([NotNull] rulesParser.Characteristic_toleranceContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterClosed_interval([NotNull] rulesParser.Closed_intervalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_applicability([NotNull] rulesParser.Dme_applicabilityContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_applicability_may([NotNull] rulesParser.Dme_applicability_mayContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_applicability_must([NotNull] rulesParser.Dme_applicability_mustContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_applicability_must_not([NotNull] rulesParser.Dme_applicability_must_notContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_parameter([NotNull] rulesParser.Dme_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_rule([NotNull] rulesParser.Dme_ruleContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_rules([NotNull] rulesParser.Dme_rulesContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_then_decision_class([NotNull] rulesParser.Dme_then_decision_classContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_then_id([NotNull] rulesParser.Dme_then_idContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_then_make_model([NotNull] rulesParser.Dme_then_make_modelContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterDme_then_statement([NotNull] rulesParser.Dme_then_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterEveryRule([NotNull] ParserRuleContext ctx)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterFeature_area([NotNull] rulesParser.Feature_areaContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterFeature_is_datum([NotNull] rulesParser.Feature_is_datumContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterFeature_is_internal([NotNull] rulesParser.Feature_is_internalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterFeature_length([NotNull] rulesParser.Feature_lengthContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterFeature_parameter([NotNull] rulesParser.Feature_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterFeature_size([NotNull] rulesParser.Feature_sizeContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterFeature_type_is([NotNull] rulesParser.Feature_type_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterIdentifier([NotNull] rulesParser.IdentifierContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterIf_statement([NotNull] rulesParser.If_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterInterval_fn([NotNull] rulesParser.Interval_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterManufacturer([NotNull] rulesParser.ManufacturerContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterMax([NotNull] rulesParser.MaxContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterMin([NotNull] rulesParser.MinContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterModel_number([NotNull] rulesParser.Model_numberContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterObject_parameter([NotNull] rulesParser.Object_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterPart_parameter([NotNull] rulesParser.Part_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterSampling_category_is([NotNull] rulesParser.Sampling_category_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterSerial_number([NotNull] rulesParser.Serial_numberContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterShape_class_is([NotNull] rulesParser.Shape_class_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterToken_fn([NotNull] rulesParser.Token_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterToken_var([NotNull] rulesParser.Token_varContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterUuid_statement([NotNull] rulesParser.Uuid_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterVariable([NotNull] rulesParser.VariableContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterWith_statement([NotNull] rulesParser.With_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterXpath([NotNull] rulesParser.XpathContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void EnterXpath_component([NotNull] rulesParser.Xpath_componentContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        #endregion

        public void ExitArithmetic_comp_literal([NotNull] rulesParser.Arithmetic_comp_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_comp_literal_equal([NotNull] rulesParser.Arithmetic_comp_literal_equalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_comp_literal_greater([NotNull] rulesParser.Arithmetic_comp_literal_greaterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_comp_literal_greater_or_equal([NotNull] rulesParser.Arithmetic_comp_literal_greater_or_equalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_comp_literal_less([NotNull] rulesParser.Arithmetic_comp_literal_lessContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_comp_literal_less_or_equal([NotNull] rulesParser.Arithmetic_comp_literal_less_or_equalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_const([NotNull] rulesParser.Arithmetic_constContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_div_literal([NotNull] rulesParser.Arithmetic_div_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_expr([NotNull] rulesParser.Arithmetic_exprContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_expr_braces([NotNull] rulesParser.Arithmetic_expr_bracesContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_expr_const([NotNull] rulesParser.Arithmetic_expr_constContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_expr_fn([NotNull] rulesParser.Arithmetic_expr_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_expr_mult_div([NotNull] rulesParser.Arithmetic_expr_mult_divContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_expr_negate([NotNull] rulesParser.Arithmetic_expr_negateContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_expr_plus_minus([NotNull] rulesParser.Arithmetic_expr_plus_minusContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_fn([NotNull] rulesParser.Arithmetic_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_minus_literal([NotNull] rulesParser.Arithmetic_minus_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_mult_div_literal([NotNull] rulesParser.Arithmetic_mult_div_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_mult_literal([NotNull] rulesParser.Arithmetic_mult_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_plus_literal([NotNull] rulesParser.Arithmetic_plus_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitArithmetic_plus_minus_literal([NotNull] rulesParser.Arithmetic_plus_minus_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr([NotNull] rulesParser.Boolean_exprContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr_and([NotNull] rulesParser.Boolean_expr_andContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr_arithmetic_comparison([NotNull] rulesParser.Boolean_expr_arithmetic_comparisonContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            bool isOnEnd = IsOnEnd(context);
            if (isOnEnd && IsNullOrEmpty(context.arithmetic_expr(1)))
                Suggest(KeywordSets.ArithmeticFunctions);
            else if (isOnEnd)
            {
                Suggest(rulesLexer.AND_KEYWORD);
                Suggest(rulesLexer.OR_KEYWORD);
            }
        }

        public void ExitBoolean_expr_braces([NotNull] rulesParser.Boolean_expr_bracesContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr_fn([NotNull] rulesParser.Boolean_expr_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr_in([NotNull] rulesParser.Boolean_expr_inContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr_literal([NotNull] rulesParser.Boolean_expr_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (IsOnEnd(context))
                Suggest(KeywordSets.BooleanOperators);
        }

        public void ExitBoolean_expr_not([NotNull] rulesParser.Boolean_expr_notContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr_or([NotNull] rulesParser.Boolean_expr_orContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_expr_token([NotNull] rulesParser.Boolean_expr_tokenContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_fn([NotNull] rulesParser.Boolean_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitBoolean_literal([NotNull] rulesParser.Boolean_literalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitCharacteristic_is([NotNull] rulesParser.Characteristic_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (IsOnEnd(context) && context.ENUM_CHARACTERISTIC() == null)
                Suggest(ConstantSets.CharacteristicTypes.Constants.Select(c => c.Name));
        }

        public void ExitCharacteristic_parameter([NotNull] rulesParser.Characteristic_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitCharacteristic_tolerance([NotNull] rulesParser.Characteristic_toleranceContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitClosed_interval([NotNull] rulesParser.Closed_intervalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitDme_applicability([NotNull] rulesParser.Dme_applicabilityContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (IsOnEnd(context) && IsNullOrEmpty(context))
            {
                Suggest(rulesLexer.MAY_KEYWORD);
                Suggest(rulesLexer.MUST_KEYWORD);
            }
        }

        public void ExitDme_applicability_may([NotNull] rulesParser.Dme_applicability_mayContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitDme_applicability_must([NotNull] rulesParser.Dme_applicability_mustContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitDme_applicability_must_not([NotNull] rulesParser.Dme_applicability_must_notContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitDme_parameter([NotNull] rulesParser.Dme_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitDme_rule([NotNull] rulesParser.Dme_ruleContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (!IsOnEnd(context))
                return;

            var lastThen = context.dme_then_statement().Length > 0 ? context.dme_then_statement(context.dme_then_statement().Length - 1) : null;
            if (IsNullOrEmpty(lastThen))
            {
                var ifStatement = context.if_statement();
                if (ifStatement == null && context.uuid_statement() == null)
                    Suggest(rulesLexer.UUID_KEYWORD);
                if (ifStatement == null)
                    Suggest(rulesLexer.IF_KEYWORD);

                if (ifStatement == null || Completeness.IsComplete(ifStatement))
                    Suggest(KeywordSets.ThenBlocks);

                if(!IsNullOrEmpty(ifStatement) && !Completeness.IsComplete(ifStatement))
                {
                    // sometimes this approach for completion does not work for nested arithmetic expressions
                    // suggest everything possible inside 'if' block to solve that 
                    SuggestBoolean();
                }
            }
            else if (Completeness.IsComplete(lastThen))
                Suggest(KeywordSets.ThenBlocks);
        }

        public void ExitDme_rules([NotNull] rulesParser.Dme_rulesContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (context.dme_rule().Length == 0)
                Suggest(rulesLexer.DME_RULE_KEYWORD);
        }

        public void ExitDme_then_decision_class([NotNull] rulesParser.Dme_then_decision_classContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (!IsOnEnd(context))
                return;

            if (IsNullOrEmpty(context.dme_applicability()))
            {
                // dme_class 
                Suggest(rulesLexer.MAY_KEYWORD);
                Suggest(rulesLexer.MUST_KEYWORD);
            }
            else if (context.ENUM_DME_CLASS() == null)
            {
                // dme_class may
                // dme_class must
                Suggest(ConstantSets.DmeClassName.Constants.Select(c => c.Name));
            }
            else
            {
                // dme_class may ENUM_VALUE
                // dme_class may ENUM value with (...)
                var lastWith = context.with_statement().Length > 0 ? context.with_statement(context.with_statement().Length - 1) : null;
                if (lastWith == null || Completeness.IsComplete(lastWith))
                    Suggest(rulesLexer.WITH_KEYWORD);
            }

        }

        public void ExitDme_then_id([NotNull] rulesParser.Dme_then_idContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitDme_then_make_model([NotNull] rulesParser.Dme_then_make_modelContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (!IsOnEnd(context))
                return;

            if (!IsNullOrEmpty(context.dme_applicability()))
            {
                if (!IsNullOrEmpty(context.manufacturer()))
                {
                    if (IsNullOrEmpty(context.model_number()))
                        Suggest(rulesLexer.MODEL_NUMBER_KEYWORD);
                    else if (IsNullOrEmpty(context.serial_number()))
                        Suggest(rulesLexer.SERIAL_NUMBER_KEYWORD);
                }
            }
        }

        public void ExitDme_then_statement([NotNull] rulesParser.Dme_then_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitEveryRule([NotNull] ParserRuleContext ctx)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitFeature_area([NotNull] rulesParser.Feature_areaContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitFeature_is_datum([NotNull] rulesParser.Feature_is_datumContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitFeature_is_internal([NotNull] rulesParser.Feature_is_internalContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitFeature_length([NotNull] rulesParser.Feature_lengthContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitFeature_parameter([NotNull] rulesParser.Feature_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitFeature_size([NotNull] rulesParser.Feature_sizeContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitFeature_type_is([NotNull] rulesParser.Feature_type_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (IsOnEnd(context) && context.ENUM_FEATURE() == null)
                Suggest(ConstantSets.FeatureTypes.Constants.Select(c => c.Name));
        }

        public void ExitIdentifier([NotNull] rulesParser.IdentifierContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitIf_statement([NotNull] rulesParser.If_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (IsOnEnd(context) && IsNullOrEmpty(context.boolean_expr()))
                SuggestBoolean();
        }

        public void ExitInterval_fn([NotNull] rulesParser.Interval_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if(IsOnEnd(context))
            {
                if (IsNullOrEmpty(context.closed_interval()))
                    Suggest(rulesLexer.CLOSED_INTERVAL_KEYWORD);
            }
        }

        public void ExitManufacturer([NotNull] rulesParser.ManufacturerContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitMax([NotNull] rulesParser.MaxContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitMin([NotNull] rulesParser.MinContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitModel_number([NotNull] rulesParser.Model_numberContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitObject_parameter([NotNull] rulesParser.Object_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitPart_parameter([NotNull] rulesParser.Part_parameterContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitSampling_category_is([NotNull] rulesParser.Sampling_category_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitSerial_number([NotNull] rulesParser.Serial_numberContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitShape_class_is([NotNull] rulesParser.Shape_class_isContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (IsOnEnd(context) && context.ENUM_SHAPE_CLASS() == null)
                Suggest(ConstantSets.ShapeClasses.Constants.Select(c => c.Name));
        }

        public void ExitToken_fn([NotNull] rulesParser.Token_fnContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitToken_var([NotNull] rulesParser.Token_varContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitUuid_statement([NotNull] rulesParser.Uuid_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitVariable([NotNull] rulesParser.VariableContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitWith_statement([NotNull] rulesParser.With_statementContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (!IsOnEnd(context))
                return;

            if (!IsNullOrEmpty(context.xpath()))
            {
                var compLiteral = context.arithmetic_comp_literal();
                if (!IsNullOrEmpty(compLiteral) && IsNullOrEmpty(context.arithmetic_expr()))
                    Suggest(KeywordSets.ArithmeticFunctions);
                if (IsNullOrEmpty(compLiteral))
                    Suggest(rulesLexer.IN_KEYWORD);
            }
        }

        public void ExitXpath([NotNull] rulesParser.XpathContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void ExitXpath_component([NotNull] rulesParser.Xpath_componentContext context)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void VisitErrorNode([NotNull] IErrorNode node)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public void VisitTerminal([NotNull] ITerminalNode node)
        {
            Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        private void Log(string method)
        {
            if (Logger == null || method == "EnterEveryRule" || method == "ExitEveryRule")
                return;

            if (method.StartsWith("Enter"))
                Logger.WriteLine($"<{method.Substring(5)}>");
            if (method.StartsWith("Exit"))
                Logger.WriteLine($"</{method.Substring(4)}>");
        }

        private void SuggestBoolean()
        {
            Suggest(rulesLexer.NOT_KEYWORD);
            Suggest(rulesLexer.TOKEN_KEYWORD);
            Suggest(rulesLexer.BOOLEAN_TRUE_LITERAL);
            Suggest(rulesLexer.BOOLEAN_FALSE_LITERAL);

            Suggest(KeywordSets.BooleanFunctions);
            Suggest(KeywordSets.ArithmeticFunctions);
        }

        private void Suggest(int tokenType)
        {
            Collected.Add(tokenType);
        }

        private void Suggest(IEnumerable<int> tokenTypes)
        {
            Collected.Add(tokenTypes);
        }

        private void Suggest(IEnumerable<string> suggestions)
        {
            Collected.Add(suggestions);
        }

        private bool IsOnEnd(ParserRuleContext context)
        {
            return context.Stop == null || context.Stop == Tokens[Tokens.Count - 1];
        }

        private bool IsNullOrEmpty(ParserRuleContext expr)
        {
            return Completeness.IsNullOrEmpty(expr);
        }

        private SuggestionBucket Collected { get; } = new SuggestionBucket(rulesLexer.DefaultVocabulary);
        private IList<IToken> Tokens { get; }
    }

}
