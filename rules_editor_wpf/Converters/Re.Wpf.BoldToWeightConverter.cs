using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Re.Wpf.Converters
{
    class BoldToWeightConverter : IValueConverter
    {
        /// <summary> Is bold (boolean) -> Weight.</summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isBold = (bool)value;
            return isBold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Regular;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
