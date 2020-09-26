using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re.Grammar;

namespace Re.Rules
{
    // Visit Re.Rules.Ast and create QIF rules classes
    class AstVisitorQif
    {
        public Re.Qif3.DMESelectionRulesType Accept(rulesParser.Dme_rulesContext rules)
        {
            return Visit(rules);
        }

        private Re.Qif3.DMESelectionRulesType Visit(rulesParser.Dme_rulesContext rules)
        {
            int nRule = rules.dme_rule().Length;
            var r = new Re.Qif3.IfThenDMERuleType[nRule];
            for (int i = 0; i < nRule; ++i)
                r[i] = Visit(rules.dme_rule()[i]);

            return new Re.Qif3.DMESelectionRulesType()
            {
                n = (uint)nRule,
                DMEDecisionRule = r
            };
        }

        private Re.Qif3.IfThenDMERuleType Visit(rulesParser.Dme_ruleContext rule)
        {
            // TODO t.name
            var uuid = (rule.uuid_statement() != null) ? Visit(rule.uuid_statement()) : null;
            var expr = (rule.if_statement() != null) ? Visit(rule.if_statement()) : null;
            int nThen = rule.dme_then_statement().Length;

            var decisions = new Re.Qif3.DMEDecisionBaseType[nThen];
            for (int i = 0; i < nThen; ++i)
                decisions[i] = Visit(rule.dme_then_statement()[i]);

            var dmeThen = new Re.Qif3.DMEThenType()
            {
                n = (uint)nThen,
                DMEDecision = decisions
            };

            return new Re.Qif3.IfThenDMERuleType()
            {
                UUID = uuid,
                BooleanExpression = expr,
                DMEThen = dmeThen
            };
        }

        private string Visit(rulesParser.Uuid_statementContext t) => t.UUID().Symbol.Text;

        private Re.Qif3.BooleanExpressionBaseType Visit(rulesParser.If_statementContext t)
        {
            return Visit((dynamic)t.boolean_expr());
        }

        private Re.Qif3.BooleanExpressionBaseType Visit(rulesParser.Boolean_exprContext t)
        {
            // processing for all derived classes must be implemented
            Debug.Assert(false);
            return null;
        }

        private Re.Qif3.BooleanExpressionBaseType Visit(rulesParser.Boolean_expr_fnContext t)
        {
            return Visit(t.boolean_fn());
        }

        private Re.Qif3.BooleanExpressionBaseType Visit(rulesParser.Boolean_fnContext t)
        {
            if (t.characteristic_is() != null)
                return Visit(t.characteristic_is());
            if (t.feature_is_datum() != null)
                return Visit(t.feature_is_datum());
            if (t.feature_is_internal() != null)
                return Visit(t.feature_is_internal());
            if (t.feature_type_is() != null)
                return Visit(t.feature_type_is());
            if (t.sampling_category_is() != null)
                return Visit(t.sampling_category_is());
            if (t.shape_class_is() != null)
                return Visit(t.shape_class_is());
            return null;
        }

        private Re.Qif3.CharacteristicIsType Visit(rulesParser.Characteristic_isContext t)
        {
            return new Re.Qif3.CharacteristicIsType()
            {
                val = CharacteristicTypeEnum(t.ENUM_CHARACTERISTIC().Symbol.Text)
            };
        }

        private Re.Qif3.FeatureIsDatumType Visit(rulesParser.Feature_is_datumContext t) => new Re.Qif3.FeatureIsDatumType();

        private Re.Qif3.FeatureIsInternalType Visit(rulesParser.Feature_is_internalContext t) => new Re.Qif3.FeatureIsInternalType();

        private Re.Qif3.FeatureTypeIsType Visit(rulesParser.Feature_type_isContext t)
        {
            return new Re.Qif3.FeatureTypeIsType() { val = FeatureTypeEnum(t.ENUM_FEATURE().Symbol.Text) };
        }

