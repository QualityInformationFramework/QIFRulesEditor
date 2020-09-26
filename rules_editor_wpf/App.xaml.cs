using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Re.Wpf
{
    /// <summary> Interaction logic for App.xaml </summary>
    public partial class App : Application
    {
        public static NLog.Logger Logger { get; } = NLog.LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            ConstantSets.Load("Data/ConstantSnippets.xml");
            SetupLogger();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            RulesEditorApp.One.SaveSettings();
            base.OnExit(e);
        }

        private void SetupLogger()
        {
            var target = NLog.LogManager.Configuration.AllTargets.FirstOrDefault(t => t.Name == "logfile");
            if (target is NLog.Targets.FileTarget ft)
                ft.FileName = Path.Combine(RulesEditorApp.One.AppDataPath, "app.log");
        }
    }
}
