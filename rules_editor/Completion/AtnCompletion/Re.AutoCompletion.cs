using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.AtnCompletion
{
    /// <summary> The idea is taken from https://github.com/oranoran/antlr4-autosuggest. </summary>
    class AutoCompletion
    {
        public AutoCompletion(ILexerFactory lexerFactory, IParserFactory parserFactory, string input)
        {
            Input = input;
            LexerWrapper = new LexerWrapper(lexerFactory);
            ParserWrapper = new ParserWrapper(parserFactory, LexerWrapper.Vocabulary);
        }

        public CasePreference CasePreference { get; set; } = CasePreference.Both;
        public IEnumerable<string> ExcludedRules { get; set; } = new List<string>();
        public IReadOnlyList<IToken> InputTokens { get; private set; }
        public string UntokenizedText { get; private set; }
        public LexerWrapper LexerWrapper { get; private set; }
        public IReadOnlyCollection<string> Suggest()
        {
            var tokenizationResult = LexerWrapper.TokenizeNonDefaultChannel(Input);
            InputTokens = tokenizationResult.Tokens;
            UntokenizedText = tokenizationResult.UntokenizedText;

            RunParserAtnAndCollectSuggestions();
            return CollectedSuggestions;
        }

        private void RunParserAtnAndCollectSuggestions()
        {
            ParseAndCollectTokenSuggestions(ParserWrapper.AtnState(0), 0);
        }

        private void ParseAndCollectTokenSuggestions(ATNState state, int tokenIndex)
        {
            if (DidVisitParserStateOnThisTokenIndex(state, tokenIndex))
                return;

            var prevTokenListIndexForThisState = SetParserStateLastVisitedOnThisTokenIndex(state, tokenIndex);

            try
            {
                if (!HaveMoreTokens(tokenIndex))
                {
                    SuggestNextTokensForParserState(state);
                    return;
                }

                foreach (var t in state.Transitions)
                {
                    if (t.IsEpsilon)
                        ParseAndCollectTokenSuggestions(t.target, tokenIndex);
                    else if (t is AtomTransition)
                        HandleAtomicTransition(t as AtomTransition, tokenIndex);
                    else
                        HandleSetTransition(t as SetTransition, tokenIndex);
                }
            }
            finally
            {
                SetParserStateLastVisitedOnThisTokenIndex(state, prevTokenListIndexForThisState);
            }
        }

        private void HandleAtomicTransition(AtomTransition t, int tokenIndex)
        {
            var nextToken = InputTokens[tokenIndex];
            if (t.label == nextToken.Type)
                ParseAndCollectTokenSuggestions(t.target, tokenIndex + 1);
        }

        private void HandleSetTransition(SetTransition t, int tokenIndex)
        {
            var nextToken = InputTokens[tokenIndex];
            int type = nextToken.Type;
            foreach (int transTokenType in t.Label.ToArray())
            {
                if (transTokenType == type)
                    ParseAndCollectTokenSuggestions(t.target, tokenIndex + 1);
            }
        }

        private void SuggestNextTokensForParserState(ATNState state)
        {
            // do not start collecting from the same state twice (speed up)
            if (!CollectedStates.Add(state))
                return;

            var transitionLabels = new HashSet<int>();
            FillParserTransitionLabels(state, transitionLabels, new HashSet<TransitionWrapper>());
            var tokenSuggester = new TokenSuggester(UntokenizedText, LexerWrapper, CasePreference) { ExcludedRules = ExcludedRules };
            var suggestions = tokenSuggester.Suggest(transitionLabels);
            ParseSuggestionsAndAddValidOnes(state, suggestions);
        }

        private void ParseSuggestionsAndAddValidOnes(ATNState state, IEnumerable<string> suggestions)
        {
            foreach (var suggestion in suggestions)
            {
                var addedToken = GetAddedToken(suggestion);
                if (IsParsebleWithAddedToken(state, addedToken, new HashSet<TransitionWrapper>()))
                    CollectedSuggestions.Add(suggestion);
            }
        }

        private bool IsParsebleWithAddedToken(ATNState state, IToken newToken, ISet<TransitionWrapper> visitedTransitions)
        {
            if (newToken == null)
                return false;

            foreach (var t in state.Transitions)
            {
                if (t.IsEpsilon)
                {
                    var transWrapper = new TransitionWrapper(state, t);
                    if (visitedTransitions.Contains(transWrapper))
                        continue;

                    visitedTransitions.Add(transWrapper);
                    try
                    {
                        if (IsParsebleWithAddedToken(t.target, newToken, visitedTransitions))
                            return true;
                    }
                    finally
                    {
                        visitedTransitions.Remove(transWrapper);
                    }
                }
                else if (t is AtomTransition)
                {
                    if ((t as AtomTransition).label == newToken.Type)
                        return true;
                }
                else if (t is SetTransition)
                {
                    if ((t as SetTransition).Label.ToArray().Contains(newToken.Type))
                        return true;
                }
                else
                    throw new Exception("Unknown transition type");
            }

            return false;
        }

        private IToken GetAddedToken(string suggestedCompletion)
        {
            var completedText = Input + suggestedCompletion;
            var completedTextTokens = LexerWrapper.TokenizeNonDefaultChannel(completedText).Tokens;
            if (completedTextTokens.Count <= InputTokens.Count)
                return null;

            return completedTextTokens.Last();
        }

        private void FillParserTransitionLabels(ATNState state, ISet<int> result, ISet<TransitionWrapper> visitedTransitions)
        {
            foreach (var t in state.Transitions)
            {
                var transWrapper = new TransitionWrapper(state, t);
                if (visitedTransitions.Contains(transWrapper))
                    continue;

                if (t.IsEpsilon)
                {
                    try
                    {
                        visitedTransitions.Add(transWrapper);
                        FillParserTransitionLabels(t.target, result, visitedTransitions);
                    }
                    finally
                    {
                        visitedTransitions.Remove(transWrapper);
                    }
                }
                else if (t is AtomTransition)
                {
                    int label = (t as AtomTransition).label;
                    if (label > 0)
                        result.Add(label);
                }
                else if (t is SetTransition)
                {
                    foreach (var interval in (t as SetTransition).Label.GetIntervals())
                    {
                        for (int i = interval.a; i <= interval.b; ++i)
                            result.Add(i);
                    }
                }
            }
        }

        private bool HaveMoreTokens(int tokenIndex)
        {
            return tokenIndex < InputTokens.Count;
        }

        private int? SetParserStateLastVisitedOnThisTokenIndex(ATNState state, int? tokenListIndex)
        {
            int? res;
            if (!tokenListIndex.HasValue)
            {
                if (ParserStateToTokenListIndexWhereLastVisited.TryGetValue(state, out res))
                    ParserStateToTokenListIndexWhereLastVisited.Remove(state);
                return res;
            }

            ParserStateToTokenListIndexWhereLastVisited.TryGetValue(state, out res);
            ParserStateToTokenListIndexWhereLastVisited[state] = tokenListIndex;
            return res;
        }

        private bool DidVisitParserStateOnThisTokenIndex(ATNState state, int? currTokenIndex)
        {
            ParserStateToTokenListIndexWhereLastVisited.TryGetValue(state, out var lastVisitedThisStateAtTokenListIndex);
            return lastVisitedThisStateAtTokenListIndex.HasValue ? currTokenIndex.Value == lastVisitedThisStateAtTokenListIndex.Value : false;
        }

        private string Input { get; set; }
        private ParserWrapper ParserWrapper { get; set; }
        private Dictionary<ATNState, int?> ParserStateToTokenListIndexWhereLastVisited { get; set; } = new Dictionary<ATNState, int?>();
        private HashSet<string> CollectedSuggestions { get; } = new HashSet<string>();

        /// <summary> Gets states from which suggestions were made. </summary>
        private HashSet<ATNState> CollectedStates { get; } = new HashSet<ATNState>();
    }
}
