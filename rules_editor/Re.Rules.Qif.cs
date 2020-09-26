using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Re.Rules
{
    public class Qif
    {
        public static Qif CreateFromAst(Re.Rules.Ast ast)
        {
            return new Qif() { m_rules = ast.ToQifRules() };
        }

        public static Qif CreateFromDocument(Re.Document.Qif doc)
        {
            // doc -> rules node -> deserialize to Rules classes
            var xr = new XmlNodeReader(doc.Rules);
            var xs = new XmlSerializer(typeof(Re.Qif3.QIFRulesType));
            var rules = xs.Deserialize(xr) as Re.Qif3.QIFRulesType;
            return new Qif() { m_rules = rules };
        }

        public void WriteToXml(string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                var xs = new XmlSerializer(typeof(Qif3.QIFRulesType));
                xs.Serialize(sw, m_rules);
            }
        }

        public void Serialize(XmlWriter xw)
        {
            var xs = new XmlSerializer(typeof(Qif3.QIFRulesType));
            xs.Serialize(xw, m_rules);
        }

        public string ToLanguageText()
        {
            var visitor = new QifVisitorLanguage();
            return visitor.Accept(m_rules);
        }

        private Re.Qif3.QIFRulesType m_rules;
    }
}