        private Re.Qif3.SamplingCategoryIsType Visit(rulesParser.Sampling_category_isContext t)
        {
            return new Re.Qif3.SamplingCategoryIsType()
            {
                val = uint.Parse(t.INTEGER().Symbol.Text)
            };
        }

        private Re.Qif3.ShapeClassIsType Visit(rulesParser.Shape_class_isContext t)
        {
            return new Re.Qif3.ShapeClassIsType()
            {
                val = ShapeClassEnum(t.ENUM_SHAPE_CLASS().Symbol.Text)
            };
        }

        private Re.Qif3.TokenEqualType Visit(rulesParser.Boolean_expr_tokenContext t)
        {
            return new Re.Qif3.TokenEqualType()
            {
                TokenExpression = new Re.Qif3.TokenExpressionBaseType[2]
                {
                    Visit(t.token_var()[0]),
                    Visit(t.token_var()[1])
                }
            };
        }

        private Re.Qif3.TokenExpressionBaseType Visit(rulesParser.Token_varContext t)
        {
            if (t.token_fn() != null)
                return Visit(t.token_fn());

            return new Re.Qif3.TokenConstantType()
            {
                val = Re.Util.UnQuoteStr(t.STRING_LITERAL().Symbol.Text)
            };
        }

        private Re.Qif3.TokenParameterValueType Visit(rulesParser.Token_fnContext t)
        {
            // TODO objectid
            return new Re.Qif3.TokenParameterValueType()
            {
                Parameter = t.xpath().GetText()
            };
        }

        private Re.Qif3.BooleanExpressionBaseType Visit(rulesParser.Boolean_expr_inContext t)
        {
            // TODO this is experimental language extension
            Debug.Assert(false);
            return null;
        }

        private Re.Qif3.ConstantIsType Visit(rulesParser.Boolean_expr_literalContext t)
        {
            return new Re.Qif3.ConstantIsType() { val = BooleanConstantEnum(t.boolean_literal()) };
        }

        private Re.Qif3.BooleanExpressionBaseType Visit(rulesParser.Boolean_expr_bracesContext t)
        {
            return Visit((dynamic)t.boolean_expr());
        }

        private Re.Qif3.BooleanExpressionBaseType Visit(rulesParser.Boolean_expr_arithmetic_comparisonContext t)
        {
            if (t.arithmetic_comp_literal().arithmetic_comp_literal_equal() != null)
            {
                return new Re.Qif3.ArithmeticEqualType()
                {
                    ArithmeticExpression = new Re.Qif3.ArithmeticExpressionBaseType[2]
                    {
                        Visit((dynamic)t.arithmetic_expr()[0]),
                        Visit((dynamic)t.arithmetic_expr()[1])
                    }
                };
            }

            if (t.arithmetic_comp_literal().arithmetic_comp_literal_greater() != null)
            {
                return new Re.Qif3.GreaterThanType()
                {
                    ArithmeticExpression = new Re.Qif3.ArithmeticExpressionBaseType[2]
                    {
                        Visit((dynamic)t.arithmetic_expr()[0]),
                        Visit((dynamic)t.arithmetic_expr()[1])
                    }
                };
            }

            if (t.arithmetic_comp_literal().arithmetic_comp_literal_greater_or_equal() != null)
            {
                return new Re.Qif3.GreaterOrEqualType()
                {
                    ArithmeticExpression = new Re.Qif3.ArithmeticExpressionBaseType[2]
                    {
                        Visit((dynamic)t.arithmetic_expr()[0]),
                        Visit((dynamic)t.arithmetic_expr()[1])
                    }
                };
            }

            if (t.arithmetic_comp_literal().arithmetic_comp_literal_less() != null)
            {
                return new Re.Qif3.LessThanType()
                {
                    ArithmeticExpression = new Re.Qif3.ArithmeticExpressionBaseType[2]
                    {
                        Visit((dynamic)t.arithmetic_expr()[0]),
                        Visit((dynamic)t.arithmetic_expr()[1])
                    }
                };
            }

            if (t.arithmetic_comp_literal().arithmetic_comp_literal_less_or_equal() != null)
            {
                return new Re.Qif3.LessOrEqualType()
                {
                    ArithmeticExpression = new Re.Qif3.ArithmeticExpressionBaseType[2]
                    {
                        Visit((dynamic)t.arithmetic_expr()[0]),
                        Visit((dynamic)t.arithmetic_expr()[1])
                    }
                };
            }

            return null;
        }

