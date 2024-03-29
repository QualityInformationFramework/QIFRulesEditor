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

using Antlr4.Runtime;
using Re.AtnCompletion;
using Re.Completion;
using Re.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re
{
    /// <summary> Represents a completion suggestion. </summary>
    public class CompletionData
    {
        /// <summary> Creates a collection of completion data based on specified suggestion strings. </summary>
        /// <param name="suggestions"> Suggestion string </param>
        /// <returns> Collection of completion data </returns>
        public static IReadOnlyCollection<CompletionData> Create(IEnumerable<string> suggestions)
        {
            var result = new List<CompletionData>();
            foreach (var s in suggestions)
            {
                result.Add(new CompletionData()
                {
                    Text = s,
                    Part = s
                });
            }
            return result;
        }

        /// <summary> Gets or sets the whole completion word. </summary>
        public string Text { get; set; }

        /// <summary> Gets or sets the missed part that might be appended. </summary>
        public string Part { get; set; }
    }

    public enum CompletionEngine
    {
        Atn,
        Listener
    }

    /// <summary> Implements auto completion for Rules language. </summary>
    public class RulesCompletion
    {
        public RulesCompletion(CompletionEngine engine)
        {
            CompletionEngine = engine;
        }

        /// <summary> Suggests the next word after end of the specified input string. </summary>
        /// <param name="input"> Input string </param>
        /// <returns> Suggested words </returns>
        public IReadOnlyCollection<CompletionData> Suggest(string input)
        {
            Input = input;
            var autoCompletion = CreateSuggester();
            var result = autoCompletion.Suggest(input);
            InputTokens = autoCompletion.Tokens;

            if (SuggestForIdentifier(out var completions))
                return completions;

            return CompletionData.Create(result);
        }

        private static IReadOnlyCollection<CompletionData> CreatePartialSuggestions(IEnumerable<string> suggestions, string input)
        {
            var result = new List<CompletionData>();
            foreach (var s in suggestions)
            {
                if (s.StartsWith(input))
                {
                    result.Add(new CompletionData()
                    {
                        Text = s,
                        Part = s.Substring(input.Length)
                    });
                }
            }
            return result;
        }

        private IAntlrEngine CreateSuggester()
        {
            if (CompletionEngine == CompletionEngine.Atn)
                return new AtnEngine();
            else if (CompletionEngine == CompletionEngine.Listener)
                return new ListenerEngine();

            throw new Exception("Unknown engine type");
        }

        private bool SuggestForIdentifier(out IReadOnlyCollection<CompletionData> completions)
        {
            completions = null;
            var lastToken = InputTokens.Count > 0 ? InputTokens[InputTokens.Count - 1] : null;
            if (lastToken == null || lastToken.Type != rulesLexer.IDENTIFIER)
                return false;

            if (!TryRemoveLastToken(out string subInput))
                return false;

            var autoCompletion = CreateSuggester();
            completions = CreatePartialSuggestions(autoCompletion.Suggest(subInput), lastToken.Text);
            return true;
        }

        private bool TryRemoveLastToken(out string res)
        {
            res = null;
            if (InputTokens.Count == 0)
                return false;

            var index = InputTokens[InputTokens.Count - 1].StartIndex;
            if (index >= 0)
            {
                res = Input.Substring(0, index);
                return true;
            }

            return false;
        }

        private IReadOnlyList<IToken> InputTokens { get; set; }
        private string Input { get; set; }
        private static string[] ExcludedRules { get; } = new string[] { "HEX4", "UUID" };
        private static LexerFactory LexerFactory { get; } = new LexerFactory();
        private static ParserFactory ParserFactory { get; } = new ParserFactory();
        private CompletionEngine CompletionEngine { get; }
    }
}
