using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Rules
{
    public class Ast
    {
        public Ast(Language language)
        {
            // create lexer
            m_lexer = new Grammar.rulesLexer(new AntlrInputStream(language.Text));
            m_lexer.RemoveErrorListeners();
            m_lexer.AddErrorListener(m_lexerErrorListener);

            // create parser
            m_parser = new Grammar.rulesParser(new CommonTokenStream(m_lexer));
            m_parser.RemoveErrorListeners();
            m_parser.AddErrorListener(m_parserErrorListener);

            // parse
            m_root = m_parser.dme_rules();
        }

        public Re.Qif3.QIFRulesType ToQifRules()
        {
            var qifRules = new Re.Qif3.QIFRulesType();
            var visitor = new AstVisitorQif();
            qifRules.DMESelectionRules = visitor.Accept(m_root);
            return qifRules;
        }

        public string ToStringTree() => m_root.ToStringTree(m_parser);

        public IEnumerable<SyntaxError> LexerErrors => m_lexerErrorListener.Errors;

        public IEnumerable<SyntaxError> ParserErrors => m_parserErrorListener.Errors;

        public bool HasError()
        {
            Debug.Assert((m_lexerErrorListener.IsEmpty && m_parserErrorListener.IsEmpty) == (m_parser.NumberOfSyntaxErrors == 0));
            return !m_lexerErrorListener.IsEmpty || !m_parserErrorListener.IsEmpty;
        }

        public string ToDebugText()
        {
            //  [Lexer error: <lexer_error>
            //  ...]
            //  [Parser error: <parser_errors>
            //  ...]
            //  AST:
            //  <AST>
            var s = new StringBuilder();
            foreach (var err in LexerErrors)
                s.AppendLine($"Lexer error: {err}");
            foreach (var err in ParserErrors)
                s.AppendLine($"Parser error: {err}");
            s.AppendLine("AST:");
            s.Append(ToStringTree());
            return s.ToString();
        }

        private Re.Grammar.rulesLexer m_lexer;
        private Re.Grammar.rulesParser m_parser;
        private Re.Grammar.rulesParser.Dme_rulesContext m_root;
        private LexerErrorListener m_lexerErrorListener = new LexerErrorListener();
        private ParserErrorListener m_parserErrorListener = new ParserErrorListener();
    }

    public class LexerErrorListener : IAntlrErrorListener<int>
    {
        public virtual void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            m_errors.Add(new SyntaxError(line, charPositionInLine, msg));;
        }
        public bool IsEmpty => m_errors.Count == 0;
        public IEnumerable<SyntaxError> Errors => m_errors;

        private List<SyntaxError> m_errors = new List<SyntaxError>();
    }

    public class ParserErrorListener : IAntlrErrorListener<IToken>
    {
        public virtual void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            m_errors.Add(new SyntaxError(line, charPositionInLine, msg));
        }
        public bool IsEmpty => m_errors.Count == 0;
        public IEnumerable<SyntaxError> Errors => m_errors;

        private List<SyntaxError> m_errors = new List<SyntaxError>();
    }
}
