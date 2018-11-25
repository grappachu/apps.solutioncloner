using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    /// <summary>
    ///     Definisce un convertitore di <see cref="Visibility" /> personalizzabile per i valori string
    /// </summary>
    /// <example>
    /// Creare un convertitore personalizzato tra le risorse risorse
    ///
    ///    <UserControl.Resources>
    ///       <local:BoolToStringConverter x:Key="BoolToLabelConverter" TrueValue="Click" FalseValue="Clicked" />
    ///    </UserControl.Resources>
    ///
    /// Utilizzare il controllo definito nel binding
    ///
    ///   <ToggleButton Content="{Binding IsUnclicked, Converter={StaticResource BoolToLabelConverter}}" Command="{Binding ClickMeCommand}"  />
    /// 
    /// </example>
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class BoolToStringConverter : IValueConverter
    {
        /// <summary>
        ///     Attiva una nuova istanza di <see cref="BoolToStringConverter" />
        /// </summary>
        public BoolToStringConverter()
        {
            TrueValue = bool.TrueString;
            FalseValue = bool.FalseString;
        }

        /// <summary>
        ///     Ottiene o imposta il testo da associare al valore true
        /// </summary>
        public string TrueValue { get; set; }

        /// <summary>
        ///     Ottiene o imposta il testo da associare al valore false
        /// </summary>
        public string FalseValue { get; set; }

        /// <inheritdoc />
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ? TrueValue : FalseValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
    }
}