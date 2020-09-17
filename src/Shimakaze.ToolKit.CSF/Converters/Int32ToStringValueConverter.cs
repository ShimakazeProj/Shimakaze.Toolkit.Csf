using System;
using System.Globalization;
using System.Windows.Data;

namespace Shimakaze.ToolKit.CSF.Converters
{
    public class Int32ToStringValueConverter : IValueConverter
    {
        public static IValueConverter Instance { get; } = new Int32ToStringValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value?.ToString();
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            int.TryParse(value as string, out var result) ? result : 0;
    }
}