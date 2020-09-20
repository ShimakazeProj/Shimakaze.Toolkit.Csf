using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Shimakaze.Toolkit.Csf.Data;
using Shimakaze.Toolkit.Csf.Properties;

namespace Shimakaze.Toolkit.Csf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Resource.ResourceManager.I18NInitialize();
            ControlzEx.Theming.ThemeManager.Current.ThemeSyncMode = ControlzEx.Theming.ThemeSyncMode.SyncAll;
            ControlzEx.Theming.ThemeManager.Current.SyncTheme();

            base.OnStartup(e);
            this.ChangeThemeBaseColor("Dark");
            this.ChangeThemeColorScheme("Purple");
        }
    }
}
