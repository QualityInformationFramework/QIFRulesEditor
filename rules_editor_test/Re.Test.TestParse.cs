using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Re.Test
{
    [TestClass]
    public class TestParse
    {
        [DataTestMethod]
        [DataRow(@"lang_rules\comments.rules")]
        [DataRow(@"lang_rules\decision_class.1.rules")]
        [DataRow(@"lang_rules\decision_class.2.rules")]
        [DataRow(@"lang_rules\decision_class.3.rules")]
        [DataRow(@"lang_rules\decision_id.1.rules")]
        [DataRow(@"lang_rules\decision_make_model.1.rules")]
        [DataRow(@"lang_rules\decision_make_model.2.rules")]
        [DataRow(@"lang_rules\empty.rules")]
        [DataRow(@"lang_rules\fn.1.rules")]
        [DataRow(@"lang_rules\fn.2.rules")]
        [DataRow(@"lang_rules\fn.3.rules")]
        [DataRow(@"lang_rules\fn.4.rules")]
        [DataRow(@"lang_rules\if.1.rules")]
        [DataRow(@"lang_rules\if.2.rules")]
        [DataRow(@"lang_rules\if.3.rules")]
        [DataRow(@"lang_rules\if.4.rules")]
        [DataRow(@"lang_rules\sample.1.rules")]
        [DataRow(@"lang_rules\sample.2.rules")]
        [DataRow(@"lang_rules\sample.3.rules")]
        [DataRow(@"lang_rules\sample.5.rules")]
        [DataRow(@"lang_rules\sample.5.intervals.rules")]
        [DataRow(@"lang_rules\uuid.rules")]
        public void Test(string file)
        {
            var context = Context.CreateForFile("TestParse", file);

            // Workflow:
            //   text -> Language -> AST
            // 
            // Input:
            //   text
            //
            // Output:
            //   AST
            //
            // Exam:
            //   AST

            // load text from file
            var language = Re.Rules.Language.CreateFromFile(file);

            // parse and create AST
            var ast = new Re.Rules.Ast(language);
            var astDump = context.GenDumpName("ast");
            File.WriteAllText(astDump, ast.ToDebugText());

            // exam
            context.ExamFile(astDump);
        }
    }
}
