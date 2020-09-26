using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.AtnCompletion
{
    interface IParserFactory
    {
        Parser Create(ITokenStream tokens);
    }
}
