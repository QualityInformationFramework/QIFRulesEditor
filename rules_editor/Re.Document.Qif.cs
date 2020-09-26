using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Re.Document
{
    public class Qif
    {
        // TODO create valid empty document (uuid, maxid, namespace...)

        public static Qif CreateFromFile(string fileName)
        {
            var doc = new Qif();
            doc.m_dom = new XmlDocument();
            doc.m_dom.Load(fileName);
            doc.NamespaceManager = new XmlNamespaceManager(doc.m_dom.NameTable);
            doc.NamespaceManager.AddNamespace("t", NamespaceUri);
            return doc;
        }

        public static Qif CreateRulesEmpty()
        {
            // create document
            var doc = new Qif();
            doc.m_dom = new XmlDocument();

            // initialize namespace manager
            doc.NamespaceManager = new XmlNamespaceManager(doc.m_dom.NameTable);
            doc.NamespaceManager.AddNamespace("t", NamespaceUri);

            // create root QIDDocument
            var root = doc.m_dom.CreateElement("QIFDocument", NamespaceUri);
            doc.m_dom.AppendChild(root);

            // add QPId
            var qpid = doc.m_dom.CreateElement("QPId", NamespaceUri);
            qpid.InnerText = Guid.NewGuid().ToString();
            root.AppendChild(qpid);

            // add Rules
            root.AppendChild(doc.m_dom.CreateElement("Rules", NamespaceUri));
            return doc;
        }

        public void ResetRules(Re.Rules.Qif qifRules)
        {
            // remove old rules element
            var rules = Rules;
            Rules.ParentNode.RemoveChild(rules);

            // create new rules element
            using (var xw = m_dom.DocumentElement.CreateNavigator().AppendChild())
            {
                xw.WriteWhitespace("");
                qifRules.Serialize(xw);
            }
            Debug.Assert(Rules != null);
        }

        public void Write(string fileName) => m_dom.Save(fileName);

        public bool Validate() => Validator().Validate(Root);

        public string ValidateMessages()
        {
            if (m_validator == null)
                return "";
            return Validator().ToDebugText();
        }

        public XmlNode Rules => m_dom.SelectSingleNode("t:QIFDocument/t:Rules", NamespaceManager);

        public XmlNode Root => m_dom.DocumentElement;

        private Re.XmlValidator Validator()
        {
            if (m_validator == null)
            {
                // TODO include schema to project, use relative path
                m_validator = Re.XmlValidator.Create(new string[1]
                {
                @"c:\projects\3dv_trunk\Data\qif3\QIFApplications\QIFDocument.xsd"
                });
            }
            return m_validator;
        }

        private XmlNamespaceManager NamespaceManager { get; set; }

        private const string NamespaceUri = "http://qifstandards.org/xsd/qif3";

        private XmlDocument m_dom;
        private Re.XmlValidator m_validator;

    }
}