        private Re.Qif3.NotType Visit(rulesParser.Boolean_expr_notContext t)
        {
            return new Re.Qif3.NotType()
            {
                BooleanExpression = Visit((dynamic)t.boolean_expr())
            };
        }

        private Re.Qif3.AndType Visit(rulesParser.Boolean_expr_andContext t)
        {
            return new Re.Qif3.AndType()
            {
                n = 2,
                BooleanExpression = new Re.Qif3.BooleanExpressionBaseType[2] {
                    Visit((dynamic)t.boolean_expr()[0]),
                    Visit((dynamic)t.boolean_expr()[1])
                }
            };
        }

        private Re.Qif3.OrType Visit(rulesParser.Boolean_expr_orContext t)
        {
            return new Re.Qif3.OrType()
            {
                n = 2,
                BooleanExpression = new Re.Qif3.BooleanExpressionBaseType[2] {
                    Visit((dynamic)t.boolean_expr()[0]),
                    Visit((dynamic)t.boolean_expr()[1])
                }
            };
        }

        private Re.Qif3.DMEDecisionBaseType Visit(rulesParser.Dme_then_statementContext t)
        {
            if (t.dme_then_decision_class() != null)
                return Visit(t.dme_then_decision_class());
            if (t.dme_then_id() != null)
                return Visit(t.dme_then_id());
            if (t.dme_then_make_model() != null)
                return Visit(t.dme_then_make_model());
            return null;
        }

        private Re.Qif3.DMEParameterConstraintSetType DMEParameterConstraintSet(rulesParser.With_statementContext[] w)
        {
            if (w.Length == 0)
                return null;

            int nConstraint = w.Length;
            var constraints = new Re.Qif3.DMEParameterConstraintType[nConstraint];
            for (int i = 0; i < nConstraint; ++i)
                constraints[i] = DMEParameterConstraint(w[i]);

            return new Re.Qif3.DMEParameterConstraintSetType()
            {
                n = (uint)nConstraint,
                DMEParameterConstraint = constraints
            };
        }

        private Re.Qif3.DMEParameterConstraintType DMEParameterConstraint(rulesParser.With_statementContext w)
        {
            return new Re.Qif3.DMEParameterConstraintType()
            {
                ParameterName = w.xpath().GetText(),
                Comparison = ArithmeticComparisonEnum(w.arithmetic_comp_literal()),
                ArithmeticExpression = Visit((dynamic)w.arithmetic_expr())
            };
        }

        private Re.Qif3.ArithmeticExpressionBaseType Visit(rulesParser.Arithmetic_exprContext t)
        {
            // processing for all derived classes must be implemented
            Debug.Assert(false);
            return null;
        }

        private Re.Qif3.ArithmeticExpressionBaseType Visit(rulesParser.Arithmetic_expr_mult_divContext t)
        {
            if (t.arithmetic_mult_div_literal().arithmetic_div_literal() != null)
            {
                return new Re.Qif3.DividedByType()
                {
                    ArithmeticExpression = new Re.Qif3.ArithmeticExpressionBaseType[2]
                    {
                        Visit((dynamic)t.arithmetic_expr()[0]),
                        Visit((dynamic)t.arithmetic_expr()[1])
                    }
                };
            }

            if (t.arithmetic_mult_div_literal().arithmetic_mult_literal() != null)
            {
                return new Re.Qif3.TimesType()
                {
                    ArithmeticExpression = new Re.Qif3.ArithmeticExpressionBaseType[2]
                    {
                        Visit((dynamic)t.arithmetic_expr()[0]),
                        Visit((dynamic)t.arithmetic_expr()[1])
                    }
                };
            }

            return null;
        }

