using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    /// <summary>
    ///     Definisce un convertitore di <see cref="Visibility" /> personalizzabile per i valori null/not null
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
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Attiva una nuova istanza di <see cref="NullToVisibilityConverter" />
        /// </summary>
        public NullToVisibilityConverter()
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? TrueValue : FalseValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}