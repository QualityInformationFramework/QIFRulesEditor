﻿///////////////////////////////////////////////////////////////////////////////
///
/// Copyright 2018-2020, Capvidia, Metrosage, and project contributors
/// https://www.capvidia.com/
/// 
/// This software is provided for free use to the QIF Community under the 
/// following license:
/// 
/// Boost Software License - Version 1.0 - August 17th, 2003
/// https://www.boost.org/LICENSE_1_0.txt
/// 
/// Permission is hereby granted, free of charge, to any person or organization
/// obtaining a copy of the software and accompanying documentation covered by
/// this license (the "Software") to use, reproduce, display, distribute,
/// execute, and transmit the Software, and to prepare derivative works of the
/// Software, and to permit third-parties to whom the Software is furnished to
/// do so, all subject to the following:
/// 
/// The copyright notices in the Software and this entire statement, including
/// the above license grant, this restriction and the following disclaimer,
/// must be included in all copies of the Software, in whole or in part, and
/// all derivative works of the Software, unless such copies or derivative
/// works are solely in the form of machine-executable object code generated by
/// a source language processor.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
/// SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
/// FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
/// DEALINGS IN THE SOFTWARE.

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
