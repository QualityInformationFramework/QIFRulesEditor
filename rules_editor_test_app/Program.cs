using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Re.TestApp
{
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application
        /// Arguments:
        /// rules_esitor_test_app.exe path - run the specified test
        /// rules_esitor_test_app.exe -f path - run all test in the specified folder
        /// </summary>
        /// <param name="args"> Command line arguments </param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments");
                return;
            }

            if (args.Length == 1)
                DoTest(args[0]);

            else if(args.Length == 2 && args[0] == "-f")
            {
                var files = Directory.GetFiles(args[1], "*.rules");
                foreach (var f in files)
                    DoTest(f);
            }
        }

        static void DoTest(string path)
        {
            Console.WriteLine($"Test {path}");
            Console.WriteLine();
            ConstantSets.Load("Data/ConstantSnippets.xml");
            Test.Tests.TestListenerCompletion(path);
        }
    }
}
