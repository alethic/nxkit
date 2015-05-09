using System;
using System.Globalization;
using System.Windows.Data;

namespace NXKit.XForms.Layout.View.Windows
{

    public class SmallFontSizeConverter :
        IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
                return (double)value - 2;
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
                return (double)value + 2;
            else
                return value;
        }

    }

}
