using Microsoft.VisualStudio.TestTools.UnitTesting;
using Re.Completion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Test
{
    /// <summary>
    /// Input
    ///     Rules Language string
    /// Output & exam
    ///     Array of suggested strings
    /// </summary>
    /// [TestClass]
    public class TestCodeCompletion
    {
        public TestCodeCompletion()
        {
            ConstantSets.Load("Data/ConstantSnippets.xml");
        }

        [DataTestMethod]
        [DataRow("", "empty")]
        [DataRow("dme_r", "dme_r")]
        [DataRow("dme_rule", "dme_rule")]
        [DataRow("dme_rule { if ", "if")]
        [DataRow("dme_rule { if characteristic_is(", "characteristic_is")]
        [DataRow("dme_rule { if ch", "ch")]
        public void Test(string text, string name)
        {
            var context = Context.CreateWithPrefix("TestAutoCompletion", name);

            var suggestions = new RulesCompletion(CompletionEngine.Atn).Suggest(text);

            var path = context.GenDumpName("suggestions");
            DumpSuggestions(suggestions, path);

            context.ExamFile(path);
        }

        [DataTestMethod]
        [DataRow("TestAutoCompletion/test1.rules")]
        public void TestFromFile(string file)
        {
            var context = Context.CreateForFile("TestAutoCompletion", file);
            var text = File.ReadAllText(file);
            var suggestions = new RulesCompletion(CompletionEngine.Atn).Suggest(text);

            var dump = context.GenDumpName("suggestions");
            DumpSuggestions(suggestions, dump);

            context.ExamFile(dump);
        }

        [TestMethod]
        //[DataRow("", "empty")]
        //[DataRow("dme_rule {", "empty")]
        //[DataRow("dme_rule { if characteristic_is( ANGLE", "empty")]
        //[DataRow("dme_rule { dme_class may CARTESIAN_CMM with CartesianWorkingVolume/XAxisLength >= 800", "with")]
        public void TestCompletion2(string input, string name)
        {
            var context = Context.CreateWithPrefix("TestAutoCompletion2", name);

            var suggestions = new Suggester().Run(input);

            var path = context.GenDumpName("suggestions");
            DumpSuggestions(suggestions, path);

            context.ExamFile(path);
        }

        [TestMethod]
        [DataRow("TestAutoCompletion/test1.rules")]
        public void TestCompletion2File(string file)
        {
            var context = Context.CreateForFile("TestAutoCompletion", file);
            var text = File.ReadAllText(file);
            //var suggestions = new Suggestion.Suggester().Run(text);
            //Re.Util.TestListener(text);

            var dump = context.GenDumpName("suggestions");
            //DumpSuggestions(suggestions, dump);

            context.ExamFile(dump);
        }

        private void DumpSuggestions(IEnumerable<string> suggestions, string path)
        {
            File.WriteAllLines(path, suggestions);
        }

        private void DumpSuggestions(IEnumerable<CompletionData> suggesstions, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                foreach (var s in suggesstions)
                {
                    sw.WriteLine(s.Text);
                    sw.WriteLine(s.Part);
                }
            }
        }
    }
}
