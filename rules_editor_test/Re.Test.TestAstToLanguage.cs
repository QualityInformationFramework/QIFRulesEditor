using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Re.Test
{
    [TestClass]
    public class TestAstToLanguage
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
        // [DataRow(@"lang_rules\fn.1.rules")] - error test
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
        //[DataRow(@"lang_rules\sample.5.intervals.rules")] - experimental
        [DataRow(@"lang_rules\uuid.rules")]
        public void Test(string file)
        {
            var context = Context.CreateForFile("TestAstToLanguage", file);

            // TODO AST' -> Document -> QIF
            // TODO validate QIF

            // Workflow:
            //   text -> Language -> AST -> QIFRules -> Language' -> AST'
            //
            // Input:
            //   text
            //
            // Output:
            //   Language
            //   AST
            //   QIFRules
            //   Language'
            //   AST'
            //
            // Exam:
            //   Language'
            //   AST'
            //
            // Analysis:
            //   Lang <=> Lang'

            // load text from file
            var language = Re.Rules.Language.CreateFromFile(file);
            var langDump = context.GenDumpName("lang");
            File.WriteAllText(langDump, language.Text);

            // parse and create AST
            var ast = new Re.Rules.Ast(language);
            var astDump = context.GenDumpName("ast");
            File.WriteAllText(astDump, ast.ToDebugText());
            Assert.IsFalse(ast.HasError());

            // AST -> QIF Rules classes
            var qifRules = Re.Rules.Qif.CreateFromAst(ast);
            qifRules.WriteToXml(context.GenDumpName("xml"));

            // QIF Rules classes -> language2
            var language2 = Re.Rules.Language.CreateFromQifRules(qifRules);
            var lang2Dump = context.GenDumpName("lang2");
            File.WriteAllText(lang2Dump, language2.Text);

            // langiage2 -> AST2
            var ast2 = new Re.Rules.Ast(language2);
            var ast2Dump = context.GenDumpName("ast2");
            File.WriteAllText(ast2Dump, ast2.ToDebugText());
            Assert.IsFalse(ast2.HasError());

            // exam
            context.ExamFile("lang", lang2Dump);
            context.ExamFile("ast", ast2Dump);
        }
    }
}
