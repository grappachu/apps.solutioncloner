using System;
using System.Globalization;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class ThreeStateBoolConverter : IValueConverter
    {
        public ThreeStateBoolConverter()
        {
            TrueValue = true;
        }

        public bool NullValue { get; set; }
        public bool TrueValue { get; set; }
        public bool FalseValue { get; set; }

        /// <inheritdoc />
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var b = value as bool?;
            if (b == null)
            {
                return NullValue;
            }

            return b.Value ? TrueValue : FalseValue;
        }


        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value as bool?;
        }
    }
}