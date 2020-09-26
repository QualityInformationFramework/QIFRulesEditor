using ICSharpCode.AvalonEdit.Document;
using Re.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Re.Wpf
{
    /// <summary> Background error checker. </summary>
    class SyntaxChecker : IDisposable
    {
        /// <summary> Constructor. </summary>
        /// <param name="textSource"> Text source </param>
        public SyntaxChecker(ITextSource textSource)
        {
            ForceCheck = true;
            TextSource = textSource;
            TextSource.TextChanged += TextSource_TextChanged;
            LastInvalidation = DateTime.Now;

            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += Worker_DoWork;
            Worker.RunWorkerAsync();
        }

        /// <summary> Gets syntax errors. </summary>
        public IReadOnlyList<SyntaxError> Errors
        {
            get
            {
                lock (mLocker)
                    return mErrors;
            }
        }

        /// <summary> Occurs when the text checked. </summary>
        public event EventHandler OnChecked;

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            Stop();
            Worker.Dispose();
        }

        private void Invalidate()
        {
            lock (mLocker)
            {
                LastInvalidation = DateTime.Now;
                mErrors = null;
                TextModified = true;
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (!Worker.CancellationPending)
                {
                    if (IsNeededToCheck())
                        Check();
                    Thread.Sleep(Frequency);
                }

                e.Cancel = Worker.CancellationPending;
                FinishEvent.Set();
            }
            catch(Exception ex)
            {
                App.Logger.Error(ex, "Syntax Checker exception");
            }
        }

        private void Check()
        {
            string text;
            lock (mLocker)
            {
                TextModified = false;
                ForceCheck = false;
                text = TextSource.GetText();
            }

            var ast = new Ast(new Language(text));

            lock (mLocker)
            {
                if (!TextModified)
                {
                    mErrors = new List<SyntaxError>();
                    mErrors.AddRange(ast.LexerErrors);
                    mErrors.AddRange(ast.ParserErrors);
                    TextModified = false;
                }
            }

            OnChecked?.Invoke(this, EventArgs.Empty);
        }

        private bool IsNeededToCheck()
        {
            lock (mLocker)
            {
                return ForceCheck || (DateTime.Now - LastInvalidation > IdleSpan && TextModified);
            }
        }

        private void TextSource_TextChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Stop()
        {
            Worker.CancelAsync();
            FinishEvent.WaitOne();
        }

        private AutoResetEvent FinishEvent { get; } = new AutoResetEvent(false);
        private BackgroundWorker Worker { get; } = new BackgroundWorker();
        private DateTime LastInvalidation { get; set; }
        private bool TextModified { get; set; }
        private ITextSource TextSource { get; set; }
        private bool ForceCheck { get; set; }

        private List<SyntaxError> mErrors;
        private static readonly TimeSpan IdleSpan = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan Frequency = TimeSpan.FromSeconds(0.25);
        private readonly object mLocker = new object();
    }
}
