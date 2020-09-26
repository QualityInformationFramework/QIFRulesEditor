using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Rules
{
    public class Language
    {
        static public Language CreateFromFile(string fileName) => new Language(File.ReadAllText(fileName));
        static public Language CreateFromQifRules(Re.Rules.Qif qifRules) => new Language(qifRules.ToLanguageText());
        public Language() { }
        public Language(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
