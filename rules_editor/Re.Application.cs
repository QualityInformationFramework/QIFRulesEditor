using Antlr4.Runtime;
using System;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Re
{
    class Application
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 2)
                {
                    if (args[0] == "antlr")
                        testAntlr(args[1]);
                    else if (args[0] == "qif")
                        testQifRules(args[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private static Qif3.QIFRulesType readQifRules(string fileName)
        {
            Qif3.QIFRulesType rules = null;
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open))
                {
                    using (var xr = XmlReader.Create(fs))
                    {
                        xr.MoveToContent();
                        xr.ReadToDescendant("Rules");
                        var xs = new XmlSerializer(typeof(Qif3.QIFRulesType));
                        rules = xs.Deserialize(xr) as Qif3.QIFRulesType;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            return rules;
        }

        private static bool writeQifRules(Qif3.QIFRulesType rules, string fileName)
        {
            try
            {
                using (var sw = new StreamWriter(fileName))
                {
                    var xs = new XmlSerializer(typeof(Qif3.QIFRulesType));
                    xs.Serialize(sw, rules);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return false;
            }

            return true;
        }

        static void testQifRules(string fileName)
        {
            // read QIF Rules
            var rules = readQifRules(fileName);
            if (rules == null)
                return;

            // write QIF Rules
            writeQifRules(rules, fileName + ".~.qif");

            // TODO extract rules from XmlDocument
            // TODO place rules to XmlDocument
            // TODO convert rules objects to text

            //var xs = new XmlSerializer(typeof(Qif3.QIFRulesType));
            //var rules = xs.Deserialize(xr) as Qif3.QIFRulesType;
            //rules.DMESelectionRules.DMEDecisionRule[0].BooleanExpression = new Qif3.ConstantIsType() { val = Qif3.BooleanConstantEnumType.QIF_TRUE };
            //using (var sw = new StreamWriter(fileName + ".~.qif"))
            //{
            //    var xs1 = new XmlSerializer(typeof(Qif3.QIFRulesType));
            //    xs1.Serialize(sw, rules);
            //}
            //var fs = new FileStream(@"C:\~xsd\DMERules2.QIF", FileMode.Open);
            //var sr = new StreamReader(@"C:\~xsd\r1.qif");
            //var xs = new XmlSerializer(typeof(Qif3.QIFRulesType));
            //var rules = xs.Deserialize(sr) as Qif3.QIFRulesType;
            //var sr = new StreamReader(@"C:\~xsd\DMERules1.QIF");
            //var xs = new XmlSerializer(typeof(Qif3.QIFDocumentType));
            //var doc = xs.Deserialize(sr) as Qif3.QIFDocumentType;
        }

        static void testAntlr(string fileName)
        {
            // open and parse file
            // print AST

            string inputStr;
            using (var sr = new StreamReader(fileName))
            {
                inputStr = sr.ReadToEnd();
            }

            AntlrInputStream inputStream = new AntlrInputStream(inputStr);
            var lexer = new Grammar.rulesLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new Grammar.rulesParser(commonTokenStream);

            // print tree
            var tree = parser.dme_rules();
            Console.WriteLine("{0}", tree.ToStringTree(parser));
            Console.WriteLine("");

            //StringBuilder text = new StringBuilder();
            //Console.WriteLine("Input the chat.");
            //// to type the EOF character and end the input: use CTRL+D, then press <enter>
            //while ((input = Console.ReadLine()) != "\u0004")
            //{
            //    text.AppendLine(input);
            //}


            // visitor
            //SpeakVisitor visitor = new SpeakVisitor();
            //SpeakParser.ChatContext chatContext = speakParser.chat();
            //visitor.Visit(chatContext);
            //foreach (var line in visitor.Lines)
            //{
            //    Console.WriteLine("{0} has said {1}", line.Person, line.Text);
            //}
        }
    }

    //public class SpeakLine
    //{
    //    public string Person { get; set; }
    //    public string Text { get; set; }
    //}

    //public class SpeakVisitor : SpeakBaseVisitor<object>
    //{
    //    public List<SpeakLine> Lines = new List<SpeakLine>();
    //    public override object VisitLine(SpeakParser.LineContext context)
    //    {
    //        // NameContext name = context.name();
    //        var name = context.name();
    //        //OpinionContext opinion = context.opinion();
    //        var opinion = context.opinion();
    //        SpeakLine line = new SpeakLine() { Person = name.GetText(), Text = opinion.GetText().Trim('"') };
    //        Lines.Add(line);
    //        return line;
    //    }
    //}
}
