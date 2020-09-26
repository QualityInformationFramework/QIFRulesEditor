using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Re.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Re.Wpf
{
    public class RulesEditorApp
    {
        /// <summary> Gets application instance. </summary>
        public static RulesEditorApp One { get; } = new RulesEditorApp();

        /// <summary> Gets formats which is available to open. </summary>
        public static Format[] SupportedFormatsOpen { get; } = new Format[] { Formats.Qif, Formats.Rml };

        /// <summary> Gets formats which is available to save. </summary>
        public static Format[] SupportedFormatsSave { get; } = new Format[] { Formats.Qif, Formats.Rml };

        /// <summary> Gets settings of the editor. </summary>
        public static SettingsViewModel Settings { get; } = new SettingsViewModel();

        /// <summary> Loads highlighting definition. </summary>
        /// <returns> Loaded highlighting definition </returns>
        public static IHighlightingDefinition LoadHighlightingDefinition()
        {
            string path = @"Data\RulesSyntax.xml";
            try
            {
                using (var xmlReader = System.Xml.XmlReader.Create(path))
                    return HighlightingLoader.Load(xmlReader, null);
            }
            catch (Exception e)
            {
                App.Logger.Error(e, "Failed to load highlighting definition");
                return null;
            }
        }

        /// <summary> Loads settings of the editor. </summary>
        public void LoadSettings()
        {
            if (!File.Exists(PathSettings))
                return;

            try
            {
                var xml = new XmlSerializer(typeof(SettingsViewModel));
                using (var sr = new StreamReader(PathSettings))
                    Settings.Apply(xml.Deserialize(sr) as SettingsViewModel);
            }
            catch (Exception e)
            {
                App.Logger.Error(e, "Failed to load settings");
            }
        }

        /// <summary> Loads settings of the editor. </summary>
        public void SaveSettings()
        {
            try
            {
                Directory.CreateDirectory(AppDataPath);
                var xml = new XmlSerializer(typeof(SettingsViewModel));
                using (var sw = new StreamWriter(PathSettings))
                    xml.Serialize(sw, Settings);
            }
            catch (Exception e)
            {
                App.Logger.Error(e, "Failed to save settings");
            }
        }

        /// <summary> Gets application name. </summary>
        public string Name => "Rules Editor";

        /// <summary> Gets path to application folder in %AppData%. </summary>
        public string AppDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Name);
        private RulesEditorApp() { }
        private string PathSettings => Path.Combine(AppDataPath, "options.xml");
    }
}
