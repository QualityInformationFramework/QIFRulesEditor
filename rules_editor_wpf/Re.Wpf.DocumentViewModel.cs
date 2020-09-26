using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Re.Rules;

namespace Re.Wpf
{
    /// <summary> Provides binding interface for a QIF document </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class DocumentViewModel : INotifyPropertyChanged, IDisposable
    {
        /// <summary> Creates a view model document based on QIF file. </summary>
        /// <param name="path"> QIF file path </param>
        /// <returns> Created view model document </returns>
        public static DocumentViewModel Load(string path)
        {
            var qifDocument = DocumentContext.Load(path);
            var textDocument = new TextDocument(new StringTextSource(qifDocument.Language.Text));
            return new DocumentViewModel(qifDocument, textDocument);
        }

        /// <summary> Creates an empty view model document. </summary>
        /// <returns> Created  view model document </returns>
        public static DocumentViewModel CreateEmpty()
        {
            return new DocumentViewModel(new DocumentContext(), new TextDocument());
        }

        /// <summary> Gets or sets modified state of the document. </summary>
        public bool IsModified
        {
            get { return mIsModified; }
            set
            {
                mIsModified = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsModified"));
            }
        }

        /// <summary> Gets file name of the document. </summary>
        public string FileName => QifDocument.FileInfo?.Name;

        /// <summary> Gets TextDocument instance which is used in the editor. </summary>
        public TextDocument TextDocument { get; private set; }

        /// <summary> Gets collection of syntax errors. </summary>
        public ObservableCollection<SyntaxError> SyntaxErrors { get; } = new ObservableCollection<SyntaxError>();

        /// <summary> Gets a value that indicates whether the document has syntax errors. </summary>
        public bool HasSyntaxErrors => SyntaxErrors.Count > 0;

        /// <summary> Occurs when a property value changes. Inherited from INotifyPropertyChanged. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Occurs when a new message was sent. </summary>
        public event EventHandler<Message> OnMessage;

        /// <summary> Occurs after syntax checked. </summary>
        public event EventHandler SyntaxChecked;

        /// <summary> Runs syntax checking of rules language. </summary>
        private void CheckSyntax()
        {
            Dispatcher.Invoke(() =>
            {
                // TODO: sometimes (hard to reproduce) SyntaxChecker.Errors == 0 which should not happen
                if (SyntaxChecker.Errors != null)
                {
                    SyntaxErrors.Clear();
                    foreach (var e in SyntaxChecker.Errors)
                        SyntaxErrors.Add(e);
                     SyntaxChecked?.Invoke(this, new EventArgs());
                }
            });
        }

        /// <summary> Saves the document. </summary>
        /// <param name="path"> File path to save </param>
        public void Save(string path)
        {
            CheckSyntax();
            if (Formats.Qif.Check(path) && HasSyntaxErrors)
            {
                OnMessage.Invoke(this, Message.CreateError("Saving failed. The document contains syntax error(s)"));
                return;
            }

            QifDocument.Save(path, new Language(TextDocument.Text));
            OnMessage.Invoke(this, Message.CreateInformation($"The document has been saved to {path}"));
        }

        public void Suggest(IList<ICompletionData> suggestions, Caret caret)
        {
            var input = TextDocument.GetText(new TextSegment() { StartOffset = 0, EndOffset = caret.Offset });
            var suggestedStrings = new RulesCompletion(CompletionEngine.Listener).Suggest(input);
            foreach (var s in suggestedStrings)
                suggestions.Add(new CompletionData(s));
        }

        public void InsertSnippet(int offset, Snippet snippet)
        {
            TextDocument.Insert(offset, snippet.Text);
        }

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            SyntaxChecker.Dispose();
        }

        private DocumentViewModel(DocumentContext documentContext, TextDocument textDocument)
        {
            Dispatcher = Dispatcher.CurrentDispatcher;

            QifDocument = documentContext;
            TextDocument = textDocument;

            SyntaxChecker = new SyntaxChecker(new AvalonTextSource(TextDocument, Dispatcher));
            SyntaxChecker.OnChecked += SyntaxChecker_OnChecked;
        }

        private void SyntaxChecker_OnChecked(object sender, EventArgs e)
        {
            CheckSyntax();
        }

        private Dispatcher Dispatcher { get; }
        private SyntaxChecker SyntaxChecker { get; set; }
        private DocumentContext QifDocument { get; set; }

        private bool mIsModified = false;
    }
}
