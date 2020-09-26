using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Rules
{
    /// <summary> Represents a syntax error. </summary>
    public class SyntaxError
    {
        /// <summary> Constructor. </summary>
        /// <param name="line"> Line index </param>
        /// <param name="pos"> Position in the line </param>
        /// <param name="error"> Error description </param>
        public SyntaxError(int line, int pos, string error)
        {
            Line = line;
            Position = pos;
            Error = error;
        }

        /// <summary> Gets or sets line index in the document. </summary>
        public int Line { get; set; }

        /// <summary> Gets or sets position in the line. </summary>
        public int Position { get; set; }

        /// <summary> Gets or sets error description. </summary>
        public string Error { get; set; }

        /// <summary> Gets string representation of the error. </summary>
        public override string ToString()
        {
            return $"line {Line}:{Position} {Error}";
        }
    }
}
