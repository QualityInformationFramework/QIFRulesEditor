using ICSharpCode.AvalonEdit.AddIn;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using ICSharpCode.SharpDevelop.Editor;
using Microsoft.Win32;
using Re.Completion;
using Re.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Re.Wpf
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            textEditor.SyntaxHighlighting = RulesEditorApp.LoadHighlightingDefinition();
            textEditor.TextArea.TextEntered += TextArea_TextEntered;
            textEditor.TextArea.Caret.PositionChanged += Caret_PositionChanged;
            RulesEditorApp.Settings.PropertyChanged += Settings_PropertyChanged;
            SearchPanel = SearchPanel.Install(textEditor.TextArea);

            CreateEmptyDocument();
        }

        void InitializeTextMarkerService()
        {
            // create marker service
            var textMarkerService = new TextMarkerService(textEditor.Document);
            textEditor.TextArea.TextView.BackgroundRenderers.Add(textMarkerService);
            textEditor.TextArea.TextView.LineTransformers.Add(textMarkerService);

            // register service
            var services = (IServiceContainer)textEditor.Document.ServiceProvider.GetService(typeof(IServiceContainer));
            if (services != null)
            {
                if (services.GetService(typeof(ITextMarkerService)) != null)
                    services.RemoveService(typeof(ITextMarkerService));

                services.AddService(typeof(ITextMarkerService), textMarkerService);
            }
            TextMarkerService = textMarkerService;
        }

        void RegisteterHandles()
        {
            textEditor.Document.TextChanged += Document_TextChanged;
        }

        private void Document_TextChanged(object sender, EventArgs e)
        {
            // TODO: binding?
            Document.IsModified = textEditor.IsModified;
        }

        private void ClearMarkers()
        {
            TextMarkerService.RemoveAll(m => true);
        }

        private void AddMarkers()
        {
            foreach (var e in Document.SyntaxErrors)
            {
                var pos = textEditor.Document.GetOffset(e.Line, e.Position);
                if (pos < Document.TextDocument.TextLength)
                {
                    var marker = TextMarkerService.Create(pos, 1);
                    marker.MarkerTypes = TextMarkerTypes.SquigglyUnderline;
                    marker.MarkerColor = Colors.Red;
                }
            }
        }

        private void CreateEmptyDocument()
        {
            DataContext = DocumentViewModel.CreateEmpty();
            Document.OnMessage += Document_OnMessage;
            Document.SyntaxChecked += Document_SyntaxChecked;
            textEditor.Document = Document.TextDocument;
            RegisteterHandles();
            InitializeTextMarkerService();
        }

        private void Document_SyntaxChecked(object sender, EventArgs e)
        {
            ClearMarkers();
            AddMarkers();
        }

        private void CloseDocument()
        {
            if (Document != null)
            {
                textEditor.Document.TextChanged -= Document_TextChanged;
                textEditor.Document = null;
                Document.OnMessage -= Document_OnMessage;
                textBlockMessage.Text = string.Empty;
                textBlockCaret.Text = string.Empty;
                (DataContext as IDisposable).Dispose();
                DataContext = null;
                ClearMarkers();
            }
        }

        private void LoadDocument(string path)
        {
            DataContext = DocumentViewModel.Load(path);
            Document.OnMessage += Document_OnMessage;
            Document.SyntaxChecked += Document_SyntaxChecked;
            textEditor.Document = Document.TextDocument;
            RegisteterHandles();
            InitializeTextMarkerService();
        }

        private void OpenDocument(string path)
        {
            if (Document != null && Document.IsModified)
            {
                var res = MessageBox.Show("There are unsaved changes that will be lost. Would you like to continue?", "Confirm opening", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res != MessageBoxResult.Yes)
                    return;
            }

            CloseDocument();

            try
            {
                LoadDocument(path);
                Title = $"{App.Name} - {Document.FileName}";
            }
            catch (Exception e)
            {
                var msg = "Cannot open file. Error:" + Environment.NewLine + e.Message;
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CreateEmptyDocument();
            }
        }

        private void NavigateToError(SyntaxError e)
        {
            var caret = textEditor.TextArea.Caret;
            caret.Offset = textEditor.Document.GetOffset(e.Line, e.Position);
            caret.Show();
            caret.BringCaretToView();
        }

        private void CheckSyntax()
        {
            ClearMarkers();
            AddMarkers();
        }

        private void Save(string path)
        {
            try
            {
                Document.Save(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowCompletionWindow()
        {
#if true
            var completionWindow = new CompletionWindow(textEditor.TextArea);
            Document.Suggest(completionWindow.CompletionList.CompletionData, textEditor.TextArea.Caret);
            completionWindow.Show();
#endif
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HighlightCurrentLine")
                textEditor.Options.HighlightCurrentLine = RulesEditorApp.Settings.HighlightCurrentLine;
        }

        private void Document_OnMessage(object sender, Message e)
        {
            textBlockMessage.Foreground = new SolidColorBrush(e.Type == MessageType.Error ? Colors.Red : Colors.Black);
            textBlockMessage.Text = e.Text;
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            var caret = textEditor.TextArea.Caret;
            textBlockCaret.Text = $"Ln {caret.Line}, Col {caret.Column}";
        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = Utilities.CreateFilter(RulesEditorApp.SupportedFormatsOpen),
                FilterIndex = RulesEditorApp.SupportedFormatsOpen.Length + 1
            };
            if (dialog.ShowDialog(this).GetValueOrDefault(false))
                OpenDocument(dialog.FileName);
        }

        private void textEditor_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                    OpenDocument(files[0]);
            }
        }

        private void buttonCompile_Click(object sender, RoutedEventArgs e)
        {
            CheckSyntax();
        }

        private void dataGridErrors_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = e.MouseDevice.DirectlyOver;
            if (element != null && element is FrameworkElement && ((FrameworkElement)element).Parent is DataGridCell)
            {
                if (dataGridErrors.SelectedItems != null && dataGridErrors.SelectedItems.Count == 1)
                {
                    if (dataGridErrors.SelectedItem is SyntaxError error)
                        NavigateToError(error);
                }
            }
        }

        private void buttonSaveQif_Click(object sender, RoutedEventArgs e)
        {
            if (Utilities.ShowSaveDialog(out string path, new[] { Formats.Qif }, this))
                Save(path);
        }

        private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (Utilities.ShowSaveDialog(out string path, RulesEditorApp.SupportedFormatsSave, this))
                Document.Save(path);
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            if (Document.IsModified)
            {
                var res = MessageBox.Show("There are unsaved changes that will be lost. Would you like to continue?", "Confirm opening", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res != MessageBoxResult.Yes)
                    return;
            }

            CloseDocument();
            CreateEmptyDocument();
        }

        private void buttonCut_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Cut();
        }

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Copy();
        }

        private void buttonPaste_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Paste();
        }

        private void buttonUndo_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Undo();
        }

        private void buttonRedo_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Redo();
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == " ")
                ShowCompletionWindow();
        }

        private void CommandBindingCompletion_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowCompletionWindow();
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            SearchPanel.Open();
        }

        private void buttonNewRule_Click(object sender, RoutedEventArgs e)
        {
            string snippet = @"dme_rule
{

}
";
            Document.InsertSnippet(textEditor.TextArea.Caret.Offset, new Snippet(snippet));
        }

        private void listBoxConstants_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem != null && listBox.SelectedItem is Constant constant)
                    Document.InsertSnippet(textEditor.TextArea.Caret.Offset, new Snippet(constant));
            }
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            var window = new SettingsWindow()
            {
                Owner = this
            };
            window.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var str = textEditor.Document.GetText(0, textEditor.CaretOffset);
            Console.WriteLine("Suggestions:");
            foreach (var s in Suggester3.Suggest(str))
                Console.WriteLine(s);
        }

        // not sure if binding works with private properties
        // TODO: figure it out
        public DocumentViewModel Document => DataContext as DocumentViewModel;
        private RulesEditorApp App { get; } = RulesEditorApp.One;
        private ITextMarkerService TextMarkerService { get; set; }
        private SearchPanel SearchPanel { get; }
    }
}
