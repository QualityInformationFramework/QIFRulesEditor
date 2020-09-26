using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Re.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    class ErrorListener : Antlr4.Runtime.IAntlrErrorListener<IToken>
    {
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            ExpectedTokens = e.GetExpectedTokens().ToArray();
        }

        public int[] ExpectedTokens { get; private set; }
    }

    /// <summary> Suggester that based on expectations of parser. </summary>
    public class Suggester3
    {
        public static IEnumerable<string> Suggest(string input)
        {
            var lexer = new rulesLexer(new Antlr4.Runtime.AntlrInputStream(input));
            var parser = new rulesParser(new CommonTokenStream(lexer));
            parser.RemoveErrorListeners();
            var errorListener = new ErrorListener();
            parser.AddErrorListener(errorListener);
            parser.dme_rules();

            var res = new List<string>();
            if (errorListener.ExpectedTokens != null)
            {
                foreach (var p in errorListener.ExpectedTokens)
                    res.Add(rulesLexer.DefaultVocabulary.GetDisplayName(p));
            }
            return res;
        }
    }
}
