using Antlr4.Runtime.Atn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.AtnCompletion
{
    class TokenSuggester
    {
        public TokenSuggester(LexerWrapper lexer, string input) : this(input, lexer, CasePreference.Both)
        {
        }

        public TokenSuggester(string origPartialToken, LexerWrapper lexer, CasePreference casePreference)
        {
            OrigPartialToken = origPartialToken;
            Lexer = lexer;
            CasePreference = casePreference;
        }

        public IEnumerable<string> ExcludedRules { get; set; } = new List<string>();

        public IEnumerable<string> Suggest(IEnumerable<int> nextParserTransitionLabels)
        {
            foreach (int label in nextParserTransitionLabels)
            {
                if (!ExcludedRules.Contains(Lexer.FindRuleNameByRuleNumber(label - 1)))
                {
                    var state = Lexer.FindStateByRuleNumber(label - 1);
                    Suggest("", state, OrigPartialToken);
                }
            }
            return Suggestions;
        }

        private void Suggest(string tokenSoFar, ATNState state, string remainingText)
        {
            if (VisitedStates.Contains(state.stateNumber))
                return;

            VisitedStates.Add(state.stateNumber);
            try
            {
                var transitions = state.Transitions;
                if (tokenSoFar.Length > 0 && !transitions.Any())
                {
                    Suggestions.Add(ChopOffCommonStart(tokenSoFar, OrigPartialToken));
                    return;
                }

                foreach (var t in transitions)
                    SuggestViaLexerTransition(tokenSoFar, remainingText, t);
            }
            finally
            {
                VisitedStates.RemoveAt(VisitedStates.Count - 1);
            }
        }

        private void SuggestViaLexerTransition(string tokenSoFar, string remainingText, Transition transition)
        {
            if (transition.IsEpsilon)
                Suggest(tokenSoFar, transition.target, remainingText);
            else if (transition is AtomTransition)
            {
                var newTokenChar = char.ConvertFromUtf32((transition as AtomTransition).label);
                if (remainingText.Length == 0 || remainingText.StartsWith(newTokenChar))
                    SuggestViaNonEpsilonTransition(tokenSoFar, remainingText, newTokenChar, transition.target);
            }
            else if (transition is SetTransition)
            {
                var symbols = transition.Label.ToArray();
                foreach (int symbol in symbols)
                {
                    var charStr = char.ConvertFromUtf32(symbol);
                    bool ignoreCase = ShouldIgnoreThisCase(charStr[0], symbols);
                    if (!ignoreCase && (remainingText.Length == 0 || remainingText.StartsWith(charStr)))
                        SuggestViaNonEpsilonTransition(tokenSoFar, remainingText, charStr, transition.target);
                }
            }
        }

        private void SuggestViaNonEpsilonTransition(string tokenSoFar, string remainingText, string newTokenSoFar, ATNState state)
        {
            var newRemainingState = remainingText.Length > 0 ? remainingText.Substring(1) : remainingText;
            Suggest(tokenSoFar + newTokenSoFar, state, newRemainingState);
        }

        private bool ShouldIgnoreThisCase(char transChar, int[] allTransChars)
        {
            switch (CasePreference)
            {
                case CasePreference.Both:
                    return false;
                case CasePreference.Lower:
                    return char.IsUpper(transChar) && allTransChars.Contains(char.ToLower(transChar));
                case CasePreference.Upper:
                    return char.IsLower(transChar) && allTransChars.Contains(char.ToUpper(transChar));
            }
            return false;
        }

        private string ChopOffCommonStart(string a, string b)
        {
            return a.Substring(Math.Min(a.Length, b.Length));
        }

        private HashSet<string> Suggestions { get; } = new HashSet<string>();
        private LexerWrapper Lexer { get; set; }
        private string OrigPartialToken { get; set; }
        CasePreference CasePreference { get; }
        List<int> VisitedStates { get; } = new List<int>();
    }
}
