using System;
using System.Windows.Markup;

namespace Shimakaze.ToolKit.CSF.Markup
{
    public class I18nExtension : MarkupExtension
    {
        public string Key { get; set; }
        public I18nExtension() { }

        public I18nExtension(string key) : this() => this.Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider) => Properties.Resource.ResourceManager.GetString(this.Key);

    }
    public class SettingsExtension : MarkupExtension
    {
        public string Key { get; set; }
        public SettingsExtension() { }

        public SettingsExtension(string key) : this() => this.Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider) => Properties.Settings.Default[this.Key];

    }
}