        private Re.Qif3.NegateType Visit(rulesParser.Arithmetic_expr_negateContext t)
        {
            return new Re.Qif3.NegateType()
            {
                ArithmeticExpression = Visit((dynamic)t.arithmetic_expr())
            };
        }

        private Re.Qif3.ArithmeticConstantType Visit(rulesParser.Arithmetic_expr_constContext t)
        {
            decimal v = 0;
            if (t.arithmetic_const().DOUBLE() != null)
                v = decimal.Parse(t.arithmetic_const().DOUBLE().GetText(), System.Globalization.NumberStyles.Float);
            else if (t.arithmetic_const().INTEGER() != null)
                v = decimal.Parse(t.arithmetic_const().INTEGER().GetText());
            else
                throw new Exception("Arithmetic_expr_constContext: bad underlying type");
            return new Re.Qif3.ArithmeticConstantType() { val = v };
        }

        private Re.Qif3.ArithmeticExpressionBaseType Visit(rulesParser.Arithmetic_expr_plus_minusContext t)
        {
            var expr = new Re.Qif3.ArithmeticExpressionBaseType[2]
            {
                Visit((dynamic)t.arithmetic_expr()[0]),
                Visit((dynamic)t.arithmetic_expr()[1])
            };

            if (t.arithmetic_plus_minus_literal().arithmetic_plus_literal() != null)
                return new Re.Qif3.PlusType() { ArithmeticExpression = expr };

            return new Re.Qif3.MinusType() { ArithmeticExpression = expr };
        }

        private Re.Qif3.ArithmeticExpressionBaseType Visit(rulesParser.Arithmetic_fnContext t)
        {
            if (t.characteristic_parameter() != null)
                return Visit(t.characteristic_parameter());
            if (t.characteristic_tolerance() != null)
                return Visit(t.characteristic_tolerance());
            if (t.dme_parameter() != null)
                return Visit(t.dme_parameter());
            if (t.feature_area() != null)
                return Visit(t.feature_area());
            if (t.feature_length() != null)
                return Visit(t.feature_length());
            if (t.feature_parameter() != null)
                return Visit(t.feature_parameter());
            if (t.feature_size() != null)
                return Visit(t.feature_size());
            if (t.max() != null)
                return Visit(t.max());
            if (t.min() != null)
                return Visit(t.min());
            if (t.object_parameter() != null)
                return Visit(t.object_parameter());
            if (t.part_parameter() != null)
                return Visit(t.part_parameter());
            if (t.variable() != null)
                return Visit(t.variable());
            return null;
        }

        private Re.Qif3.CharacteristicToleranceType Visit(rulesParser.Characteristic_toleranceContext t)
        {
            return new Re.Qif3.CharacteristicToleranceType();
        }

        private Re.Qif3.ArithmeticDMEParameterType Visit(rulesParser.Dme_parameterContext t)
        {
            return new Re.Qif3.ArithmeticDMEParameterType()
            {
                Parameter = t.xpath().GetText(),
                DMEClassNameEnum = DmeClassNameEnum(t.ENUM_DME_CLASS().Symbol.Text)
            };
        }

        private Re.Qif3.FeatureAreaType Visit(rulesParser.Feature_areaContext t) => new Re.Qif3.FeatureAreaType();

        private Re.Qif3.FeatureLengthType Visit(rulesParser.Feature_lengthContext t) => new Re.Qif3.FeatureLengthType();

