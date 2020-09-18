using System;
using System.Windows.Markup;

namespace Shimakaze.ToolKit.Csf.Markup
{
    public class I18nExtension : MarkupExtension
    {
        public string Key { get; set; }
        public I18nExtension() { }

        public I18nExtension(string key) : this() => this.Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider) => Properties.Resource.ResourceManager.GetString(this.Key);
    }
}