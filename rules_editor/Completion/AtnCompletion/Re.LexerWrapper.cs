using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.AtnCompletion
{
    class TokenizationResult
    {
        public IReadOnlyList<IToken> Tokens { get; set; }
        public string UntokenizedText { get; set; } = string.Empty;
    }

    class TokenizationErrorListener : IAntlrErrorListener<int>
    {
        public TokenizationErrorListener(string input)
        {
            Input = input;
        }

        public string UntokenizedText { get; private set; } = string.Empty;

        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            UntokenizedText = Input.Substring(charPositionInLine);
        }

        private string Input { get; set; }
    }

    class LexerWrapper
    {
        public LexerWrapper(ILexerFactory factory)
        {
            Factory = factory;
        }

        public IVocabulary Vocabulary => Lexer.Vocabulary;

        public TokenizationResult TokenizeNonDefaultChannel(string input)
        {
            var res = Tokenize(input);
            res.Tokens = res.Tokens.Where((t) => t.Channel == 0).ToList();
            return res;
        }

        public ATNState FindStateByRuleNumber(int ruleNumber)
        {
            return Lexer.Atn.ruleToStartState[ruleNumber];
        }

        public string FindRuleNameByRuleNumber(int ruleNumber)
        {
            return Lexer.RuleNames[ruleNumber];
        }

        private TokenizationResult Tokenize(string input)
        {
            Lexer = Factory.Create(new AntlrInputStream(input));
            var errorListener = new TokenizationErrorListener(input);
            Lexer.RemoveErrorListeners();
            Lexer.AddErrorListener(errorListener);
            return new TokenizationResult()
            {
                Tokens = Lexer.GetAllTokens().ToList(),
                UntokenizedText = errorListener.UntokenizedText
            };
        }

        Lexer Lexer
        {
            get
            {
                if (mLexer == null)
                    mLexer = Factory.Create(new AntlrInputStream(string.Empty));
                return mLexer;
            }
            set
            {
                mLexer = value;
            }
        }
        Lexer mLexer;

        ILexerFactory Factory { get; set; }
    }
}
