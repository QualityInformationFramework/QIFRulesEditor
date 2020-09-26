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
