using System;
using System.Windows.Markup;

namespace Shimakaze.ToolKit.Csf.Markup
{
    public class SettingsExtension : MarkupExtension
    {
        public string Key { get; set; }
        public SettingsExtension() { }

        public SettingsExtension(string key) : this() => this.Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider) => Properties.Settings.Default[this.Key];

    }
}