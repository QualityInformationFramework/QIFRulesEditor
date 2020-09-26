using Antlr4.Runtime;
using Re.AtnCompletion;
using Re.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    enum XPathExit
    {
        NotXPath,
        SyntaxError,
        WaitIdentifier,
        End
    }

    /// <summary> Experimental suggester. </summary>
    public sealed class Suggester
    {
        public bool SuggestSymbols { get; set; } = false;

        public IEnumerable<string> Run(string input)
        {
            var lexerWrapper = new LexerWrapper(new LexerFactory());
            var parser = new rulesParser(new CommonTokenStream(new rulesLexer(new AntlrInputStream(input))));
            var root = parser.dme_rules();

            var tree = root.ToStringTree(parser);

            return Run(lexerWrapper.TokenizeNonDefaultChannel(input).Tokens);
        }

        private IEnumerable<string> Run(IReadOnlyList<IToken> tokens)
        {
            this.tokens = tokens;
            if (tokens.Count == 0)
                return new string[] { GetName(rulesLexer.DME_RULE_KEYWORD) };

            GoDmeRule(0);
            return Collected;
        }

        private void GoDmeRule(int index)
        {
            if (tokens[index].Type != rulesLexer.DME_RULE_KEYWORD)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                if (SuggestSymbols)
                    Suggest(rulesLexer.T__0);
                return;
            }

            GoRuleOpenBrace(nextIndex);
        }

        private void GoRuleOpenBrace(int index)
        {
            if (tokens[index].Type != rulesLexer.T__0)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                Suggest(rulesLexer.UUID_KEYWORD);
                Suggest(rulesLexer.IF_KEYWORD);
                Suggest(rulesLexer.DME_THEN_DECISION_CLASS_KEYWORD);
                Suggest(rulesLexer.DME_THEN_ID_KEYWORD);
                Suggest(rulesLexer.DME_THEN_MAKE_MODEL_KEYWORD);
                return;
            }

            GoDmeIf(nextIndex);
            GoDmeClass(nextIndex);
            GoDmeId(nextIndex);
            GoDmeMakeModel(nextIndex);
        }

        private void GoDmeIf(int index)
        {
            if (tokens[index].Type != rulesLexer.IF_KEYWORD)
                return;

            GoBoolean(index);
        }

        private void GoBoolean(int index)
        {
            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                SuggestBoolean();
                return;
            }

            GoNot(nextIndex);
            GoBooleanFunction(nextIndex);
        }

        private void GoBooleanFunction(int nextIndex)
        {
            GoCharacteristicIs(nextIndex);
            GoFeatureIsDatum(nextIndex);
            GoFeatureIsInternal(nextIndex);
            GoFeatureTypeIs(nextIndex);
            GoSamplingCategoryIs(nextIndex);
            GoShapeClassIs(nextIndex);
        }

        private void GoShapeClassIs(int index)
        {
            GoIsEnum(index,
                rulesLexer.SHAPE_CLASS_IS_KEYWORD,
                rulesLexer.ENUM_SHAPE_CLASS,
                ConstantSets.ShapeClasses.Constants);
        }

        private void GoSamplingCategoryIs(int index)
        {
            if (tokens[index].Type != rulesLexer.SAMPLING_CATEGORY_IS_KEYWORD)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                if (SuggestSymbols)
                    Suggest(rulesLexer.T__3);
                return;
            }

            if (tokens[nextIndex].Type == rulesLexer.T__3)
            {
                if (nextIndex + 1 == tokens.Count)
                    return;

                if (tokens[nextIndex + 1].Type == rulesLexer.INTEGER)
                {
                    if (nextIndex + 2 == tokens.Count)
                    {
                        if (SuggestSymbols)
                            Suggest(rulesLexer.T__4);
                        return;
                    }

                    if (tokens[nextIndex + 2].Type == rulesLexer.T__4)
                    {
                        if (nextIndex + 3 == tokens.Count)
                        {
                            SuggestBooleanEnd();
                            return;
                        }
                        GoBooleanEnd(nextIndex + 3);
                    }
                }
            }
        }

        private void GoFeatureTypeIs(int index)
        {
            GoIsEnum(index,
                rulesLexer.FEATURE_TYPE_IS_KEYWORD,
                rulesLexer.ENUM_FEATURE,
                ConstantSets.FeatureTypes.Constants);
        }

        private void GoFeatureIsInternal(int index)
        {
            if (!IsTokenOfType(index, rulesLexer.FEATURE_IS_INTERNAL_KEYWORD))
                return;

            GoBooleanEnd(index);
        }

        private void GoFeatureIsDatum(int index)
        {
            if (!IsTokenOfType(index, rulesLexer.FEATURE_IS_INTERNAL_KEYWORD))
                return;

            GoBooleanEnd(index);
        }

        private void GoCharacteristicIs(int index)
        {
            GoIsEnum(index,
                rulesLexer.CHARACTERISTIC_IS_KEYWORD,
                rulesLexer.ENUM_CHARACTERISTIC,
                ConstantSets.CharacteristicTypes.Constants);
        }

        private void GoIsEnum(int index, int fnTokenType, int enumTokenType, IEnumerable<Constant> constants)
        {
            if (tokens[index].Type != fnTokenType)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                if (SuggestSymbols)
                    Suggest(rulesLexer.T__3);
                return;
            }

            if (tokens[nextIndex].Type == rulesLexer.T__3)
            {
                if (nextIndex + 1 == tokens.Count)
                {
                    Suggest(constants.Select(c => c.Name));
                    return;
                }

                if (tokens[nextIndex + 1].Type == enumTokenType)
                {
                    if (nextIndex + 2 == tokens.Count)
                    {
                        if (SuggestSymbols)
                            Suggest(rulesLexer.T__4);
                        return;
                    }

                    if (tokens[nextIndex + 2].Type == rulesLexer.T__4)
                    {
                        if (nextIndex + 3 == tokens.Count)
                        {
                            SuggestBooleanEnd();
                            return;
                        }
                        GoBooleanEnd(nextIndex + 3);
                    }
                }
            }
        }

        private void GoBooleanEnd(int index)
        {
            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                Suggest(rulesLexer.AND_KEYWORD);
                Suggest(rulesLexer.OR_KEYWORD);
                Suggest(rulesLexer.DME_THEN_DECISION_CLASS_KEYWORD);
                Suggest(rulesLexer.DME_THEN_ID_KEYWORD);
                Suggest(rulesLexer.DME_THEN_MAKE_MODEL_KEYWORD);
                return;
            }

            GoAnd(nextIndex);
            GoOr(nextIndex);
            GoDmeClass(nextIndex);
            GoDmeId(nextIndex);
            GoDmeMakeModel(nextIndex);
            // TODO: ')'
        }

        private void GoOr(int index)
        {
            if (tokens[index].Type != rulesLexer.OR_KEYWORD)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                SuggestBoolean();
                return;
            }

            GoBoolean(index);
        }

        private void GoAnd(int index)
        {
            if (tokens[index].Type != rulesLexer.AND_KEYWORD)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                SuggestBoolean();
                return;
            }

            GoBoolean(index);
        }

        private void GoNot(int index)
        {
            if (tokens[index].Type != rulesLexer.NOT_KEYWORD)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                SuggestBoolean();
                return;
            }

            GoBoolean(nextIndex);
        }

        private void GoDmeMakeModel(int index)
        {
            if (tokens[index].Type != rulesLexer.DME_THEN_MAKE_MODEL_KEYWORD)
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                Suggest(rulesLexer.MUST_KEYWORD);
                Suggest(rulesLexer.MAY_KEYWORD);
                return;
            }

            GoMust(nextIndex);
            GoMay(nextIndex);
        }

        private void GoMust(int index)
        {
            if (!IsTokenOfType(index, rulesLexer.MUST_KEYWORD))
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                Suggest(rulesLexer.NOT_KEYWORD);
                Suggest(ConstantSets.DmeClassName.Constants.Select(c => c.Name));
                return;
            }

            if (IsTokenOfType(nextIndex, rulesLexer.NOT_KEYWORD))
            {
                if (nextIndex + 1 == tokens.Count)
                {
                    Suggest(ConstantSets.DmeClassName.Constants.Select(c => c.Name));
                    return;
                }

                GoDmeClassValue(nextIndex + 1);
                return;
            }

            GoDmeClassValue(nextIndex + 1);
        }

        private void GoDmeClassValue(int index)
        {
            if (!IsTokenOfType(index, rulesLexer.ENUM_DME_CLASS))
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
            {
                Suggest(rulesLexer.WITH_KEYWORD);
                Suggest(rulesLexer.DME_THEN_ID_KEYWORD);
                Suggest(rulesLexer.DME_THEN_MAKE_MODEL_KEYWORD);
                Suggest(rulesLexer.DME_THEN_DECISION_CLASS_KEYWORD);
                return;
            }

            GoWith(nextIndex);
            GoDmeId(nextIndex);
            GoDmeMakeModel(nextIndex);
            GoDmeClass(nextIndex);
        }

        private void GoWith(int index)
        {
            if (!IsTokenOfType(index, rulesLexer.WITH_KEYWORD))
                return;

            int nextIndex = index + 1;
            if (nextIndex == tokens.Count)
                return;

            GoXPath(nextIndex);
        }

        private XPathExit GoXPath(int index)
        {
            if (!IsTokenOfType(index, rulesLexer.IDENTIFIER))
                return XPathExit.NotXPath;

            int prevType = rulesLexer.IDENTIFIER;
            do
            {
                if (index == tokens.Count)
                    break;

                if (IsTokenOfType(index, rulesLexer.IDENTIFIER) || IsTokenOfType(index, rulesLexer.T__11))
                {
                    // syntax error. Identifier and '/' must alternate
                    if (tokens[index].Type == prevType)
                        return XPathExit.SyntaxError;

                    ++index;
                    prevType = tokens[index].Type;
                }
                else
                    break;
            } while (true);

            if(index == tokens.Count)
            {
                if (IsTokenOfType(index - 1, rulesLexer.T__11))
                {
                    // TODO: list of identifiers
                    return XPathExit.WaitIdentifier;
                }

                if (SuggestSymbols)
                    Suggest(rulesLexer.T__11);
                return XPathExit.WaitIdentifier;
            }

            return XPathExit.End;
        }

        private void GoMay(int nextIndex)
        {
            throw new NotImplementedException();
        }
        private void GoDmeId(int v)
        {
            throw new NotImplementedException();
        }

        private void GoDmeClass(int v)
        {
            throw new NotImplementedException();
        }

        private HashSet<string> Collected = new HashSet<string>();

        private string GetName(int type)
        {
            return rulesLexer.DefaultVocabulary.GetDisplayName(type);
        }

        private void SuggestBoolean()
        {
            Suggest(rulesLexer.NOT_KEYWORD);
            Suggest(rulesLexer.TOKEN_KEYWORD);
            Suggest(rulesLexer.BOOLEAN_TRUE_LITERAL);
            Suggest(rulesLexer.BOOLEAN_FALSE_LITERAL);
            Suggest(rulesLexer.CHARACTERISTIC_IS_KEYWORD);
            Suggest(rulesLexer.FEATURE_TYPE_IS_KEYWORD);
            Suggest(rulesLexer.FEATURE_IS_DATUM_KEYWORD);
            Suggest(rulesLexer.FEATURE_IS_INTERNAL_KEYWORD);
            Suggest(rulesLexer.SAMPLING_CATEGORY_IS_KEYWORD);
            Suggest(rulesLexer.SHAPE_CLASS_IS_KEYWORD);
        }

        private void SuggestBooleanEnd()
        {
            Suggest(rulesLexer.AND_KEYWORD);
            Suggest(rulesLexer.OR_KEYWORD);
            Suggest(rulesLexer.DME_THEN_DECISION_CLASS_KEYWORD);
            Suggest(rulesLexer.DME_THEN_ID_KEYWORD);
            Suggest(rulesLexer.DME_THEN_MAKE_MODEL_KEYWORD);
            if (SuggestSymbols)
            {
                // TODO: check if there was and open brace
                Suggest(rulesLexer.T__4);
            }
        }

        private void Suggest(int tokenType)
        {
            Collected.Add(GetName(tokenType));
        }

        private void Suggest(IEnumerable<string> suggestions)
        {
            foreach (var s in suggestions)
                Collected.Add(s);
        }

        private void SuggestApplicability()
        {
            Suggest(rulesLexer.MUST_KEYWORD);
            Suggest(rulesLexer.MAY_KEYWORD);
        }

        private bool IsTokenOfType(int index, int type)
        {
            return tokens[index].Type == type;
        }

        //private void VisitState(int index, State state)
        //{
        //    if (tokens[index].Type != state.Token)
        //        return;

        //    if (tokens.Count == index + 1)
        //    {
        //        Collected.AddRange(state.NextStates.Select(s => s.Name));
        //        return;
        //    }

        //    foreach (var s in state.NextStates)
        //        VisitState( index + 1, s);
        //}
        private IReadOnlyList<IToken> tokens;
    }

    //class State
    //{
    //    public string Name { get; set; }
    //    public int Token { get; set; }
    //    public IReadOnlyList<int> NextStates { get; set; }
    //}

    //class StatePool
    //{
    //    public static State(int i)
    //    {
    //        return new State();
    //    }

    //    StatePool()
    //    {
    //        States
    //    }

    //    private List<State> States;
    //}
}
