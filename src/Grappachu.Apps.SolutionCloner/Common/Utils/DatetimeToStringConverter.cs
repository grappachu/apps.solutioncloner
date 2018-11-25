using System;
using System.Globalization;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    /// <summary>
    ///     Definisce un formattatore per <see cref="DateTime" /> personalizzabile
    /// </summary>
    /// <example>
    /// Creare un convertitore personalizzato tra le risorse risorse
    ///
    ///    <UserControl.Resources>
    ///       <local:DatetimeToStringConverter x:Key="CustomDateConverter" FormatString="dd MMM yyyy" NullValue="n/d" />
    ///    </UserControl.Resources>
    ///
    /// Utilizzare il controllo definito nel binding
    ///
    ///   <TextBox Text="{Binding RegistrationDate, Converter={StaticResource CustomDateConverter}}"   />
    /// 
    /// </example>
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DatetimeToStringConverter : IValueConverter
    {
        /// <summary>
        ///     Attiva una nuova istanza di <see cref="DatetimeToStringConverter" />
        /// </summary>
        public DatetimeToStringConverter()
        {
            FormatString = "dd MMM yyyy";
            NullValue = "n/d";
        }

        /// <summary>
        ///     Ottiene o imposta la stringa di formato predefinita per tutti i valori
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Ottiene una stringa che verrà utilizzata nei casi in cui il valore non è disponibile
        /// </summary>
        public string NullValue { get; set; }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return value != null ? ((DateTime)value).ToString(FormatString) : NullValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}