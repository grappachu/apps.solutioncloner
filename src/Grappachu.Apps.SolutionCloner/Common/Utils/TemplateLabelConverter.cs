using System;
using System.Globalization;
using System.Windows.Data;
using Grappachu.Apps.SolutionCloner.Engine.Model.Templates;

namespace Grappachu.SolutionCloner.Common.Utils
{
    [ValueConversion(typeof(TemplateInfo), typeof(string))]
    public class TemplateLabelConverter : IValueConverter
    {
        public TemplateLabelConverter()
        {
            NullValue = "(Seleziona)";
            Pattern = "{0} v.{1} (by {2})";
        }

        public string NullValue { get; set; }
        public string Pattern { get; set; }

        /// <inheritdoc />
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var t = value as TemplateInfo;
            if (t == null)
            {
                return NullValue;
            }

            return string.Format(Pattern, t.Name, t.Version, t.Author);
        }


        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}