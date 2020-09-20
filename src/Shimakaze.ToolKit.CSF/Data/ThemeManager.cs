using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using ControlzEx.Theming;

namespace Shimakaze.Toolkit.Csf.Data
{
    public static class ThemeManager
    {
        public static void SetTheme()
        {
        }

        public static Theme ChangeThemeBaseColor(this Application app, string baseColor)
            => ControlzEx.Theming.ThemeManager.Current.ChangeThemeBaseColor(app, baseColor);


        public static Theme ChangeThemeColorScheme(this Application app, string baseColor)
            => ControlzEx.Theming.ThemeManager.Current.ChangeThemeColorScheme(app, baseColor);
    }
}
