﻿using Re.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Wpf
{
    /// <summary> Provides methods for working with a QIF Rules document. </summary>
    class DocumentContext
    {
        /// <summary> Loads document from a file. </summary>
        /// <param name="path"> QIF file path </param>
        /// <returns> Created document context </returns>
        public static DocumentContext Load(string path)
        {
            if (Formats.Qif.Check(path))
                return LoadQif(path);
            else if (Formats.Rml.Check(path))
                return LoadRules(path);

            var ext = Path.GetExtension(path);
            throw new Exception($"Unknown file extension: {ext}");
        }

        /// <summary> Creates an empty QIF Rules document. </summary>
        public DocumentContext()
        {
            QifDocument = Document.Qif.CreateRulesEmpty();
        }

        /// <summary> Checks syntax errors in the specified rules language text. </summary>
        /// <param name="text"> Rules language text </param>
        /// <returns> Syntax errors </returns>
        public static IEnumerable<SyntaxError> CheckSyntax(string text)
        {
            var ast = new Rules.Ast(new Rules.Language(text));
            var errors = new List<SyntaxError>();
            errors.AddRange(ast.LexerErrors);
            errors.AddRange(ast.ParserErrors);
            return errors;
        }

        /// <summary> Saves the specified language text in QIF or rules language format. </summary>
        /// <param name="path"> File path to save </param>
        /// <param name="language"> Language to save </param>
        public void Save(string path, Language language)
        {
            if (path.ToLower().EndsWith("." + Formats.Qif.Extension))
                SaveQif(path, language);
            else if (path.ToLower().EndsWith("." + Formats.Rml.Extension))
                SaveRules(path, language);
            else
                throw new Exception("Unsupported format");
        }

        private static DocumentContext LoadQif(string path)
        {
            // load QIF document
            var qifDocument = Document.Qif.CreateFromFile(path);

            // prepare Rules QIF classes
            var qifRules = Qif.CreateFromDocument(qifDocument);

            // convert to language
            var language = Language.CreateFromQifRules(qifRules);

            return new DocumentContext
            {
                FileInfo = new FileInfo(path),
                QifDocument = Document.Qif.CreateFromFile(path),
                Language = language
            };
        }

        private static DocumentContext LoadRules(string path)
        {
            return new DocumentContext()
            {
                FileInfo = new FileInfo(path),
                QifDocument = Document.Qif.CreateRulesEmpty(),
                Language = new Language(File.ReadAllText(path))
            };
        }

        /// <summary> Saves the specified language text in rules language format. </summary>
        /// <param name="path"> File path to save </param>
        /// <param name="language"> Language to save </param>
        private void SaveRules(string path, Language language)
        {
            File.WriteAllText(path, Language.Text);
        }

        /// <summary> Saves the specified language text in QIF format. </summary>
        /// <param name="path"> File path to save </param>
        /// <param name="language"> Language to save </param>
        private void SaveQif(string path, Language language)
        {
            // parse language to AST
            var ast = new Ast(language);

            // convert AST to Rules QIF classes
            var qifRules2 = Qif.CreateFromAst(ast);

            // recreate Rule in QIF document by Rules QIF classes
            QifDocument.ResetRules(qifRules2);

            // save to QIF
            QifDocument.Write(path);
        }

        /// <summary> Gets file info of opened QIF document. </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary> Gets rules language instance that have been read from QIF document. </summary>
        public Language Language { get; private set; }

        private Document.Qif QifDocument { get; set; }
    }
}
