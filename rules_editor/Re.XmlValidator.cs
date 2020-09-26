using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Re
{
    public class XmlValidator
    {
        public static XmlValidator Create(string[] schemas)
        {
            try
            {
                var validator = new XmlValidator();
                validator.m_schemaSet = new XmlSchemaSet();
                foreach (var schema in schemas)
                    validator.m_schemaSet.Add(null, new XmlTextReader(schema));
                return validator;
            }
            catch
            {
                return null;
            }
        }

        public bool Validate(XmlNode node) => Validate(Reader(node));

        public bool Validate(string xmlName) => Validate(Reader(xmlName));

        public bool HasErrors => m_errors.Count > 0;

        public bool HasWarnings => m_warnings.Count > 0;

        public string ToDebugText()
        {
            // Errors...
            // Warnings...
            var s = new StringBuilder();
            foreach (var err in m_errors)
                s.AppendLine($"Error: {err}");
            foreach (var warn in m_warnings)
                s.AppendLine($"Warning: {warn}");
            return s.ToString();
        }

        private XmlReader Reader(XmlNode node)
        {
            XmlNodeReader nodeReader = new XmlNodeReader(node);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(m_schemaSet);
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
            return XmlReader.Create(nodeReader, settings);
        }

        private XmlReader Reader(string xmlName)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(m_schemaSet);
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
            return XmlReader.Create(xmlName, settings);
        }

        private bool Validate(XmlReader reader)
        {
            m_errors.Clear();
            m_warnings.Clear();

            try
            {
                while ((m_maxErrorsCount < 0 || m_errors.Count < m_maxErrorsCount) && reader.Read()) ;
                reader.Close();
                return !HasErrors;
            }
            catch (Exception /*error*/)
            {
                return false;
            }
        }

        private void ValidationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
                m_errors.Add(args.Message);
            else
                m_warnings.Add(args.Message);
        }

        private XmlSchemaSet m_schemaSet;
        private int m_maxErrorsCount = 30;
        private List<string> m_errors = new List<string>();
        private List<string> m_warnings = new List<string>();
    }

}
