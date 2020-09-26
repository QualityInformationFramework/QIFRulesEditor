using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Rules
{
    // Visit Re.Qif3.QIFRulesType and create language text
    class QifVisitorLanguage
    {
        public string Accept(Re.Qif3.QIFRulesType rules)
        {
            if(rules.DMESelectionRules == null)
                return "";
            return Visit((dynamic)rules.DMESelectionRules);
        }

        private string Visit(Re.Qif3.DMESelectionRulesType dmeRules)
        {
            var s = new StringBuilder();
            foreach (var rule in dmeRules.DMEDecisionRule)
                s.Append(Visit((dynamic)rule));
            return s.ToString();
        }

        private string Visit(Re.Qif3.IfThenDMERuleType dmeRule)
        {
            var s = new StringBuilder();
            s.AppendLine("dme_rule");
            s.AppendLine("{");

            // TODO name

            // UUID
            if (dmeRule.UUID != null)
            {
                s.AppendFormat("uuid = {0}", dmeRule.UUID);
                s.AppendLine();
                s.AppendLine();
            }

            // [if <boolean_expression>]
            if (dmeRule.BooleanExpression != null)
            {
                s.Append("if ");
                s.AppendLine(Visit((dynamic)dmeRule.BooleanExpression));
                s.AppendLine();
            }

            // dme_then...
            s.Append(Visit((dynamic)dmeRule.DMEThen));

            s.AppendLine("}");
            s.AppendLine();
            return s.ToString();
        }

        private string Visit(Re.Qif3.BooleanExpressionBaseType op) => "<TODO>";

        private string Visit(Re.Qif3.ShapeClassIsType t) => $"shape_class_is({t.val.ToString()})";

        private string Visit(Re.Qif3.FeatureTypeIsType t) => $"feature_type_is({t.val.ToString()})";

        private string Visit(Re.Qif3.FeatureIsInternalType t) => "feature_is_internal";

        private string Visit(Re.Qif3.FeatureIsDatumType t) => "feature_is_datum";

        private string Visit(Re.Qif3.CharacteristicIsType t) => $"characteristic_is({t.val.ToString()})";

        private string Visit(Re.Qif3.SamplingCategoryIsType t) => "sampling_category_is(" + t.val + ")";

        private string Visit(Re.Qif3.NotType op) => "not " + Visit((dynamic)op.BooleanExpression);

        private string Visit(Re.Qif3.ConstantIsType t) => Visit((dynamic)t.val);

        private string Visit(Re.Qif3.TokenEqualType t) =>
            Visit((dynamic)t.TokenExpression[0]) + " = " + Visit((dynamic)t.TokenExpression[1]);

        private string Visit(Re.Qif3.TokenConstantType t) => Re.Util.QuoteStr(t.val);

        // TODO t.ObjectId
        private string Visit(Re.Qif3.TokenParameterValueType t) => "token(" + t.Parameter + ")";

        private string Visit(Re.Qif3.LessOrEqualType op) =>
            "(" + Visit((dynamic)op.ArithmeticExpression[0]) +
            " <= " + Visit((dynamic)op.ArithmeticExpression[1]) + ")";

        private string Visit(Re.Qif3.LessThanType op) =>
            "(" + Visit((dynamic)op.ArithmeticExpression[0]) +
            " < " + Visit((dynamic)op.ArithmeticExpression[1]) + ")";

        private string Visit(Re.Qif3.GreaterOrEqualType op) =>
            "(" + Visit((dynamic)op.ArithmeticExpression[0]) +
            " >= " + Visit((dynamic)op.ArithmeticExpression[1]) + ")";

        private string Visit(Re.Qif3.GreaterThanType op) =>
            "(" + Visit((dynamic)op.ArithmeticExpression[0]) +
            " > " + Visit((dynamic)op.ArithmeticExpression[1]) + ")";

        private string Visit(Re.Qif3.BooleanEqualType op) =>
            "(" + Visit((dynamic)op.BooleanExpression[0]) +
            " = " + Visit((dynamic)op.BooleanExpression[1]) + ")";

        private string Visit(Re.Qif3.ArithmeticEqualType op) =>
            "(" + Visit((dynamic)op.ArithmeticExpression[0]) +
            " = " + Visit((dynamic)op.ArithmeticExpression[1]) + ")";

        private string Visit(Re.Qif3.AndType op)
        {
            var s = new StringBuilder();
            s.Append("(");
            for (int i = 0; i < op.BooleanExpression.Length; ++i)
            {
                if (i > 0)
                    s.Append(" and ");
                s.Append(Visit((dynamic)op.BooleanExpression[i]));
            }
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.OrType op)
        {
            var s = new StringBuilder();
            s.Append("(");
            for (int i = 0; i < op.BooleanExpression.Length; ++i)
            {
                if (i > 0)
                    s.Append(" or ");
                s.Append(Visit((dynamic)op.BooleanExpression[i]));
            }
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.DMEThenType dmeThen)
        {
            var s = new StringBuilder();
            foreach (var decision in dmeThen.DMEDecision)
            {
                s.Append(Visit((dynamic)decision));
                s.AppendLine();
            }
            return s.ToString();
        }

        private string Visit(Re.Qif3.DMEDecisionMakeModelType d)
        {
            var s = new StringBuilder();
            s.AppendFormat("dme_make_model {0}", Visit((dynamic)d.Applicability));
            s.AppendLine();
            s.AppendFormat("    manufacturer = {0}", Re.Util.QuoteStr(d.Manufacturer));
            s.AppendLine();
            s.AppendFormat("    model_number = {0}", Re.Util.QuoteStr(d.ModelNumber));
            s.AppendLine();
            if (d.SerialNumber != null)
            {
                s.AppendFormat("    serial_number = {0}", Re.Util.QuoteStr(d.SerialNumber));
                s.AppendLine();
            }
            return s.ToString();
        }

        private string Visit(Re.Qif3.DMEDecisionIdType d)
        {
            // TODO xIdField, xIdFieldSpecified
            var s = new StringBuilder();
            s.AppendFormat("dme_id {0} {1}", Visit((dynamic)d.Applicability), d.DMEId.Value);
            s.AppendLine();
            return s.ToString();
        }

        private string Visit(Re.Qif3.DMEDecisionClassType d)
        {
            var s = new StringBuilder();
            s.AppendFormat("dme_class {0} {1}", Visit((dynamic)d.Applicability), d.DMEClassName.ToString());
            s.AppendLine();
            if (d.ParameterConstraints != null)
                s.Append(Visit((dynamic)d.ParameterConstraints));
            return s.ToString();
        }

        private string Visit(Re.Qif3.DMEParameterConstraintSetType pcs)
        {
            var s = new StringBuilder();
            foreach (var pc in pcs.DMEParameterConstraint)
                s.Append(Visit((dynamic)pc));
            return s.ToString();
        }

        private string Visit(Re.Qif3.DMEParameterConstraintType pc)
        {
            var s = new StringBuilder();
            s.AppendFormat("    with {0} {1} {2}",
                pc.ParameterName,
                Visit((dynamic)pc.Comparison),
                Visit((dynamic)pc.ArithmeticExpression));
            s.AppendLine();
            return s.ToString();
        }

        private string Visit(Re.Qif3.ArithmeticComparisonEnumType op)
        {
            switch (op)
            {
                case Re.Qif3.ArithmeticComparisonEnumType.EQUAL: return "=";
                case Re.Qif3.ArithmeticComparisonEnumType.GREATER: return ">";
                case Re.Qif3.ArithmeticComparisonEnumType.GREATEROREQUAL: return ">=";
                case Re.Qif3.ArithmeticComparisonEnumType.LESS: return "<";
                case Re.Qif3.ArithmeticComparisonEnumType.LESSOREQUAL: return "<=";
            }
            return "<UNKNOWN>";
        }

        private string Visit(Re.Qif3.BooleanConstantEnumType t)
        {
            switch (t)
            {
                case Re.Qif3.BooleanConstantEnumType.QIF_FALSE: return "false";
                case Re.Qif3.BooleanConstantEnumType.QIF_TRUE: return "true";
            }
            return "<UNKNOWN>";
        }

        //[System.Xml.Serialization.XmlElementAttribute("ArithmeticParameterValue", typeof(ArithmeticParameterValueType))] - Plan
        private string Visit(Re.Qif3.ArithmeticExpressionBaseType ae)
        {
            return "<TODO>";
        }

        // from Plan?
        private string Visit(Re.Qif3.VariableValueType t) => "variable(" + t.VariableName + ")";

        private string Visit(Re.Qif3.FeatureSizeType t) => "feature_size";

        private string Visit(Re.Qif3.FeatureAreaType t) => "feature_area";

        private string Visit(Re.Qif3.FeatureLengthType t) => "feature_length";

        private string Visit(Re.Qif3.CharacteristicToleranceType t) => "characteristic_tolerance";

        private string Visit(Re.Qif3.ArithmeticPartParameterType t) => $"part_parameter({t.Parameter})";

        private string Visit(Re.Qif3.MinusType op) =>
            "(" + Visit((dynamic)op.ArithmeticExpression[0]) +
            " - " + Visit((dynamic)op.ArithmeticExpression[1]) + ")";

        private string Visit(Re.Qif3.DividedByType op) =>
            "(" + Visit((dynamic)op.ArithmeticExpression[0]) +
            " / " + Visit((dynamic)op.ArithmeticExpression[1]) + ")";

        private string Visit(Re.Qif3.NegateType op) => "-" + Visit((dynamic)op.ArithmeticExpression);

        private string Visit(Re.Qif3.MaxType op)
        {
            var s = new StringBuilder();
            s.Append("max(");
            for (int i = 0; i < op.ArithmeticExpression.Length; ++i)
            {
                if (i > 0)
                    s.Append(", ");
                s.Append(Visit((dynamic)op.ArithmeticExpression[i]));
            }
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.MinType op)
        {
            var s = new StringBuilder();
            s.Append("min(");
            for (int i = 0; i < op.ArithmeticExpression.Length; ++i)
            {
                if (i > 0)
                    s.Append(", ");
                s.Append(Visit((dynamic)op.ArithmeticExpression[i]));
            }
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.TimesType op)
        {
            var s = new StringBuilder();
            s.Append("(");
            for (int i = 0; i < op.ArithmeticExpression.Length; ++i)
            {
                if (i > 0)
                    s.Append(" * ");
                s.Append(Visit((dynamic)op.ArithmeticExpression[i]));
            }
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.PlusType op)
        {
            var s = new StringBuilder();
            s.Append("(");
            for (int i = 0; i < op.ArithmeticExpression.Length; ++i)
            {
                if (i > 0)
                    s.Append(" + ");
                s.Append(Visit((dynamic)op.ArithmeticExpression[i]));
            }
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.ArithmeticCharacteristicParameterType t)
        {
            var s = new StringBuilder();
            s.AppendFormat("characteristic_parameter({0}", t.Parameter);
            if (t.CharacteristicTypeEnumSpecified)
                s.AppendFormat(", {0}", t.CharacteristicTypeEnum.ToString());
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.ArithmeticFeatureParameterType t)
        {
            var s = new StringBuilder();
            s.AppendFormat("feature_parameter({0}", t.Parameter);
            if (t.FeatureTypeEnumSpecified)
                s.AppendFormat(", {0}", t.FeatureTypeEnum.ToString());
            s.Append(")");
            return s.ToString();
        }

        private string Visit(Re.Qif3.ArithmeticDMEParameterType t) =>
            "dme_parameter(" + t.Parameter + ", " + t.DMEClassNameEnum.ToString() + ")";

        private string Visit(Re.Qif3.ArithmeticConstantType t) => t.val.ToString(System.Globalization.CultureInfo.InvariantCulture);

        private string Visit(Re.Qif3.QIFMayType t) => "may";

        private string Visit(Re.Qif3.QIFMustNotType t) => "must not";

        private string Visit(Re.Qif3.QIFMustType t) => "must";
    }
}
