using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Re.Wpf.Converters
{
    class ItalicToStyleConverter : IValueConverter
    {
        /// <summary> Is italic (boolean) -> Style. </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isItalic = (bool)value;
            return isItalic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
