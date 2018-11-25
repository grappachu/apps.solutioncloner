using System;
using System.Windows;
using System.Windows.Data;

namespace Grappachu.SolutionCloner.Common.Utils
{
    //[ValueConversion(typeof(object), typeof(Visibility))]
    //public class IgnoreNewItemPlaceholderConverter : IValueConverter
    //{
    //    public Visibility Visibility { get; set; }

    //    public IgnoreNewItemPlaceholderConverter()
    //    {
    //        Visibility = Visibility.Hidden;
    //    }

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value != null && value.ToString() == "{NewItemPlaceholder}")
    //            return Visibility;
    //        return Visibility.Visible;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    public class IgnoreNewItemPlaceholderConverter : IValueConverter {
        public static readonly IgnoreNewItemPlaceholderConverter Instance = new IgnoreNewItemPlaceholderConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value != null && value.ToString() == "{NewItemPlaceholder}")
                return DependencyProperty.UnsetValue;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}