using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Re.Wpf
{
    class CompletionData : ICompletionData
    {
        public CompletionData(string text)
        {
            Text = text;
            TextToInsert = text;
        }

        public CompletionData(Re.CompletionData completion)
        {
            Text = completion.Text;
            TextToInsert = completion.Part;
        }

        public ImageSource Image => null;

        public string Text { get; private set; }

        public object Content => Text;

        /// <summary> TODO </summary>
        public object Description => Text;

        public double Priority => 1;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, TextToInsert);
        }

        private string TextToInsert { get; set; }
    }
}
