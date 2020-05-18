using Keeper.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Keeper.Desktop.Converters
{
    class DurationConverter : IValueConverter
    {
        private const string format = "hh\\:mm\\:ss";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is TimeEntry t && !t.StopTime.Equals(default))
                return t.StopTime.Subtract(t.StartTime).ToString(format);

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
