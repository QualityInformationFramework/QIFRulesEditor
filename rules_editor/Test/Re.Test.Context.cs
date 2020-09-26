using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Test
{
    //
    // Layout:
    //   <test>\
    //     <file>[.<suffix>].exam    - exam files
    //     <file>[.<suffix>].result  - result files
    //     <file>.~.<suffix>         - dump files
    //
    // Prefix      = <test>\<file>
    // Exam        = Prefix[.<suffix>].exam
    // Result      = Prefix[.<suffix>].result
    // GenDumpName = Prefix.~.<suffix>
    //
    public class Context
    {
        public static Context CreateForFile(string test, string file)
        {
            PrepareForTest(test);
            return new Context()
            {
                Prefix = Path.Combine(test, Path.GetFileName(file))
            };
        }

        public static Context CreateWithPrefix(string test, string prefix)
        {
            PrepareForTest(test);
            return new Context()
            {
                Prefix = Path.Combine(test, prefix)
            };
        }

        public static Context CreateSameDirectory(string testPath)
        {
            PrepareForTest(string.Empty);
            return new Context()
            {
                Prefix = Path.Combine(Path.GetDirectoryName(testPath), Path.GetFileNameWithoutExtension(testPath))
            };
        }

        // Prefix.~.<suffix>
        public string GenDumpName(string suffix) => Prefix + ".~." + suffix;

        public bool ExamStr(string str) => ExamStr("", str);

        public bool ExamStr(string suffix, string str)
        {
            var exam = Exam(suffix);
            var result = Result(suffix);

            // delete previous result
            File.Delete(result);

            if (File.Exists(exam))
            {
                // compare with existing exam
                string strExam = File.ReadAllText(exam);
                if (strExam != str)
                    File.WriteAllText(result, str);
                return strExam == str;
            }
            else
            {
                // create exam file
                File.WriteAllText(exam, str);
                return true;
            }
        }

        public bool ExamFile(string file) => ExamFile("", file);

        public bool ExamFile(string suffix, string file)
        {
            var exam = Exam(suffix);
            var result = Result(suffix);

            // delete previous result
            File.Delete(result);

            string str = File.ReadAllText(file);
            if (File.Exists(exam))
            {
                // compare with existing exam
                string strExam = File.ReadAllText(exam);
                if (strExam != str)
                {
                    File.Copy(file, result, true);
                    return false;
                }
                return true;
            }
            else
            {
                // create exam file
                File.Copy(file, exam, true);
                return true;
            }
        }

        // test prefix = <test>\<file>
        private string Prefix { get; set; }

        // Prefix[.<suffix>].result
        private string Result(string suffix)
        {
            var s = string.IsNullOrEmpty(suffix) ? "" : ("." + suffix);
            return Prefix + s + ".result";
        }

        // Prefix[.<suffix>].exam
        private string Exam(string suffix)
        {
            var s = string.IsNullOrEmpty(suffix) ? "" : ("." + suffix);
            return Prefix + s + ".exam";
        }

        private static void PrepareForTest(string test)
        {
            // set "en-us" culture and "." as decimal separator
            var ci = new System.Globalization.CultureInfo("en-us");
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            if (!string.IsNullOrEmpty(test))
                Directory.CreateDirectory(test);
        }
    }

}
