using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class EmptyToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Attiva una nuova istanza di <see cref="EmptyToVisibilityConverter" />
        /// </summary>
        public EmptyToVisibilityConverter()
        {
            OtherValue = Visibility.Visible;
            EmptyOrNullValue = Visibility.Hidden;
        }

        /// <summary>
        ///     Ottiene o imposta il valore di <see cref="Visibility" /> da associare al valore true
        /// </summary>
        public Visibility OtherValue { get; set; }

        /// <summary>
        ///     Ottiene o imposta il valore di <see cref="Visibility" /> da associare al valore false
        /// </summary>
        public Visibility EmptyOrNullValue { get; set; }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is IEnumerable && !((IEnumerable)value).OfType<object>().Any()) ) 
                return EmptyOrNullValue ;
            else
                return OtherValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}