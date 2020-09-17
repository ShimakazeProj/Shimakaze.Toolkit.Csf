using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Shimakaze.ToolKit.CSF.Data;

namespace Shimakaze.ToolKit.CSF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ControlzEx.Theming.ThemeManager.Current.ThemeSyncMode = ControlzEx.Theming.ThemeSyncMode.SyncAll;
            ControlzEx.Theming.ThemeManager.Current.SyncTheme();

            base.OnStartup(e);
            this.ChangeThemeBaseColor("Dark");
            this.ChangeThemeColorScheme("Purple");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.IO.File.WriteAllText("error.log", e.Exception.ToString());
            MessageBox.Show(e.Exception.ToString(), "Fatal Error");
        }
    }
}
