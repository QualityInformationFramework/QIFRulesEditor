using Re.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re
{
    static class KeywordSets
    {
        public static int[] BooleanFunctions { get; } = new int[]
        {
            rulesLexer.CHARACTERISTIC_IS_KEYWORD,
            rulesLexer.FEATURE_TYPE_IS_KEYWORD,
            rulesLexer.FEATURE_IS_DATUM_KEYWORD,
            rulesLexer.FEATURE_IS_INTERNAL_KEYWORD,
            rulesLexer.SAMPLING_CATEGORY_IS_KEYWORD,
            rulesLexer.SHAPE_CLASS_IS_KEYWORD
        };

        public static int[] ArithmeticFunctions { get; } = new int[]
        {
            rulesLexer.VARIABLE_KEYWORD,
            rulesLexer.FEATURE_PARAMETER_KEYWORD,
            rulesLexer.CHARACTERISTIC_PARAMETER_KEYWORD,
            rulesLexer.DME_PARAMETER_KEYWORD,
            rulesLexer.PART_PARAMETER_KEYWORD,
            rulesLexer.FEATURE_LENGTH_KEYWORD,
            rulesLexer.FEATURE_AREA_KEYWORD,
            rulesLexer.FEATURE_SIZE_KEYWORD,
            rulesLexer.MAX_KEYWORD,
            rulesLexer.MIN_KEYWORD,
            rulesLexer.CHARACTERISTIC_TOLERANCE_KEYWORD,
            rulesLexer.OBJECT_PARAMETER_KEYWORD
        };

        public static int[] ThenBlocks { get; } = new int[]
        {
            rulesLexer.DME_THEN_DECISION_CLASS_KEYWORD,
            rulesLexer.DME_THEN_ID_KEYWORD,
            rulesLexer.DME_THEN_MAKE_MODEL_KEYWORD
        };

        public static int[] BooleanOperators { get; } = new int[]
        {
            rulesLexer.AND_KEYWORD,
            rulesParser.OR_KEYWORD
        };
    }
}