        private Re.Qif3.ArithmeticFeatureParameterType Visit(rulesParser.Feature_parameterContext t)
        {
            if (t.ENUM_FEATURE() == null)
            {
                return new Re.Qif3.ArithmeticFeatureParameterType()
                {
                    Parameter = t.xpath().GetText(),
                    FeatureTypeEnumSpecified = false,
                };
            }

            return new Re.Qif3.ArithmeticFeatureParameterType()
            {
                Parameter = t.xpath().GetText(),
                FeatureTypeEnumSpecified = true,
                FeatureTypeEnum = FeatureTypeEnum(t.ENUM_FEATURE().Symbol.Text)
            };
        }

        private Re.Qif3.FeatureSizeType Visit(rulesParser.Feature_sizeContext t) => new Re.Qif3.FeatureSizeType();

        private Re.Qif3.MaxType Visit(rulesParser.MaxContext t)
        {
            int n = t.arithmetic_expr().Length;
            var aExpr = new Re.Qif3.ArithmeticExpressionBaseType[n];
            for (int i = 0; i < n; ++i)
                aExpr[i] = Visit((dynamic)t.arithmetic_expr()[i]);
            return new Re.Qif3.MaxType() { ArithmeticExpression = aExpr };
        }

        private Re.Qif3.MinType Visit(rulesParser.MinContext t)
        {
            int n = t.arithmetic_expr().Length;
            var aExpr = new Re.Qif3.ArithmeticExpressionBaseType[n];
            for (int i = 0; i < n; ++i)
                aExpr[i] = Visit((dynamic)t.arithmetic_expr()[i]);
            return new Re.Qif3.MinType() { ArithmeticExpression = aExpr };
        }

        private Re.Qif3.ArithmeticParameterValueType Visit(rulesParser.Object_parameterContext t)
        {
            // TODO ObjectId
            return new Re.Qif3.ArithmeticParameterValueType()
            {
                Parameter = t.xpath().GetText()
            };
        }

        private Re.Qif3.ArithmeticPartParameterType Visit(rulesParser.Part_parameterContext t)
        {
            return new Re.Qif3.ArithmeticPartParameterType() { Parameter = t.xpath().GetText() };
        }

        private Re.Qif3.VariableValueType Visit(rulesParser.VariableContext t)
        {
            return new Re.Qif3.VariableValueType() { VariableName = t.identifier().GetText() };
        }

        private Re.Qif3.ArithmeticCharacteristicParameterType Visit(rulesParser.Characteristic_parameterContext t)
        {
            if (t.ENUM_CHARACTERISTIC() == null)
            {
                return new Re.Qif3.ArithmeticCharacteristicParameterType()
                {
                    Parameter = t.xpath().GetText(),
                    CharacteristicTypeEnumSpecified = false
                };
            }

            return new Re.Qif3.ArithmeticCharacteristicParameterType()
            {
                Parameter = t.xpath().GetText(),
                CharacteristicTypeEnumSpecified = true,
                CharacteristicTypeEnum = CharacteristicTypeEnum(t.ENUM_CHARACTERISTIC().Symbol.Text)
            };
        }

        private Re.Qif3.ArithmeticExpressionBaseType Visit(rulesParser.Arithmetic_expr_fnContext t)
        {
            return Visit(t.arithmetic_fn());
        }

        private Re.Qif3.ArithmeticExpressionBaseType Visit(rulesParser.Arithmetic_expr_bracesContext t)
        {
            return Visit((dynamic)t.arithmetic_expr());
        }

        private Re.Qif3.ApplicabilityBaseType Visit(rulesParser.Dme_applicabilityContext a)
        {
            // TODO desirability
            if (a.dme_applicability_may() != null)
                return new Re.Qif3.QIFMayType();
            if (a.dme_applicability_must() != null)
                return new Re.Qif3.QIFMustType();
            if (a.dme_applicability_must_not() != null)
                return new Re.Qif3.QIFMustNotType();
            return null;
        }

