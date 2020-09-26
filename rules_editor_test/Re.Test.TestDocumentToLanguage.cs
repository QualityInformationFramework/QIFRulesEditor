using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Re.Test
{
    [TestClass]
    public class TestDocumentToLanguage
    {
        [DataTestMethod]
        [DataRow(@"qif_rules\DMERules1.QIF")]
        [DataRow(@"qif_rules\DMERules2.QIF")]
        [DataRow(@"qif_rules\DMERules3.QIF")]
        [DataRow(@"qif_rules\DMERules4.QIF")]
        [DataRow(@"qif_rules\DMERules5.QIF")]
        [DataRow(@"qif_rules\docRules.QIF")]
        [DataRow(@"qif_rules\featureRules1.QIF")]
        [DataRow(@"qif_rules\featureRules2.QIF")]
        [DataRow(@"qif_rules\featureRules3.QIF")]
        [DataRow(@"qif_rules\featureRules4.QIF")]
        public void Test(string file)
        {
            var context = Context.CreateForFile("TestDocumentToLanguage", file);

            // TODO QifRules' -> QIF'
            // TODO validate QIF'
            //
            // Workflow:
            //   QIF -> Document -> QIFRules -> Language -> AST -> QIFRules'
            //
            // Input:
            //   QIF
            //
            // Output:
            //   QIF
            //   Language
            //   AST
            //   QIFRules'
            //
            // Exam:
            //   Language
            //   AST
            //   QIFRules'
            //
            // Analysis:
            //   QIF <=> QIFRules'

            // open QIF document
            var doc = Re.Document.Qif.CreateFromFile(file);
            var qifDump = context.GenDumpName("qif");
            doc.Write(qifDump);

            // prepare Rules QIF classes
            var qifRules = Re.Rules.Qif.CreateFromDocument(doc);

            // convert to language
            var language = Re.Rules.Language.CreateFromQifRules(qifRules);
            var langDump = context.GenDumpName("lang");
            File.WriteAllText(langDump, language.Text);

            // parse language to AST
            var ast = new Re.Rules.Ast(language);
            var astDump = context.GenDumpName("ast");
            File.WriteAllText(astDump, ast.ToDebugText());

            // convert AST to Rules QIF classes
            var qifRules2 = Re.Rules.Qif.CreateFromAst(ast);

            // recreate Rule in QIF document by Rules QIF classes
            doc.ResetRules(qifRules2);
            var qifDump2 = context.GenDumpName("qif2");
            doc.Write(qifDump2);

            // exam
            context.ExamFile("lang", langDump);
            context.ExamFile("ast", astDump);
            context.ExamFile("qif", qifDump2);
        }
    }
}
