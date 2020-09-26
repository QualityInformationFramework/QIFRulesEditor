using Antlr4.Runtime;
using Re.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    /// <summary> Completion engine that uses listener based completion algorithm. </summary>
    /// <seealso cref="Re.Completion.IAntlrEngine" />
    class ListenerEngine : IAntlrEngine
    {
        /// <summary> Gets recognized tokens. </summary>
        public IReadOnlyList<IToken> Tokens => mTokens ?? mEmptyTokens;

        /// <summary> Finds continuations for the specified text. </summary>
        /// <param name="input"> Text </param>
        /// <returns> Found suggestions </returns>
        public IEnumerable<string> Suggest(string input)
        {
            try
            {
                // run algorithm and find suggestions
                var lexer = new rulesLexer(new AntlrInputStream(input));
                var tokens = lexer.GetAllTokens();
                var parser = new rulesParser(new CommonTokenStream(new ListTokenSource(tokens)));
                var completion = new CompletionListener(tokens);
                parser.AddParseListener(completion);
                parser.dme_rules();
                mTokens = new List<IToken>(tokens);

                // suggestions are in single quotes now
                // remove them
                var result = new List<string>();
                foreach (var s in completion.Suggestions)
                    result.Add(s.StartsWith("\'") ? s.Substring(1, s.Length - 2) : s);

                return result;
            }
            catch
            {
                // TODO: log exception
                return Enumerable.Empty<string>();
            }
        }

        private List<IToken> mTokens;
        private readonly List<IToken> mEmptyTokens = new List<IToken>();
    }
}