        private Re.Qif3.DMEDecisionClassType Visit(rulesParser.Dme_then_decision_classContext d)
        {
            return new Re.Qif3.DMEDecisionClassType()
            {
                Applicability = Visit(d.dme_applicability()),
                DMEClassName = DmeClassNameEnum(d.ENUM_DME_CLASS().Symbol.Text),
                ParameterConstraints = DMEParameterConstraintSet(d.with_statement())
            };
        }

        private Re.Qif3.DMEDecisionIdType Visit(rulesParser.Dme_then_idContext t)
        {
            // TODO xid
            return new Re.Qif3.DMEDecisionIdType()
            {
                Applicability = Visit(t.dme_applicability()),
                DMEId = new Re.Qif3.QIFReferenceType()
                {
                    Value = uint.Parse(t.INTEGER().Symbol.Text)
                }
            };
        }

        private Re.Qif3.DMEDecisionMakeModelType Visit(rulesParser.Dme_then_make_modelContext t)
        {
            var manufacturer = Re.Util.UnQuoteStr(t.manufacturer().STRING_LITERAL().Symbol.Text);
            var modelNumber = Re.Util.UnQuoteStr(t.model_number().STRING_LITERAL().Symbol.Text);
            string serialNumber = null;
            if (t.serial_number() != null)
                serialNumber = Re.Util.UnQuoteStr(t.serial_number().STRING_LITERAL().Symbol.Text);

            return new Re.Qif3.DMEDecisionMakeModelType()
            {
                Applicability = Visit(t.dme_applicability()),
                Manufacturer = manufacturer,
                ModelNumber = modelNumber,
                SerialNumber = serialNumber
            };
        }

        private Re.Qif3.CharacteristicTypeEnumType CharacteristicTypeEnum(string s)
        {
            Re.Qif3.CharacteristicTypeEnumType t;
            if (!Enum.TryParse(s, out t))
                Debug.Assert(true);
            return t;
        }

        private Re.Qif3.FeatureTypeEnumType FeatureTypeEnum(string s)
        {
            Re.Qif3.FeatureTypeEnumType t;
            if (!Enum.TryParse(s, out t))
                Debug.Assert(true);
            return t;
        }

        private Re.Qif3.ShapeClassEnumType ShapeClassEnum(string s)
        {
            Re.Qif3.ShapeClassEnumType t;
            if (!Enum.TryParse(s, out t))
                Debug.Assert(true);
            return t;
        }

        private Re.Qif3.ArithmeticComparisonEnumType ArithmeticComparisonEnum(rulesParser.Arithmetic_comp_literalContext t)
        {
            if (t.arithmetic_comp_literal_equal() != null)
                return Re.Qif3.ArithmeticComparisonEnumType.EQUAL;
            if (t.arithmetic_comp_literal_greater() != null)
                return Re.Qif3.ArithmeticComparisonEnumType.GREATER;
            if (t.arithmetic_comp_literal_greater_or_equal() != null)
                return Re.Qif3.ArithmeticComparisonEnumType.GREATEROREQUAL;
            if (t.arithmetic_comp_literal_less() != null)
                return Re.Qif3.ArithmeticComparisonEnumType.LESS;
            if (t.arithmetic_comp_literal_less_or_equal() != null)
                return Re.Qif3.ArithmeticComparisonEnumType.LESSOREQUAL;
            throw new Exception("Arithmetic_comparisonContext: bad value");
        }

        private Re.Qif3.DMEClassNameEnumType DmeClassNameEnum(string s)
        {
            Re.Qif3.DMEClassNameEnumType t;
            if (!Enum.TryParse(s, out t))
                Debug.Assert(true);
            return t;
        }

        Re.Qif3.BooleanConstantEnumType BooleanConstantEnum(rulesParser.Boolean_literalContext t)
        {
            return (t.BOOLEAN_TRUE_LITERAL() != null)
                ? Re.Qif3.BooleanConstantEnumType.QIF_TRUE
                : Re.Qif3.BooleanConstantEnumType.QIF_FALSE;
        }
    }
}
