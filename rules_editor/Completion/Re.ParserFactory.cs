using Antlr4.Runtime;
using Re.AtnCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    /// <summary> Rules parser factory. </summary>
    /// <seealso cref="Re.AtnCompletion.IParserFactory" />
    class ParserFactory : IParserFactory
    {
        public Parser Create(ITokenStream tokens)
        {
            return new Grammar.rulesParser(tokens);
        }
    }
}
