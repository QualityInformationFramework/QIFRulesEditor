using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.AtnCompletion
{
    class ParserWrapper
    {
        public ParserWrapper(IParserFactory factory, IVocabulary vocabulary)
        {
            var parser = factory.Create(null);
            Atn = parser.Atn;
            RuleNames = parser.RuleNames;

            Vocabulary = vocabulary;
        }

        public ATNState AtnState(int i)
        {
            return Atn.states[i];
        }

        IVocabulary Vocabulary { get; set; }
        ATN Atn { get; set; }
        string[] RuleNames { get; set; }
    }
}
