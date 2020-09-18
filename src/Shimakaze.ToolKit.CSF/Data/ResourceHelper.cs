using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace Shimakaze.ToolKit.Csf.Data
{
    public static class ResourceHelper
    {
        private static IEnumerable<string> names;
        private static ResourceManager resourceManager;

        public static void I18NInitialize(this ResourceManager resourceMgr)
        {
            resourceManager = resourceMgr ?? Properties.Resource.ResourceManager;
            resourceManager.GetString(string.Empty);
            names = resourceMgr?.GetResourceSet(CultureInfo.CurrentUICulture, false, true)
                               ?.Cast<DictionaryEntry>()
                                .Select(item => item.Key as string);
        }
        public static void I18NInitialize(this DependencyObject element)
        {
            if (!(element is UIElement target)) return;

            // 获取子元素
            foreach (var child in LogicalTreeHelper.GetChildren(element))
                I18NInitialize(child as DependencyObject);

            if (string.IsNullOrWhiteSpace(target.Uid)) return;

            foreach (var (name, property)
                in names.Select(name => (name, prefx: $"{target.Uid}."))
                                      .Where(i => i.name!.StartsWith(i.prefx!))
                                      .Select(i => (i.name, i.name.Substring(i.prefx.Length))))
            {
                var propertyInfo = element.GetType().GetProperty(property);
                var str = resourceManager.GetString(name, CultureInfo.CurrentUICulture);
                if (propertyInfo is null || string.IsNullOrWhiteSpace(str)) continue;
                var constructor = propertyInfo.PropertyType.GetConstructor(new[] { typeof(string) });
                var method = propertyInfo.PropertyType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public,
                                                             null, new[] { typeof(string) }, null);

                propertyInfo.SetValue(element, propertyInfo.PropertyType == typeof(string)
                                               ? str
                                               : constructor is null
                                                   ? method is null
                                                         ? str
                                                         : method.Invoke(null, new object[] { str })
                                                   : constructor.Invoke(new object[] { str }));
            }
        }
    }
}
