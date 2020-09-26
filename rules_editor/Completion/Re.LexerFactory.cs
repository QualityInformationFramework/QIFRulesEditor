using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    /// <summary> Rules lexer factory. </summary>
    /// <seealso cref="Re.AtnCompletion.ILexerFactory" />
    class LexerFactory : AtnCompletion.ILexerFactory
    {
        public Lexer Create(ICharStream chars)
        {
            return new Grammar.rulesLexer(chars);
        }
    }
}
