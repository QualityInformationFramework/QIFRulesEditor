using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Re.Wpf
{
    /// <summary> Interaction logic for SettingsWindow.xaml </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            OldSettings = RulesEditorApp.Settings.Clone();
            FontDialog.Apply += FontDialog_Apply;
        }

        private void FontDialog_Apply(object sender, EventArgs e)
        {
            ApplyFont();
        }

        private void buttonChooseFont_Click(object sender, RoutedEventArgs e)
        {
            var settings = RulesEditorApp.Settings;
            var currName = settings.FontName;
            var currSize = settings.FontSize;

            FontDialog.Font = new System.Drawing.Font(currName, (float)(currSize * 72.0 / 96.0));
            var res = FontDialog.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
                ApplyFont();
            else if (res == System.Windows.Forms.DialogResult.Cancel)
            {
                settings.FontName = currName;
                settings.FontSize = currSize;
            }
        }

        private void ApplyFont()
        {
            var settings = RulesEditorApp.Settings;
            settings.FontName = FontDialog.Font.Name;
            settings.FontSize = FontDialog.Font.Size * 96.0 / 72.0;
            settings.IsFontBold = (FontDialog.Font.Style & System.Drawing.FontStyle.Bold) == System.Drawing.FontStyle.Bold;
            settings.IsFontItalic = (FontDialog.Font.Style & System.Drawing.FontStyle.Italic) == System.Drawing.FontStyle.Italic;
        }

        private FontDialog FontDialog { get; } = new FontDialog()
        {
            ShowApply = true,
            ShowEffects = false
        };

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            IsOk = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!IsOk)
                RulesEditorApp.Settings.Apply(OldSettings);
        }

        private SettingsViewModel OldSettings { get; }

        /// <summary> Gets or sets a value indicating whether the OK has been clicked and no changes should be undone during closing. </summary>
        private bool IsOk { get; set; } = false;

    }
}