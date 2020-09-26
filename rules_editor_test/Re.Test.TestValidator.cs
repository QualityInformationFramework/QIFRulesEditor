using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Re.Test
{
    [TestClass]
    public class TestDocumentValidator
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
            var context = Context.CreateForFile("TestValidator", file);

            // Workflow:
            //   QIF -> Validate
            //
            // Input:
            //   QIF
            //
            // Output:
            //   Validator errors and warnings
            //
            // Exam:
            //   Validator errors and warnings

            // load document
            var doc = Re.Document.Qif.CreateFromFile(file);

            // validate QIF
            doc.Validate();
            var msg = doc.ValidateMessages();

            // exam
            context.ExamStr(msg);
        }
    }
}
