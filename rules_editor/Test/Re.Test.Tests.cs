using Antlr4.Runtime;
using Re.Completion;
using Re.Grammar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Re.Test
{
    public static class Tests
    {
        public static void TestListenerCompletion(string path)
        {
            var test = Context.CreateSameDirectory(path);
            var text = File.ReadAllText(path);
            Console.WriteLine("Text:");
            Console.WriteLine(text);

            var lexer = new rulesLexer(new AntlrInputStream(text));
            var tokens = lexer.GetAllTokens();
            var parser = new rulesParser(new CommonTokenStream(new ListTokenSource(tokens)));

            using (var logger = new StreamWriter(test.GenDumpName("listener.trace.~.xml")))
            {
                var completion = new CompletionListener(tokens) { Logger = logger };
                parser.AddParseListener(completion);
                parser.dme_rules();

                Console.WriteLine("Suggestions:");
                foreach (var s in completion.Suggestions)
                    Console.WriteLine(s);

                var result = test.GenDumpName("result.~.txt");
                File.WriteAllLines(result, completion.Suggestions);
                if (!test.ExamFile(result))
                    Console.WriteLine("Exam fail");
            }
        }

        public static void DumpGraph(string pathDump)
        {
            var nodes = new HashSet<string>();
            var edges = new HashSet<Tuple<string, string>>();
            int line = 0;
            using (var sr = new StreamReader(pathDump))
            {
                while (!sr.EndOfStream)
                {
                    ++line;
                    Console.WriteLine($"Line: {line}");
                    var path = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < path.Length; ++i)
                    {
                        nodes.Add(path[i]);
                        if (i < path.Length - 1)
                            edges.Add(new Tuple<string, string>(path[i], path[i + 1]));
                    }
                }
            }

            XDocument doc = new XDocument();
            var root = new XElement("DirectedGraph");
            doc.Add(root);

            var xNodes = new XElement("Nodes");
            int iNode = 0;
            foreach (var n in nodes)
            {
                ++iNode;
                Console.WriteLine($"Node {iNode} from {nodes.Count}");
                var elem = new XElement("Node", new XAttribute("Id", n));


                if (int.TryParse(n, out int id))
                {
                    var state = Grammar.rulesParser._ATN.states[id];
                    elem.Add(new XAttribute("Label", Grammar.rulesParser.ruleNames[state.ruleIndex]));
                }

                xNodes.Add(elem);
            }
            root.Add(xNodes);

            var xEdges = new XElement("Links");
            int iEdge = 0;
            foreach (var e in edges)
            {
                ++iEdge;
                Console.WriteLine($"Edge {iEdge} from {edges.Count}");
                xEdges.Add(new XElement("Link", new XAttribute("Source", e.Item1), new XAttribute("Target", e.Item2)));
            }
            root.Add(xEdges);

            doc.Save("output.dgml");
        }
    }
}
