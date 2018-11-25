using System;
using System.Globalization;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value == null;
        }


        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}