using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Re.Wpf
{
    /// <summary>
    /// Implements ITextSource interface getting text from Avalon's document
    /// using dispatcher for getting text from threads that differs from the one where text source was created
    /// </summary>
    /// <seealso cref="Re.Wpf.ITextSource" />
    class AvalonTextSource : ITextSource
    {
        /// <summary> Constructor. </summary>
        /// <param name="textSource"> Text source </param>
        /// <param name="dispatcher"> Dispatcher of the thread where the text source was created </param>
        public AvalonTextSource(ICSharpCode.AvalonEdit.Document.IDocument document, Dispatcher dispatcher)
        {
            Document = document;
            Document.TextChanged += Document_TextChanged;
            Dispatcher = dispatcher;
        }

        public event EventHandler TextChanged;

        /// <summary> Override. Gets the text from the text source </summary>
        public string GetText()
        {
            string text = null;
            Dispatcher.Invoke(() =>
            {
                text = Document.Text;
            });
            return text;
        }

        private void Document_TextChanged(object sender, ICSharpCode.AvalonEdit.Document.TextChangeEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private ICSharpCode.AvalonEdit.Document.IDocument Document { get; }
        private Dispatcher Dispatcher { get; }
    }
}
