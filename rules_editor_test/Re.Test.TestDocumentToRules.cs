using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Re.Test
{
    [TestClass]
    public class TestDocumentToRules
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
            var context = Context.CreateForFile("TestDocumentToRules", file);

            // Workflow:
            //   QIF -> Document -> QIFRules -> Document' -> QIF'
            //
            // Input:
            //   QIF
            //
            // Output:
            //   QIFRules
            //   QIF'
            //
            // Exam:
            //   QIF'
            //
            // Analysis:
            //   QIF <=> QIF'

            // load document
            var doc = Re.Document.Qif.CreateFromFile(file);

            // create qif classes from DOM
            var qifRules = Re.Rules.Qif.CreateFromDocument(doc);
            qifRules.WriteToXml(context.GenDumpName("rules"));

            // re-create DOM rules node
            doc.ResetRules(qifRules);
            var qifDump = context.GenDumpName("qif");
            doc.Write(qifDump);

            // validate QIF'
            //var resValidate = doc.Validate();
            //var msgValidate = doc.ValidateMessages();
            //File.WriteAllText(context.GenDumpName("validate"), msgValidate);

            // exam
            context.ExamFile(qifDump);
            //Assert.IsTrue(resValidate);
        }
    }
}
