using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re
{
    public static class Util
    {
        public static string QuoteStr(string s)
        {
            // \ -> \\
            // " -> \"
            // s -> "s"
            return "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }

        public static string UnQuoteStr(string s)
        {
            Debug.Assert(s.Length >= 2);
            Debug.Assert(s.StartsWith("\""));
            Debug.Assert(s.EndsWith("\""));

            // "s" -> s
            // \" -> \"
            // \\ -> \
            return s.TrimEnd('"').TrimStart('"').Replace("\\\"", "\"").Replace("\\\\", "\\");
        }
    }
}
