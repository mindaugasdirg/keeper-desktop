using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Keeper.Desktop.Converters
{
    class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(value is null) && value.Equals(true) ? "Yes" : "No";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value.Equals("Yes");
    }
}
