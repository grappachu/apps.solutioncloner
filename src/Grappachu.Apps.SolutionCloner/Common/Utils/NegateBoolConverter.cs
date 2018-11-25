using System;
using System.Globalization;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    [ValueConversion(typeof(bool?), typeof(bool?))]
    public class NegateBoolConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return null;
        }
    }
}