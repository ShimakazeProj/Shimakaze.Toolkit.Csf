using Shimakaze.ToolKit.Csf.ViewModel;

using System;
using System.Globalization;
using System.Windows.Data;

namespace Shimakaze.ToolKit.Csf.Converters
{
    public class ClassGroupDescription:IValueConverter
    {
        public static ClassGroupDescription CreateInstance() => new ClassGroupDescription();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CsfLabelViewModel lbl) return lbl.Class;
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}