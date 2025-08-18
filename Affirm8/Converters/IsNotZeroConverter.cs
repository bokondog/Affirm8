using System.Globalization;

namespace Affirm8.Converters
{
    /// <summary>
    /// Converter to check if a number is not zero (for showing like counts)
    /// </summary>
    public class IsNotZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
