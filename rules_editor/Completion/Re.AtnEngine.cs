using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    /// <summary> Completion engine implementation using ATN based completion algorithm. </summary>
    /// <seealso cref="Re.Completion.IAntlrEngine" />
    class AtnEngine : IAntlrEngine
    {
        /// <summary> Gets recognized tokens. </summary>
        public IReadOnlyList<IToken> Tokens => Engine.InputTokens;

        /// <summary> Finds continuations for the specified text. </summary>
        /// <param name="input"> Text </param>
        /// <returns> Found suggestions </returns>
        public IEnumerable<string> Suggest(string input)
        {
            Engine = new Re.AtnCompletion.AutoCompletion(new LexerFactory(), new ParserFactory(), input);
            return Engine.Suggest();
        }

        AtnCompletion.AutoCompletion Engine { get; set; }
    }
}
