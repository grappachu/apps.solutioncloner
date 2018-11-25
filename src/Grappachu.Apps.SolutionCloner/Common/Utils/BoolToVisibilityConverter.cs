using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    /// <summary>
    ///     Definisce un convertitore di <see cref="Visibility" /> personalizzabile per i valori booleani
    /// </summary>
    /// <example>
    /// Creare un convertitore personalizzato tra le risorse risorse
    ///
    ///    <UserControl.Resources>
    ///       <local:BoolToVisibilityConverter x:Key="BoolToHiddenConverter" TrueValue="Visible" FalseValue="Hidden" />
    ///    </UserControl.Resources>
    ///
    /// Utilizzare il controllo definito nel binding
    ///
    ///   <Button Content="Click me!" Command="{Binding ClickMeCommand}"
    ///           Visibility="{Binding IsFreezed, Converter={StaticResource BoolToHiddenConverter}}" />
    /// 
    /// </example>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Attiva una nuova istanza di <see cref="BoolToVisibilityConverter" />
        /// </summary>
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Hidden;
        }

        /// <summary>
        ///     Ottiene o imposta il valore di <see cref="Visibility" /> da associare al valore true
        /// </summary>
        public Visibility TrueValue { get; set; }

        /// <summary>
        ///     Ottiene o imposta il valore di <see cref="Visibility" /> da associare al valore false
        /// </summary>
        public Visibility FalseValue { get; set; }

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