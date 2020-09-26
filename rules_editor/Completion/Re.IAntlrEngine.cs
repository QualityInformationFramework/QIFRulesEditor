using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    /// <summary> Interface for ANTLR based completion engines. </summary>
    interface IAntlrEngine
    {
        /// <summary> Finds continuations for the specified text. </summary>
        /// <param name="input"> Text </param>
        /// <returns> Found suggestions </returns>
        IEnumerable<string> Suggest(string input);

        /// <summary> Gets recognized tokens. </summary>
        IReadOnlyList<IToken> Tokens { get; }
    }
}
